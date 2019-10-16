using Abstracts;
using Abstracts.ModelBase;
using infranstructure.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class RepositoryTest
    {
        private static IRepository defaultRepository;
        public static IRepository DefaultRepository
        {
            get
            {
                if (defaultRepository == null)
                {
                    var tranRepoOption = Options.Create(
                                         new TransactionRepositoryOptions
                                         {
                                             ConnectionString = @"Server=localhost\SQLEXPRESS;Database=TransactionFeed;Trusted_Connection=True;"
                                         });

                    var Mocklogger = new Mock<ILogger<TransactionRepository>>();

                    defaultRepository = new TransactionRepository(tranRepoOption, Mocklogger.Object);
                }
                return defaultRepository;
            }
        }

        [Fact]
        public async Task SelectTransactionShouldBeOk()
        {

            var result = await DefaultRepository.GetTransactions(new QueryRequest
            {
                Currency = "USD",
                EndDate = new System.DateTime(2019, 10, 19),
                StartDate = new System.DateTime(2019, 10, 10),
                Status = TransactionStatus.R
            });

            Assert.Single(result);
            Assert.Equal("Invoice0000002", result[0].Id);
            Assert.Equal("300.00 USD", result[0].Payment);
            Assert.Equal("R", result[0].Status.ToString());

        }

        [Fact]
        public async Task SaveTransactionShouldBeOk()
        {
            var id1 = Guid.NewGuid().ToString();
            var id2 = Guid.NewGuid().ToString();
            var list = new List<TransactionModel>
            {
                new TransactionModel
                {
                    Amount = 220.00m,
                    CurrencyCode = "USD",
                    Status = TransactionStatus.A,
                    TransactionDate = new System.DateTime(2019, 01, 05),
                    TransactionIdentificator = id1
                },
                new TransactionModel
                {
                    Amount = 300.00m,
                    CurrencyCode = "EUR",
                    Status = TransactionStatus.D,
                    TransactionDate = new System.DateTime(2019, 01, 06),
                    TransactionIdentificator = id2
                },
            };
            await DefaultRepository.Save(list);

            var result = await DefaultRepository.GetTransactions(new QueryRequest
            {
                StartDate = new System.DateTime(2019, 01, 04),
                EndDate = new System.DateTime(2019, 01, 07),
            });

            Assert.Equal(2, result.Count);

            using (var conn = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=TransactionFeed;Trusted_Connection=True;"))
            {
                conn.Open();
                var comm = conn.CreateCommand();
                comm.CommandText = $"delete [dbo].[transaction] where TransactionIdentificator in ('{id1}','{id2}')";
                comm.ExecuteNonQuery();
            }
        }
    }
}
