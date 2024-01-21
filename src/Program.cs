using BinaryPatrick.Deduplicator.Deduplicators;
using BinaryPatrick.Deduplicator.Interfaces;

namespace BinaryPatrick.Deduplicator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("Missing folder path as argument");
            }

            string path = args[0];
            if (!Directory.Exists(path))
            {
                throw new ArgumentException("Folder path does not exist");
            }

            List<IDeduplicator> deduplicators = new()
            {
                new FilenameDeduplicator(),
                new HashDeduplicator(),
            };

            foreach (IDeduplicator deduplicator in deduplicators)
            {
                deduplicator.Deduplicate(path);
            }
        }
    }
}