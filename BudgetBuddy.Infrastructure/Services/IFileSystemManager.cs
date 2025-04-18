namespace BlazorHybrid.Infrastructure.Services;

public interface IFileSystemManager
{
    public enum SpecialFolder
    {
        Documents,
        Downloads,
        Pictures,
        Videos
    }

    Task<byte[]?> Read(SpecialFolder folder, string filePath, CancellationToken cancellationToken = default);
    Task<bool> Write(SpecialFolder folder, string filePath, byte[] file, CancellationToken cancellationToken = default);
}