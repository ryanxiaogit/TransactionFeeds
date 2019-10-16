using Abstracts;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace infranstructure
{
    public class FileStaging : IFileStaging
    {
        private readonly ILogger<FileStaging> _logger;

        public FileStaging(ILogger<FileStaging> logger)
        {
            _logger = logger;
        }

        public async Task<bool> StagingFile(string traceID, Stream fileStream, string targetFilePath)
        {
            var result = true;

            try
            {
                using (var targetStream = File.Create(targetFilePath))
                {
                    await fileStream.CopyToAsync(targetStream);
                    _logger.Log(LogLevel.Information, $"{traceID}: File Saved at {targetFilePath}");
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"{traceID}: Fail to save file at {targetFilePath}: {ex.Message}");
                result = false;
            }

            return result;
        }
    }
}
