using Microsoft.AspNetCore.Hosting;
using System.Net;

namespace Parking.Management.Service.Core.Common;

public interface IFileService
{
    #region --- Common ---
    Task WriteFile(byte[] content, string directory, string fileName);

    Task WriteFile(Stream binaryStream, string directory, string fileName);

    Task WriteFileFromUrl(string url, string directory, string fileName);

    Task DeleteFile(string directory, string fileName);

    byte[]? ReadFileAsByte(string directory, string fileName);

    Stream? ReadFileAsStream(string directory, string fileName);

    string GetPathOfFile(string directory, string fileName);

    string GeneratePathForUrl(string directory, string fileName);
    #endregion
    
    #region --- Recognition ---
    Task WriteImageRecognition(byte[] content, string fileName);

    Task WriteImageRecognition(Stream binaryStream, string fileName);

    Task WriteImageRecognitionFromUrl(string url, string fileName);

    Task DeleteImageRecognition(string fileName);

    byte[]? ReadImageRecognitionAsByte(string fileName);

    Stream? ReadImageRecognitionAsStream(string fileName);

    string GetPathOfImageRecognition(string fileName);

    string GeneratePathImageRecognitionForUrl(string fileName);
    #endregion
}

public class FileService: IFileService
{
    private readonly string _fileFolder;
    private readonly string _imageRecognitionFolder;
    
    private const string FILE_FOLDER_NAME = "files";
    private const string IMAGE_FOLDER_NAME = "images";
    private const string IMAGE_RECOGNITION_FOLDER_NAME = "recognitions";
    

    public FileService(IHostingEnvironment environment)
    {
        _fileFolder = Path.Combine(environment.WebRootPath, FILE_FOLDER_NAME);
        _imageRecognitionFolder = Path.Combine(environment.WebRootPath, IMAGE_FOLDER_NAME, IMAGE_RECOGNITION_FOLDER_NAME);
        
        if (!Directory.Exists(_fileFolder))
            Directory.CreateDirectory(_fileFolder);
        
        if (!Directory.Exists(_imageRecognitionFolder))
            Directory.CreateDirectory(_imageRecognitionFolder);
    }
    
        #region --- Common ---
    public async Task WriteFile(byte[] content, string directory, string fileName)
    {
        var filePath = Path.Combine(string.IsNullOrEmpty(directory) ? _fileFolder : directory, fileName);
        await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await fileStream.WriteAsync(content, 0, content.Length);
    }

    public async Task WriteFile(Stream binaryStream, string directory, string fileName)
    {
        var filePath = Path.Combine(string.IsNullOrEmpty(directory) ? _fileFolder : directory, fileName);
        await using var handler = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await binaryStream.CopyToAsync(handler);
    }

    public async Task WriteFileFromUrl(string url, string directory, string fileName)
    {
        using WebClient client = new();
        await using Stream stream = client.OpenRead(url);
        
        await WriteFile(stream, string.IsNullOrEmpty(directory) ? _fileFolder : directory, fileName);
    }

    public async Task DeleteFile(string directory, string fileName)
    {
        var filePath = Path.Combine(string.IsNullOrEmpty(directory) ? _fileFolder : directory, fileName);
        if (File.Exists(filePath))
            await Task.Run(() => File.Delete(filePath));
    }

    public byte[]? ReadFileAsByte(string directory, string fileName)
    {
        var filePath = Path.Combine(string.IsNullOrEmpty(directory) ? _fileFolder : directory, fileName);
        if (!File.Exists(filePath))
            return null;

        using (var streamReader = new StreamReader(filePath))
        {
            using (var memoryStream = new MemoryStream())
            {
                streamReader.BaseStream.CopyTo(memoryStream);
                var fileBytes = memoryStream.ToArray();

                return fileBytes;
            }
        }
    }

    public Stream? ReadFileAsStream(string directory, string fileName)
    {
        var filePath = Path.Combine(string.IsNullOrEmpty(directory) ? _fileFolder : directory, fileName);
        if (!File.Exists(filePath))
            return null;

        var streamReader = new StreamReader(filePath);
        var memoryStream = new MemoryStream();
        streamReader.BaseStream.CopyTo(memoryStream);

        return memoryStream;
    }

    public string GetPathOfFile(string directory, string fileName)
        => Path.Combine(string.IsNullOrEmpty(directory) ? _fileFolder : directory, fileName);
    
    public string GeneratePathForUrl(string directory, string fileName)
        => $"{FILE_FOLDER_NAME}/{fileName}";
    #endregion
    
        
    #region --- Recognition ---
    public async Task WriteImageRecognition(byte[] content, string fileName)
        => await WriteFile(content, _imageRecognitionFolder, fileName);

    public async Task WriteImageRecognition(Stream binaryStream, string fileName)
        => await WriteFile(binaryStream, _imageRecognitionFolder, fileName);

    public async Task WriteImageRecognitionFromUrl(string url, string fileName)
        => await WriteFileFromUrl(url, _imageRecognitionFolder, fileName);

    public async Task DeleteImageRecognition(string fileName)
        => await DeleteFile(_imageRecognitionFolder, fileName);

    public byte[]? ReadImageRecognitionAsByte(string fileName)
        => ReadFileAsByte(_imageRecognitionFolder, fileName);

    public Stream? ReadImageRecognitionAsStream(string fileName)
        => ReadFileAsStream(_imageRecognitionFolder, fileName);

    public string GetPathOfImageRecognition(string fileName)
        => GetPathOfFile(_imageRecognitionFolder, fileName);

    public string GeneratePathImageRecognitionForUrl(string fileName)
        => $"{IMAGE_FOLDER_NAME}/{IMAGE_RECOGNITION_FOLDER_NAME}/{fileName}";
    #endregion
}