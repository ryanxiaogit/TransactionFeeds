﻿using Abstracts.ModelBase;
using infranstructure.FileReader;
using infranstructure.FileReader.Dto;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Xunit;

namespace Tests
{
    public class FileReaderTest
    {
        static CsvReader defaultCsvReader = null;
        static XmlReader defaultXmlReader = null;

        public static CsvReader DefualtCsvReader
        {
            get
            {
                if (defaultCsvReader == null)
                {
                    Mock<ILogger<CsvReader>> mockLogger = new Mock<ILogger<CsvReader>>();
                    defaultCsvReader = new CsvReader(mockLogger.Object);
                }
                return defaultCsvReader;
            }
        }

        public static XmlReader DefualtXmlReader
        {
            get
            {
                if (defaultXmlReader == null)
                {
                    Mock<ILogger<XmlReader>> mockLogger = new Mock<ILogger<XmlReader>>();
                    defaultXmlReader = new XmlReader(mockLogger.Object);
                }
                return defaultXmlReader;
            }
        }

        //bool ValidateCsvFile(StreamReader file, out List<TransactionModel> transactions);
        //bool ValidateXmlFile(StreamReader file, out List<TransactionModel> transactions);
        [Fact]
        public void ReadCsvShouldbeOk()
        {
            var csvReader = new StreamReader("./TestData/FileReader/Transactions_csv_20191013.txt");
            var transactions = DefualtCsvReader.Handle(csvReader) as List<TransactionModel>;

            Assert.NotNull(transactions);
            Assert.Equal(2, transactions.Count);

            Assert.Equal(1000, transactions[0].Amount);
            Assert.Equal("USD", transactions[0].CurrencyCode);
            Assert.Equal("2/20/2019 12:33:16 PM", transactions[0].TransactionDate.ToString());
            Assert.Equal(TransactionStatus.A, transactions[0].Status);
        }

        [Fact]
        public void ReadXmlShouldbeOk()
        {
            var xmlReader = new StreamReader("./TestData/FileReader/Transactions_xml_20191013.xml");
            var transactions = DefualtXmlReader.Handle(xmlReader) as List<TransactionModel>;

            Assert.NotNull(transactions);
            Assert.Equal(2, transactions.Count);

            Assert.Equal(200.00m, transactions[0].Amount);
            Assert.Equal("USD", transactions[0].CurrencyCode);
            Assert.Equal("1/23/2019 1:45:10 PM", transactions[0].TransactionDate.ToString());
            Assert.Equal(TransactionStatus.D, transactions[0].Status);
        }

        [Fact]
        public void ReadSingleXml()
        {
            var xmlReader = new StreamReader("./TestData/FileReader/SingleTransactions_xml_20191013.xml");
            var xsSubmit = new XmlSerializer(typeof(XmlTransaction));
            var result = xsSubmit.Deserialize(xmlReader);

            var xmlReader2 = new StreamReader("./TestData/FileReader/Transactions_xml_20191013.xml");
            var xsSubmit2 = new XmlSerializer(typeof(XmlTransactions));
            var result2 = xsSubmit2.Deserialize(xmlReader2);

        }
    }
}
