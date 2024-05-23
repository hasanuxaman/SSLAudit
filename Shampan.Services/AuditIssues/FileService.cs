using Microsoft.AspNetCore.Http;
using Shampan.Core.Interfaces.Services.AuditIssues;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;


using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;



namespace Shampan.Services.AuditIssues;

public class FileService : IFileService
{
    public async Task<List<string>> UploadFiles(IList<IFormFile>? files, string saveDirectory, string[] allowedExtensions = null, long? maxSizeInBytes = null)
    {
        var savedPaths = new List<string>();

        if (files is null)
            return savedPaths;

        foreach (var file in files)
        {
			//if (file != null)
			//{
			//	// Validate file extension
			//	var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
			//	if (allowedExtensions != null && (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension)))
			//	{
			//		throw new Exception($"Invalid file extension. Allowed extensions are: {string.Join(", ", allowedExtensions)}");
			//	}

			//	// Validate file size
			//	if (maxSizeInBytes.HasValue && file.Length > maxSizeInBytes.Value)
			//	{
			//		throw new Exception($"File size exceeds limit of {maxSizeInBytes.Value / 1024 / 1024}MB");
			//	}

			//	// Set your own custom filename
			//	var name = "78";
			//	var customFileName = name;

			//	var fileName = Path.GetFileNameWithoutExtension(file.FileName);

			//	// Append the custom filename and extension
			//	var newFileName = $"{customFileName}{extension}";

			//	var filePath = Path.Combine(Directory.GetCurrentDirectory(), saveDirectory, newFileName);
			//	savedPaths.Add(filePath); // Add saved path to list

			//	await using var fileStream = new FileStream(filePath, FileMode.Create);
			//	await file.CopyToAsync(fileStream);
			//}



			//main code of images



			if (file != null)
			{

				var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
				if (allowedExtensions != null && (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension)))
				{
					throw new Exception($"Invalid file extension. Allowed extensions are: {string.Join(", ", allowedExtensions)}");
				}


				if (maxSizeInBytes.HasValue && file.Length > maxSizeInBytes.Value)
				{
					throw new Exception($"File size exceeds limit of {maxSizeInBytes.Value / 1024 / 1024}MB");
				}


				string guid = Guid.NewGuid().ToString();

				var fileName = Path.GetFileNameWithoutExtension(file.FileName);


				var newFileName = $"{fileName}_shp_{guid}{extension}";

				var filePath = Path.Combine(Directory.GetCurrentDirectory(), saveDirectory, newFileName);
				savedPaths.Add(filePath);

				await using var fileStream = new FileStream(filePath, FileMode.Create);
				await file.CopyToAsync(fileStream);
			}
		}

		return savedPaths;
    }

	public async Task<List<string>> UploadFilesById(IList<IFormFile>? files, string saveDirectory, string[] allowedExtensions = null, long? maxSizeInBytes = null, string number = null)
	{
		var savedPaths = new List<string>();

		if (files is null)
			return savedPaths;

		foreach (var file in files)
		{


			if (file != null)
			{

				var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
				if (allowedExtensions != null && (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension)))
				{
					throw new Exception($"Invalid file extension. Allowed extensions are: {string.Join(", ", allowedExtensions)}");
				}


				if (maxSizeInBytes.HasValue && file.Length > maxSizeInBytes.Value)
				{
					throw new Exception($"File size exceeds limit of {maxSizeInBytes.Value / 1024 / 1024}MB");
				}


			
				var customFileName = number;

				var fileName = Path.GetFileNameWithoutExtension(file.FileName);


				var newFileName = $"{customFileName}{extension}";

				var filePath = Path.Combine(Directory.GetCurrentDirectory(), saveDirectory, newFileName);

				using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(file.OpenReadStream()))
				{
					image.Mutate(x => x.Resize(new ResizeOptions
					{
						Size = new SixLabors.ImageSharp.Size(92, 92),
						Mode = ResizeMode.BoxPad
					}));

					image.Save(filePath);


					savedPaths.Add(filePath);

					//await using var fileStream = new FileStream(filePath, FileMode.Create);
					//await file.CopyToAsync(fileStream);
				}



				//savedPaths.Add(filePath);

				//await using var fileStream = new FileStream(filePath, FileMode.Create);
				//await file.CopyToAsync(fileStream);
			}




		}

		return savedPaths;
	}

	


}
