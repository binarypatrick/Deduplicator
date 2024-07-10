namespace BinaryPatrick.Deduplicator.Deduplicators;

internal class BaseDeduplicator
{
    protected static void DeleteSmallestDuplicates(IEnumerable<FileInfo> files, bool isVerbose, bool isDryRun)
    {
        if (files.Count() <= 1)
        {
            return;
        }

        IEnumerable<FileInfo> orderedFiles = files
            .OrderByDescending(x => x.Length)
            .Skip(1);

        foreach (FileInfo file in orderedFiles)
        {
            if (isDryRun)
            {
                Console.WriteLine($"  {file.FullName}");
                continue;
            }

            file.Delete();
            if (isVerbose)
            {
                Console.WriteLine($"{file.FullName} deleted");
            }
        }
    }
}
