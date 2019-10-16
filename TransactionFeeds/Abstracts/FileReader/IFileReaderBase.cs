using Abstracts.ModelBase;
using System.Collections.Generic;
using System.IO;

namespace Abstracts.FileReader
{
    public interface IFileReaderBase: IResponsibilityHandler
    {
        string FileType { get; }
        List<TransactionModel> ReadFile(StreamReader fileReader);
    }
}
