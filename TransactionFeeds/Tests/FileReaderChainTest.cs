using Abstracts.ModelBase;
using infranstructure.FileReader;
using System.IO;
using Xunit;

namespace Tests
{
    public class FileReaderChainTest
    {
        static FileReaderChain defaultChain = null;

        public static FileReaderChain DefaultFileReaderChian
        {
            get
            {
                if (defaultChain == null)
                {
                    defaultChain = new FileReaderChain
                                        (
                                            FileReaderTest.DefualtCsvReader,
                                            FileReaderTest.DefualtXmlReader
                                        );
                }
                return defaultChain;
            }
        }

        [Fact]
        public void FileReaderChainShouldWork()
        {
            var csvStream = new StreamReader("./TestData/FileReader/Transactions_csv_20191013.txt");

            var result = DefaultFileReaderChian.ReadFile(csvStream);

            Assert.Equal("CSV", defaultChain.LastHandler);
            Assert.Equal(2, result.Count);
            Assert.Equal("Invoice0000002", result[1].TransactionIdentificator);
            Assert.Equal(300.00m, result[1].Amount);
            Assert.Equal("USD", result[1].CurrencyCode);
            Assert.Equal("2/21/2019 2:04:59 AM", result[1].TransactionDate.ToString());
            Assert.Equal(TransactionStatus.R, result[1].Status);

            var xmlStream = new StreamReader("./TestData/FileReader/Transactions_xml_20191013.xml");

            result = DefaultFileReaderChian.ReadFile(xmlStream);
            Assert.Equal("XML", defaultChain.LastHandler);
            Assert.Equal(2, result.Count);
            //Invoice0000002","300.00","USD","21/02/2019 02:04:59", "Failed
            Assert.Equal("Inv00002", result[1].TransactionIdentificator);
            Assert.Equal(10000.00m, result[1].Amount);
            Assert.Equal("EUR", result[1].CurrencyCode);
            Assert.Equal("1/24/2019 4:09:15 PM", result[1].TransactionDate.ToString());
            Assert.Equal(TransactionStatus.R, result[1].Status);
        }
    }
}
