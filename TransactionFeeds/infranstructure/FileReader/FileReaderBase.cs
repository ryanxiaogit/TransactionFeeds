using Abstracts.ModelBase;
using Abstracts.ResponsibilityHandler;
using System.Collections.Generic;
using System.IO;

namespace infranstructure.FileReader
{
    public abstract class FileReaderBase : BaseHandler
    {
        public abstract string FileType { get; }
        public List<TransactionModel> ReadFile(StreamReader fileReader)
        {
            return Handle(fileReader) as List<TransactionModel>;
        }
    }
}
