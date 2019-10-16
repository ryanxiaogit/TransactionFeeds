using Abstracts.ModelBase;
using System.Collections.Generic;
using System.IO;

namespace infranstructure.FileReader
{
    public class FileReaderChain
    {
        private readonly FileReaderBase rootReader = null;
        public string LastHandler { get; private set; }

        public FileReaderChain(params FileReaderBase[] readers)
        {
            if (readers.Length > 0)
            {
                rootReader = readers[0];
                rootReader.PropertyChanged += ReaderPropertyChange;
                if (readers.Length > 1)
                {
                    var currentReader = rootReader;
                    for (int i = 1; i < readers.Length; i++)
                    {
                        currentReader.SetNext(readers[i]);
                        readers[i].PropertyChanged += ReaderPropertyChange;
                    }
                }
            }
        }

        private void ReaderPropertyChange(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEndHere" && (sender as FileReaderBase).IsEndHere)
            {
                LastHandler = (sender as FileReaderBase).FileType;
            }
        }

        public List<TransactionModel> ReadFile(StreamReader stream)
        {
            LastHandler = string.Empty;
            return rootReader.Handle(stream) as List<TransactionModel>;
        }
    }
}
