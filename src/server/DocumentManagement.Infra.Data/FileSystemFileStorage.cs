using DocumentManagement.Core.Interfaces.Storage;
using Microsoft.AspNetCore.Hosting;

namespace DocumentManagement.Infra.Data;

public class FileSystemFileStorage : IFileStorage
{
    private readonly string _rootPath;

    public FileSystemFileStorage(IWebHostEnvironment env)
    {
        _rootPath = Path.Combine(env.ContentRootPath, "Uploads");
        Directory.CreateDirectory(_rootPath);
    }

    public async Task<string> SaveAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default)
    {
        var safeFileName = $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";
        var fullPath = Path.Combine(_rootPath, safeFileName);

        await using var fileStream = File.Create(fullPath);
        await stream.CopyToAsync(fileStream, ct);

        return safeFileName;
    }

    public Task<Stream> OpenReadAsync(string storageUri, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_rootPath, storageUri);
        Stream stream = File.OpenRead(fullPath);
        return Task.FromResult(stream);
    }

    public Task RemoveAsync(string storageUri, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_rootPath, storageUri);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        return Task.CompletedTask;
    }
}
