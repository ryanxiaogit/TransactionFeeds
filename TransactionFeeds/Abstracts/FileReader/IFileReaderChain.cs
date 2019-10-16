using Abstracts.ModelBase;
using System.Collections.Generic;
using System.IO;

namespace Abstracts.FileReader
{
    public interface IFileReaderChain
    {
        string LastHandler { get; }
        List<TransactionModel> ReadFile(StreamReader stream);
    }
}
