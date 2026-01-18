using System;

namespace Engine.Debugging;

public static class Debug
{
    public static string Date => $"{DateTime.Now:dd-MM-yyyy HH:mm:ss}";
    
    public static void Log(string message)
    {
        Console.ResetColor();
        Console.WriteLine($"{Date} [INFO]: {message}");
    }
    
    public static void Warn(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Date} [WARN]: {message}");
        Console.ResetColor();
    }
    
    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{Date} [ERROR]: {message}");
        Console.ResetColor();
    }

}