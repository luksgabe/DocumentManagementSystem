namespace DocumentManagement.Core.Interfaces.Storage;

public interface IFileStorage
{
    Task<string> SaveAsync(Stream stream, string fileName, string contentType, CancellationToken ct = default);
    Task<Stream> OpenReadAsync(string storageUri, CancellationToken ct = default);
    Task RemoveAsync(string storageUri, CancellationToken ct = default);
}
