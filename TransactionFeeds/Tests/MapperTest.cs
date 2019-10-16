using Abstracts.ModelBase;
using AutoMapper;
using infranstructure.FileReader.Dto;
using infranstructure.FileReader.MapProfile;
using System;
using Xunit;

namespace Tests
{
    public class MapperTest
    {
        [Fact]
        public void MapXmlTransactionToTransactionModel()
        {
            var mapper = new MapperConfiguration(cfg =>
             {
                 cfg.AddProfile<MapXmlTransactionToTransactionModel>();
             }).CreateMapper();

            var xmlTransaction = new XmlTransaction
            {
                TransactionID = "TestID",
                Status = "Reject",
                TransactionDate = "2019-01-24T16:09:15",
                Details = new XmlTransaction.PaymentDetails
                {
                    Amount = "10000.00",
                    CurrencyCode = "EUR"
                }
            };

            var targetTransaction = mapper.Map<TransactionModel>(xmlTransaction);

            Assert.NotNull(targetTransaction);
            Assert.Equal(xmlTransaction.TransactionID, targetTransaction.TransactionIdentificator);
            Assert.Equal(xmlTransaction.Details.CurrencyCode, targetTransaction.CurrencyCode);
            Assert.Equal(TransactionStatus.R, targetTransaction.Status);
            Assert.Equal(new DateTime(2019, 01, 24, 16, 09, 15).ToString(), targetTransaction.TransactionDate.ToString());
        }
    }
}
