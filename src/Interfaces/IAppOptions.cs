namespace BinaryPatrick.Deduplicator.Interfaces;

public interface IAppOptions
{
    string Path { get; }
    IEnumerable<IDeduplicator> Deduplicators { get; }
    bool IsVerbose { get; }
    bool IsDryRun { get; }
}