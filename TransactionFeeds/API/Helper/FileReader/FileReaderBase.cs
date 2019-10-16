using Abstracts.ModelBase;
using Abstracts.ResponsibilityHandler;
using System.Collections.Generic;
using System.IO;

namespace API.Helper.FileReader
{
    public abstract class FileReaderBase : BaseHandler
    {
        public List<TransactionModel> ReadFile(StreamReader fileReader)
        {
            return ImplementHandle(fileReader) as List<TransactionModel>;
        }
    }
}
