using Abstracts.ModelBase;
using Abstracts.ResponsibilityHandler;
using System.Collections.Generic;
using System.IO;

namespace API.Helper.FileReader
{
    public abstract class FileReaderBase : BaseHandler
    {
        public string FileType { get; protected set; }
        public List<TransactionModel> ReadFile(StreamReader fileReader)
        {
            return Handle(fileReader) as List<TransactionModel>;
        }
    }
}
