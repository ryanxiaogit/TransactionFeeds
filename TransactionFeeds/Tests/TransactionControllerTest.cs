using Abstracts;
using Abstracts.ModelBase;
using API.Controllers;
using API.Options;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                Mock<IMapper> mockMaaper = new Mock<IMapper>();
                Mock<IOptions<ServiceSetting>> mockOptionServiceSetting = new Mock<IOptions<ServiceSetting>>();
                Mock<IFileStaging> mockFileStaging = new Mock<IFileStaging>();
                //Mock<IFileReader> mockFileTypeValidation = new Mock<IFileReader>();


                var transControl = new TransactionController(
                       mockTransactionAggregator.Object,
                       mockLogger.Object,
                       mockMaaper.Object,
                       mockOptionServiceSetting.Object,
                       mockFileStaging.Object
                       //mockFileTypeValidation.Object
                       );


                return transControl;
            }
        }

        [Fact]
        public async Task UploadFileShouldbeOkWhenTransactionSaved()
        {
            var mockTransactionAggregator = new Mock<ITransactionAggregator>();

            mockTransactionAggregator.Setup(f =>
            f.SaveTransactions(
                It.IsNotNull<IList<TransactionModel>>()))
                .Returns(Task.FromResult(true));
            var transControl = DefualtController;

            transControl.LoadDependency(mockTransactionAggregator.Object);

            var result = await transControl.UploadFeed();

            var okResult = result as OkResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task SearchTransationsByCurrencyShouldBeOk()
        {
            string searchCurrency = "USD";
            IList<TransactionModel> returnTransactions = new List<TransactionModel>
            {
                new TransactionModel
                {
                    TransactionIdentificator = "Inv00001",
                    Amount = 200,
                    CurrencyCode = "USD",
                    Status = TransactionStatus.D,
                    TransactionDate = DateTime.ParseExact("20190101","yyyyMMdd", CultureInfo.InstalledUICulture)
                },
                new TransactionModel
                {
                    TransactionIdentificator = "Inv00002",
                    Amount = 10000,
                    CurrencyCode = "USD",
                    Status = TransactionStatus.R,
                    TransactionDate = DateTime.ParseExact("20190101","yyyyMMdd", CultureInfo.InstalledUICulture)
                },
                new TransactionModel
                {
                    TransactionIdentificator = "Inv00002",
                    Amount = 1000,
                    CurrencyCode = "USD",
                    Status = TransactionStatus.A,
                    TransactionDate = DateTime.ParseExact("20190101","yyyyMMdd", CultureInfo.InstalledUICulture)
                }
            };

            var mockTransactionAggregator = new Mock<ITransactionAggregator>();
            mockTransactionAggregator.Setup(f =>
            f.SearchTransaction(
                It.IsNotNull<TransactionModel>()))
                .Returns(Task.FromResult(returnTransactions));
            var transControl = DefualtController;



            transControl.LoadDependency(mockTransactionAggregator.Object);
            var result = await transControl.SearchTransations(searchCurrency);
            var responseMessage = result as OkObjectResult;
            Assert.NotNull(responseMessage);
            var responseList = responseMessage.Value as List<QueryResponse>;

            Assert.Equal(3, responseList.Count);
            Assert.Equal("Inv00001", responseList[0].Id);
            Assert.Equal("200.00 USD", responseList[0].Payment);
            Assert.Equal("D", responseList[0].Status);
        }

        [Fact]
        public async Task SearchTransationsByTimeRangeShouldBeOk()
        {
            int startDatetime = 20190301;
            int endDatetime = 20190302;
            IList<TransactionModel> returnTransactions = new List<TransactionModel>
            {
                new TransactionModel
                {
                    TransactionIdentificator = "Inv00001",
                    Amount = 200,
                    CurrencyCode = "USD",
                    Status = TransactionStatus.D,
                    TransactionDate = DateTime.ParseExact("20190301","yyyyMMdd", CultureInfo.InstalledUICulture)
                },
                new TransactionModel
                {
                    TransactionIdentificator = "Inv00002",
                    Amount = 10000,
                    CurrencyCode = "USD",
                    Status = TransactionStatus.R,
                    TransactionDate = DateTime.ParseExact("20190301","yyyyMMdd", CultureInfo.InstalledUICulture)
                }
            };

            var mockTransactionAggregator = new Mock<ITransactionAggregator>();
            mockTransactionAggregator.Setup(f =>
            f.SearchTransaction(
                It.IsNotNull<TransactionModel>()))
                .Returns(Task.FromResult(returnTransactions));
            var transControl = DefualtController;



            transControl.LoadDependency(mockTransactionAggregator.Object);
            var result = await transControl.SearchTransations(startDatetime, endDatetime);

            var responseMessage = result as OkObjectResult;
            Assert.NotNull(responseMessage);
            var responseList = responseMessage.Value as List<QueryResponse>;

            Assert.Equal(2, responseList.Count);
            Assert.Equal("Inv00001", responseList[0].Id);
            Assert.Equal("200.00 USD", responseList[0].Payment);
            Assert.Equal("D", responseList[0].Status);
        }

        [Fact]
        public async Task SearchTransationsByStatusShouldBeOk()
        {
            TransactionStatus status = TransactionStatus.D;
            IList<TransactionModel> returnTransactions = new List<TransactionModel>
            {
                new TransactionModel
                {
                    TransactionIdentificator = "Inv00001",
                    Amount = 200,
                    CurrencyCode = "USD",
                    Status = TransactionStatus.D,
                    TransactionDate = DateTime.ParseExact("20190301","yyyyMMdd", CultureInfo.InstalledUICulture)
                }
            };

            var mockTransactionAggregator = new Mock<ITransactionAggregator>();
            mockTransactionAggregator.Setup(f =>
            f.SearchTransaction(
                It.IsNotNull<TransactionModel>()))
                .Returns(Task.FromResult(returnTransactions));
            var transControl = DefualtController;



            transControl.LoadDependency(mockTransactionAggregator.Object);
            var result = await transControl.SearchTransations(status);

            var responseMessage = result as OkObjectResult;
            Assert.NotNull(responseMessage);
            var responseList = responseMessage.Value as List<QueryResponse>;

            Assert.Single(responseList);
            Assert.Equal("Inv00001", responseList[0].Id);
            Assert.Equal("200.00 USD", responseList[0].Payment);
            Assert.Equal("D", responseList[0].Status);
        }
    }
}
