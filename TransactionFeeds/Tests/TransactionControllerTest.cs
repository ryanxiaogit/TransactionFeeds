using Abstracts;
using Abstracts.FileReader;
using Abstracts.ModelBase;
using API.Controllers;
using API.Options;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class TransactionControllerTest
    {

        private TransactionController DefualtController
        {
            get
            {
                Mock<ITransactionAggregator> mockTransactionAggregator = new Mock<ITransactionAggregator>();
                Mock<ILogger<TransactionController>> mockLogger = new Mock<ILogger<TransactionController>>();
                Mock<IOptions<ServiceSetting>> mockOptionServiceSetting = new Mock<IOptions<ServiceSetting>>();
                Mock<IFileStaging> mockFileStaging = new Mock<IFileStaging>();
                var repository = RepositoryTest.DefaultRepository;

                var transControl = new TransactionController(
                       //mockTransactionAggregator.Object,
                       mockLogger.Object,
                       mockOptionServiceSetting.Object,
                       mockFileStaging.Object,
                       repository,
                       FileReaderChainTest.DefaultFileReaderChian as IFileReaderChain
                       );


                return transControl;
            }
        }

        //TODO: mock http request
        //[Fact]
        //public async Task UploadFileShouldbeOkWhenTransactionSaved()
        //{

            //DefualtController.HttpContext.Request.Body = 
            //    new StreamReader("./TestData/FileReader/Transactions_xml_20191013.xml").BaseStream;

            //var result = await DefualtController.UploadFeed();



            //var okResult = result as OkResult;
            //Assert.NotNull(okResult);
            //Assert.Equal(200, okResult.StatusCode);
        //}

        [Fact]
        public async Task SearchTransationsByCurrencyShouldBeOk()
        {
            string searchCurrency = "USD";
            var result = await DefualtController.SearchTransations(searchCurrency);
            var responseMessage = result as OkObjectResult;
            Assert.NotNull(responseMessage);
            var responseList = responseMessage.Value as List<QueryResponse>;

            Assert.Equal(3, responseList.Count);
            Assert.Equal("Inv00001", responseList[0].Id);
            Assert.Equal("200.00 USD", responseList[0].Payment);
            Assert.Equal("D", responseList[0].Status.ToString());
        }

        [Fact]
        public async Task SearchTransationsByTimeRangeShouldBeOk()
        {
            long startDatetime = 20191005101010;
            long endDatetime = 20191007101010;

            var result = await DefualtController.SearchTransations(startDatetime, endDatetime);

            var responseMessage = result as OkObjectResult;
            Assert.NotNull(responseMessage);
            var responseList = responseMessage.Value as List<QueryResponse>;

            Assert.Single(responseList);
            Assert.Equal("Inv00001", responseList[0].Id);
            Assert.Equal("200.00 USD", responseList[0].Payment);
            Assert.Equal("D", responseList[0].Status.ToString());
        }

        [Fact]
        public async Task SearchTransationsByStatusShouldBeOk()
        {
            TransactionStatus status = TransactionStatus.D;

            var result = await DefualtController.SearchTransations(status);

            var responseMessage = result as OkObjectResult;
            Assert.NotNull(responseMessage);
            var responseList = responseMessage.Value as List<QueryResponse>;

            Assert.Single(responseList);
            Assert.Equal("Inv00001", responseList[0].Id);
            Assert.Equal("200.00 USD", responseList[0].Payment);
            Assert.Equal("D", responseList[0].Status.ToString());
        }
    }
}
