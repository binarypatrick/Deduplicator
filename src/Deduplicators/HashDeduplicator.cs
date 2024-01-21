using BinaryPatrick.Deduplicator.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace BinaryPatrick.Deduplicator.Deduplicators
{
    internal class HashDeduplicator : BaseDeduplicator, IDeduplicator
    {
        public void Deduplicate(string path)
        {
            IEnumerable<FileInfo> files = Directory.GetFiles(path)
                .Select(x => new FileInfo(x));

            IEnumerable<IEnumerable<FileInfo>> fileGroups = files.GroupBy(GetFileHash)
                .Select(x => x.AsEnumerable());

            foreach (IEnumerable<FileInfo> fileGroup in fileGroups)
            {
                DeleteSmallestDuplicates(fileGroup);
            }
        }

        private string GetFileHash(FileInfo file)
        {
            byte[] fileBytes = File.ReadAllBytes(file.FullName);
            byte[] hashBytes = MD5.HashData(fileBytes);

            string hash = ByteArrayToString(hashBytes);
            return hash;
        }

        private static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
