using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cli.Deduplicator.Deduplicators
{
    internal class BaseDeduplicator
    {
        protected static void DeleteSmallestDuplicates(IEnumerable<FileInfo> files)
        {
            if (files.Count() <= 1)
            {
                return;
            }

            FileInfo[] orderedFiles = files
                .OrderByDescending(x => x.Length)
                .ToArray();

            for (int i = 1; i < orderedFiles.Length; i++)
            {
                orderedFiles[i].Delete();
            }
        }
    }
}
