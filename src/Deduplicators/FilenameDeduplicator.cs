using BinaryPatrick.Deduplicator.Interfaces;
using System.Text.RegularExpressions;

namespace BinaryPatrick.Deduplicator.Deduplicators;

internal partial class FilenameDeduplicator : BaseDeduplicator, IDeduplicator
{
    public string Name { get; } = "filename";
    public string Description { get; } = @"Matches duplicates by case insensitive filename";
    public int Rank { get; } = 0;

    private static readonly Regex filenameRegex = FilenameRegex();

    public void Deduplicate(IAppOptions options)
    {
        IEnumerable<IEnumerable<FileInfo>> fileGroups = Directory.GetFiles(options.Path)
            .Select(x => new FileInfo(x))
            .GroupBy(x => GetMatchKey(x.Name));

        foreach (IEnumerable<FileInfo> fileGroup in fileGroups)
        {
            DeleteSmallestDuplicates(fileGroup, options.IsVerbose, options.IsDryRun);
        }
    }

    private static string GetMatchKey(string filename)
    {
        Match match = filenameRegex.Match(filename);
        if (!match.Success)
        {
            return filename;
        }

        return match.Groups[1].Value;
    }

    [GeneratedRegex(@"(?'filename'.*?)(?'counter' ?\(\d+\))?(?'extension'\..+)", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex FilenameRegex();
}
