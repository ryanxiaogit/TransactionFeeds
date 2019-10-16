using API.Helper.FileReader.Dto;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace API.Helper.FileReader
{
    public class XmlReader : FileReaderBase
    {
        readonly ILogger<XmlReader> _logger;
        public XmlReader(ILogger<XmlReader> logger)
        {
            _logger = logger;
        }

        protected override object ImplementHandle(object request)
        {
            XmlTransactions transactions = null;
            var fileReader = request as StreamReader;
            try
            {
                var xsSubmit = new XmlSerializer(typeof(XmlTransactions));
                transactions = xsSubmit.Deserialize(fileReader) as XmlTransactions;
            }
            catch (XmlException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
            }

            return transactions;
        }
    }
}
