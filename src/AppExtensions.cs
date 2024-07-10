using System.Text;

namespace BinaryPatrick.Deduplicator;
internal static class AppExtensions
{
    public static string ToHexString(this byte[] arrInput)
    {
        StringBuilder sOutput = new StringBuilder(arrInput.Length);
        for (int i = 0; i < arrInput.Length; i++)
        {
            _ = sOutput.Append(arrInput[i].ToString("X2"));
        }
        return sOutput.ToString();
    }
}
