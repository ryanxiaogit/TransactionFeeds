using System.IO;
using System.Threading.Tasks;

namespace Abstracts
{
    public interface IFileStaging
    {
        Task<bool> StagingFile(string traceID, Stream fileStream, string folderPatch);
    }
}
