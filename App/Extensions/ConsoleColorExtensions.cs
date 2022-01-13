namespace App.Extensions;

public static class ConsoleColorExtensions
{
    public static void Write(this ConsoleColor color, object value)
    {
        Console.ForegroundColor = color;
        Console.Write(value);
        Console.ResetColor();
    }

    public static void WriteLine(this ConsoleColor color, object value)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(value);
        Console.ResetColor();
    }
}