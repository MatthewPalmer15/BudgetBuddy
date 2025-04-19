using BudgetBuddy.Infrastructure.Services;

namespace BudgetBuddy.Infrastructure.Platforms.iOS;

internal class FileSystemManager : IFileSystemManager
{
    private string? GetSpecialFolderDirectory()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    public async Task<byte[]?> Read(IFileSystemManager.SpecialFolder folder, string filePath,
        CancellationToken cancellationToken = default)
    {
        var basePath = GetSpecialFolderDirectory();
        if (string.IsNullOrWhiteSpace(basePath))
            return null;

        filePath = Path.Combine(basePath, filePath);
        if (string.IsNullOrWhiteSpace(filePath))
            return null;

        return File.Exists(filePath) ? await File.ReadAllBytesAsync(filePath, cancellationToken) : null;
    }

    public async Task<bool> Write(IFileSystemManager.SpecialFolder folder, string filePath, byte[] file,
        CancellationToken cancellationToken = default)
    {
        var basePath = GetSpecialFolderDirectory();
        if (string.IsNullOrWhiteSpace(basePath))
            return false;

        filePath = Path.Combine(basePath, filePath);
        if (string.IsNullOrWhiteSpace(filePath))
            return false;

        await File.WriteAllBytesAsync(filePath, file, cancellationToken);
        return true;
    }
}