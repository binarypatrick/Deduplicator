using BinaryPatrick.Deduplicator.Interfaces;
using System.Security.Cryptography;

namespace BinaryPatrick.Deduplicator.Deduplicators;
internal class HashDeduplicator : BaseDeduplicator, IDeduplicator
{
    public string Name { get; } = "hash";
    public string Description { get; } = @"Matches duplicates by file MD5 hash";
    public int Rank { get; } = 0;

    public void Deduplicate(IAppOptions options)
    {
        IEnumerable<IEnumerable<FileInfo>> fileGroups = Directory.GetFiles(options.Path)
            .Select(x => new FileInfo(x))
            .GroupBy(GetFileHash)
            .ToList();

        foreach (IEnumerable<FileInfo> fileGroup in fileGroups)
        {
            DeleteSmallestDuplicates(fileGroup, options.IsVerbose, options.IsDryRun);
        }
    }

    private string GetFileHash(FileInfo file)
    {
        byte[] fileBytes = File.ReadAllBytes(file.FullName);
        byte[] hashBytes = MD5.HashData(fileBytes);

        return hashBytes.ToHexString();
    }
}
