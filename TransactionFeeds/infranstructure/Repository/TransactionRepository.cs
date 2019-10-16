using Abstracts;
using Abstracts.ModelBase;
using Dapper;
using FastMember;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infranstructure.Repository
{
    public class TransactionRepository : IRepository
    {
        private readonly string _conntectionString;
        readonly ILogger<TransactionRepository> _logger;


        public TransactionRepository(
            IOptions<TransactionRepositoryOptions> options,
            ILogger<TransactionRepository> logger
            )
        {
            _conntectionString = options.Value.ConnectionString;
            _logger = logger;
        }

        public async Task<List<QueryResponse>> GetTransactions(QueryRequest request)
        {
            List<QueryResponse> items = null;

            var queryParams = new StringBuilder();
            if (!string.IsNullOrEmpty(request.Currency))
            {
                queryParams.Append("currencyCode = @Currency and ");
            }
            if (request.StartDate != DateTime.MinValue
                && request.EndDate != DateTime.MinValue)
            {
                queryParams.Append("transactionDate between @StartDate and @EndDate and ");
            }

            if (request.Status != TransactionStatus.None)
            {
                queryParams.Append("transactionStatus = @Status and ");
            }
            if (queryParams.Length > 0)
                queryParams.Remove(queryParams.Length - 5, 5);//remove the 'and' in the end

            var whereQuery = "where " + queryParams.ToString();

            using (var connection = new SqlConnection(_conntectionString))
            {
                connection.Open();
                var query = @"SELECT 
                                TransactionIdentificator AS [id],
                                CONVERT(VARCHAR(100),CAST( Amount AS NUMERIC(18,2))) + ' ' + CurrencyCode AS [payment],
                                TransactionStatus AS [Status]
                              FROM [Transaction] " + whereQuery;

                items = (await connection.QueryAsync<QueryResponse>(query, request)).ToList();
            }


            return items;
        }

        public async Task Save(List<TransactionModel> transactions)
        {
            int affectdRecords = 0;

            using (var connection = new SqlConnection(_conntectionString))
            {
                SqlTransaction trans = null;

                try
                {
                    connection.Open();
                    trans = connection.BeginTransaction();
                    int chunkSize = 500;

                    var chunks = transactions.Select((x, j) => new { index = 1, value = x })
                                               .GroupBy(x => x.index / chunkSize)
                                               .Select(x => x.Select(v => v.value).ToList())
                                               .ToList();
                    foreach (var chunk in chunks)
                    {

                        using (var sqlCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, trans)
                        {
                            DestinationTableName = "[dbo].[TRANSACTION]",
                            BatchSize = chunk.Count,
                            NotifyAfter = chunk.Count
                        })
                        {
                            sqlCopy.SqlRowsCopied += (sender, args) => { affectdRecords += (int)args.RowsCopied; };
                            sqlCopy.ColumnMappings.Add("TransactionIdentificator", "TransactionIdentificator");
                            sqlCopy.ColumnMappings.Add("Amount", "Amount");
                            sqlCopy.ColumnMappings.Add("CurrencyCode", "CurrencyCode");
                            sqlCopy.ColumnMappings.Add("TransactionDate", "TransactionDate");
                            sqlCopy.ColumnMappings.Add("Status", "TransactionStatus");

                            var reader = ObjectReader.Create(chunk);
                            {
                                await sqlCopy.WriteToServerAsync(reader);
                            }
                        }
                    }

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans?.Rollback();
                    trans?.Dispose();
                    _logger.Log(LogLevel.Error, ex.Message);
                }
                finally
                {
                    trans?.Dispose();
                }
            }

            _logger.Log(LogLevel.Information, $"Inserted Transactions: {affectdRecords} records");
        }
    }
}
