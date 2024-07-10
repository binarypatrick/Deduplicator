using BinaryPatrick.Deduplicator.Helpers;
using BinaryPatrick.Deduplicator.Interfaces;
using BinaryPatrick.Deduplicator.Models;

namespace BinaryPatrick.Deduplicator;

internal class Program
{
    private static void Main(string[] args)
    {
        IAppOptions? options = AppOptions.ParseArguments(args);
        if (options is null)
        {
            return;
        }

        if (options.IsDryRun)
        {
            ConsoleHelper.WriteInformation("Dry Run - the following files listed would be deleted:");
        }

        foreach (IDeduplicator deduplicator in options.Deduplicators)
        {
            deduplicator.Deduplicate(options);
        }
    }
}
