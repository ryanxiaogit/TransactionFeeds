using Abstracts.FileReader;
using Abstracts.ModelBase;
using System.Collections.Generic;
using System.IO;

namespace infranstructure.FileReader
{
    public class FileReaderChain : IFileReaderChain
    {
        private readonly IFileReaderBase rootReader = null;
        private string _lastHandler;

        public string LastHandler => _lastHandler;
        public FileReaderChain(ICsvReader csvReader,
            IXmlReader xmlReader
            )
        {
            rootReader = csvReader;
            rootReader.SetNext(xmlReader);

            csvReader.PropertyChanged += ReaderPropertyChange;
            xmlReader.PropertyChanged += ReaderPropertyChange;
        }

        private void ReaderPropertyChange(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEndHere" && (sender as FileReaderBase).IsEndHere)
            {
                _lastHandler = (sender as FileReaderBase).FileType;
            }
        }

        public List<TransactionModel> ReadFile(StreamReader stream)
        {
            _lastHandler = string.Empty;
            return rootReader.Handle(stream) as List<TransactionModel>;
        }
    }
}
