using Microsoft.AspNetCore.Http;

namespace Shampan.Core.Interfaces.Services.AuditIssues;

public interface IFileService
{
    Task<List<string>> UploadFiles(IList<IFormFile>? files, string saveDirectory, string[] allowedExtensions = null,
        long? maxSizeInBytes = null);

	Task<List<string>> UploadFilesById(IList<IFormFile>? files, string saveDirectory, string[] allowedExtensions = null,
		long? maxSizeInBytes = null,string number=null);
}