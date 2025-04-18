using BudgetBuddy.Infrastructure.Services;

namespace BudgetBuddy.Infrastructure.Platforms.MacCatalyst;

internal class FileSystemManager : IFileSystemManager
{
    private string? GetSpecialFolderDirectory(IFileSystemManager.SpecialFolder folder)
    {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        return folder switch
        {
            IFileSystemManager.SpecialFolder.Documents =>
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),

            IFileSystemManager.SpecialFolder.Downloads =>
                Path.Combine(userProfile, "Downloads"),

            IFileSystemManager.SpecialFolder.Pictures =>
                Path.Combine(userProfile, "Pictures"),

            IFileSystemManager.SpecialFolder.Videos =>
                Path.Combine(userProfile, "Movies"),

            _ => null
        };
    }

    public async Task<byte[]?> Read(IFileSystemManager.SpecialFolder folder, string filePath,
        CancellationToken cancellationToken = default)
    {
        var basePath = GetSpecialFolderDirectory(folder);
        if (string.IsNullOrWhiteSpace(basePath))
            return null;

        var fullPath = Path.Combine(basePath, filePath);
        if (!File.Exists(fullPath))
            return null;

        return await File.ReadAllBytesAsync(fullPath, cancellationToken);
    }

    public async Task<bool> Write(IFileSystemManager.SpecialFolder folder, string filePath, byte[] file,
        CancellationToken cancellationToken = default)
    {
        var basePath = GetSpecialFolderDirectory(folder);
        if (string.IsNullOrWhiteSpace(basePath))
            return false;

        var fullPath = Path.Combine(basePath, filePath);

        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await File.WriteAllBytesAsync(fullPath, file, cancellationToken);
        return true;
    }
}