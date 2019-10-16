using Abstracts.ModelBase;
using System.Collections.Generic;
using System.IO;

namespace API.Helper.FileReader
{
    class FileReaderChain
    {
        private readonly FileReaderBase rootReader = null;

        public FileReaderChain(params FileReaderBase[] readers)
        {
            if (readers.Length > 0)
            {
                rootReader = readers[0];
                if (readers.Length > 1)
                {
                    var currentReader = rootReader;
                    for (int i = 0; i < readers.Length; i++)
                    {
                        currentReader.SetNext(readers[i]);
                    }
                }
            }
        }

        public List<TransactionModel> ReadFile(StreamReader stream)
        {
            return rootReader.Handle(stream) as List<TransactionModel>;
        }
    }
}
