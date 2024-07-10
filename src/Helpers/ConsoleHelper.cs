namespace BinaryPatrick.Deduplicator.Helpers;
internal static class ConsoleHelper
{
    public static void WriteError(string? message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void WriteInformation(string? message)
    {
        Console.ResetColor();
        Console.WriteLine(message);
    }
}
