using System.Reflection;

namespace BinaryPatrick.Deduplicator.Interfaces;

public interface IDeduplicator
{
    public static IEnumerable<IDeduplicator> Implementations { get; } = GetImplementations();

    string Name { get; }
    string Description { get; }
    int Rank { get; }

    void Deduplicate(IAppOptions options);

    private static IEnumerable<IDeduplicator> GetImplementations()
        => Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !type.IsInterface && typeof(IDeduplicator).IsAssignableFrom(type))
            .Select(type => (IDeduplicator?)Activator.CreateInstance(type))
            .Where(x => x is not null)
            .OrderBy(x => x!.Rank)
            .ToList()!;
}
