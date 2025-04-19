using BudgetBuddy.Infrastructure.Services;

namespace BudgetBuddy.Infrastructure.Platforms.Windows;

internal class FileSystemManager : IFileSystemManager
{
    public async Task<byte[]?> Read(IFileSystemManager.SpecialFolder folder, string filePath,
        CancellationToken cancellationToken = default)
    {
        var basePath = GetSpecialFolderDirectory(folder);
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
        var basePath = GetSpecialFolderDirectory(folder);
        if (string.IsNullOrWhiteSpace(basePath))
            return false;

        filePath = Path.Combine(basePath, filePath);
        if (string.IsNullOrWhiteSpace(filePath))
            return false;

        await File.WriteAllBytesAsync(filePath, file, cancellationToken);
        return true;
    }

    private string? GetSpecialFolderDirectory(IFileSystemManager.SpecialFolder folder)
    {
        return folder switch
        {
            IFileSystemManager.SpecialFolder.Downloads =>
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),

            IFileSystemManager.SpecialFolder.Documents =>
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),

            IFileSystemManager.SpecialFolder.Pictures =>
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),

            IFileSystemManager.SpecialFolder.Videos =>
                Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),

            _ => null
        };
    }
}