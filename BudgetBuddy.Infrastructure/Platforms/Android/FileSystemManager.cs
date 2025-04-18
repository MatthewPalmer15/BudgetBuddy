using BlazorHybrid.Infrastructure.Services;
using Environment = Android.OS.Environment;

namespace BlazorHybrid.Infrastructure.Platforms.Android;

internal class FileSystemManager : IFileSystemManager
{
    public async Task<byte[]?> Read(IFileSystemManager.SpecialFolder folder, string filePath,
        CancellationToken cancellationToken = default)
    {
        var hasPermissions = await RequestPermission(cancellationToken);
        if (!hasPermissions) return null;

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
        var hasPermissions = await RequestPermission(cancellationToken);
        if (!hasPermissions) return false;

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
        var directoryPath = folder switch
        {
            IFileSystemManager.SpecialFolder.Downloads => Environment.DirectoryDownloads,
            IFileSystemManager.SpecialFolder.Documents => Environment.DirectoryDocuments,
            IFileSystemManager.SpecialFolder.Pictures => Environment.DirectoryPictures,
            IFileSystemManager.SpecialFolder.Videos => Environment.DirectoryMovies,
            _ => null
        };

        if (string.IsNullOrWhiteSpace(directoryPath))
            return null;

        return Environment.GetExternalStoragePublicDirectory(directoryPath)?.AbsolutePath;
    }

    private async Task<bool> RequestPermission(CancellationToken cancellationToken = default)
    {
        var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
        switch (status)
        {
            case PermissionStatus.Granted:
                return true;

            case PermissionStatus.Denied:
                return false;

            default:
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
                switch (status)
                {
                    case PermissionStatus.Granted:
                        return true;

                    case PermissionStatus.Denied:
                        return false;
                }

                break;
        }

        return false;
    }
}