using Abstracts.ModelBase;
using API.Helper.FileReader.Dto;
using API.Helper.FileReader.MapProfile;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace API.Helper.FileReader
{
    public class XmlReader : FileReaderBase
    {
        private readonly ILogger<XmlReader> _logger;
        private readonly IMapper _mapper;
        public XmlReader(ILogger<XmlReader> logger)
        {
            _logger = logger;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapXmlTransactionToTransactionModel>();
            }).CreateMapper();
        }

        protected override object ActualHandle(object request)
        {
            List<TransactionModel> finalTrans = null;
            XmlTransactions transactions = null;
            var fileReader = request as StreamReader;
            try
            {
                var xsSubmit = new XmlSerializer(typeof(XmlTransactions));
                transactions = xsSubmit.Deserialize(fileReader) as XmlTransactions;
                if (transactions != null && transactions.Trnasactions.Count > 0)
                {
                    finalTrans = _mapper.Map<List<TransactionModel>>(transactions.Trnasactions);
                }
                if (finalTrans != null && finalTrans.Count > 0) FileType = "XML";
            }
            catch (XmlException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
            }

            return finalTrans;
        }
    }
}
