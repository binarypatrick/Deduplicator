using Cli.Deduplicator.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cli.Deduplicator.Deduplicators
{
    internal class FilenameDeduplicator : BaseDeduplicator, IDeduplicator
    {
        private static readonly Regex filenameRegex = new(@"(?'filename'.*?)(?'counter' ?\(\d+\))?(?'extension'\..+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public void Deduplicate(string path)
        {
            IEnumerable<FileInfo> files = Directory.GetFiles(path)
                .Select(x => new FileInfo(x));

            IEnumerable<IEnumerable<FileInfo>> fileGroups = files.GroupBy(x => GetMatchKey(x.Name))
                .Select(x => x.AsEnumerable());

            foreach (IEnumerable<FileInfo> fileGroup in fileGroups)
            {
                DeleteSmallestDuplicates(fileGroup);
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
    }
}
