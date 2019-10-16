using Abstracts.FileReader;
using Abstracts.ModelBase;
using AutoMapper;
using infranstructure.FileReader.Dto;
using infranstructure.FileReader.MapProfile;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace infranstructure.FileReader
{
    public class XmlReader : FileReaderBase, IXmlReader
    {
        private readonly ILogger<XmlReader> _logger;
        private readonly IMapper _mapper;

        public override string FileType { get => "XML"; }

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
            fileReader.BaseStream.Position = 0;
            fileReader.DiscardBufferedData();

            try
            {
                var xsSubmit = new XmlSerializer(typeof(XmlTransactions));
                transactions = xsSubmit.Deserialize(fileReader) as XmlTransactions;
                if (transactions != null && transactions.Trnasactions.Count > 0)
                {
                    finalTrans = _mapper.Map<List<TransactionModel>>(transactions.Trnasactions);
                }
            }

            catch (XmlException ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
            }

            return finalTrans;
        }
    }
}
