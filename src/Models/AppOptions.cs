using BinaryPatrick.Deduplicator.Helpers;
using BinaryPatrick.Deduplicator.Interfaces;
using System.Reflection;
using System.Text;

namespace BinaryPatrick.Deduplicator.Models;
internal class AppOptions : IAppOptions
{
    private const string HELP_FLAG = "--help";
    private const string VERBOSE_FLAG = "--verbose";
    private const string DRY_RUN_FLAG = "--dry-run";
    private const string VERSION_FLAG = "--version";

    public string Path { get; set; }
    public IEnumerable<IDeduplicator> Deduplicators { get; set; }
    public bool IsVerbose { get; set; } = false;
    public bool IsDryRun { get; set; } = false;

    protected AppOptions(string[] args)
    {
        Path = args[0];
        IsVerbose = args.Contains(VERBOSE_FLAG, StringComparer.OrdinalIgnoreCase);
        IsDryRun = args.Contains(DRY_RUN_FLAG, StringComparer.OrdinalIgnoreCase);
        Deduplicators = GetDeduplicators(args);
    }

    public static AppOptions? ParseArguments(string[] args)
    {
        if (args is null || args.Length < 1)
        {
            ConsoleHelper.WriteError("No path supplied");
            ConsoleHelper.WriteInformation(GetHelpText());
            return null;
        }

        if (args.Contains(HELP_FLAG, StringComparer.OrdinalIgnoreCase))
        {
            ConsoleHelper.WriteInformation(GetHelpText());
            return null;
        }

        if (args.Contains(VERSION_FLAG, StringComparer.OrdinalIgnoreCase))
        {
            Version? version = Assembly.GetEntryAssembly()?.GetName().Version;
            string versionStr = $"{version?.Major}.{version?.Minor}.{version?.Build}";
            ConsoleHelper.WriteInformation(versionStr);
            return null;
        }

        AppOptions options = new AppOptions(args);
        if (!Directory.Exists(options.Path))
        {
            ConsoleHelper.WriteError($"{options.Path} is not a valid directory.");
            ConsoleHelper.WriteInformation(GetHelpText());
            return null;
        }

        return options;
    }

    public static string GetHelpText()
    {
        StringBuilder helpText = new StringBuilder();
        _ = helpText.AppendLine($"Deduplicator by BinaryPatrick. Copyright 2023-{DateTime.Now.Year}.");
        _ = helpText.AppendLine($"Usage: dedup [PATH] <OPTIONS>");
        _ = helpText.AppendLine();
        _ = helpText.AppendLine($"Required:");
        _ = helpText.AppendLine($"    Path: Directory path to files.");
        _ = helpText.AppendLine();
        _ = helpText.AppendLine($"Options:");
        IEnumerable<(string, string)> options = IDeduplicator.Implementations.Select(x => (name: GetFlagName(x.Name), description: $"{x.Description} only."))
            .Append((DRY_RUN_FLAG, "No files will be deleted. A preview output is given instead."))
            .Append((VERBOSE_FLAG, "Outputs filenames of duplicates found."))
            .Append((HELP_FLAG, "Shows this help text."));

        _ = helpText.Append(CreateOptionText(options));
        return helpText.ToString();
    }

    private static IEnumerable<IDeduplicator> GetDeduplicators(string[] args)
    {
        IEnumerable<IDeduplicator> deduplicators = IDeduplicator.Implementations;
        foreach (IDeduplicator deduplicator in deduplicators)
        {
            if (args.Contains(GetFlagName(deduplicator.Name), StringComparer.OrdinalIgnoreCase))
            {
                return [deduplicator];
            }
        }

        return deduplicators;
    }

    private static string GetFlagName(string name)
        => $"--{name}-only";

    private static StringBuilder CreateOptionText(IEnumerable<(string flagName, string description)> values)
    {
        int maxLength = values.Max(x => x.flagName.Length) + 1;
        StringBuilder flagBuilder = new StringBuilder();
        foreach ((string flagName, string description) in values)
        {
            string paddedName = $"{flagName}:".PadRight(maxLength, ' ');
            _ = flagBuilder.AppendLine($"    {paddedName} {description}");
        }

        return flagBuilder;
    }
}
