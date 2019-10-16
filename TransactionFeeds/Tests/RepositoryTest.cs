using Abstracts;
using Abstracts.ModelBase;
using infranstructure.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
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
    }
}
