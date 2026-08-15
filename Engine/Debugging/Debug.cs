using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace Engine.Debugging;

public static class Debug
{
    // public static string Date => $"{DateTime.Now:dd-MM-yyyy HH:mm:ss}";
    public static string Date => $"{DateTime.Now:HH:mm:ss}";
    
    public static List<ConsoleLog> ConsoleLogs { get; } = new();
    
    public static void Log(string message, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        SetColor(ConsoleColor.White);
        Console.WriteLine($"{Date} [INFO]:  {message}");
    }
    
    public static void Warn(string message, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        SetColor(ConsoleColor.Yellow);
        Console.WriteLine($"{Date} [WARN]:  {message} ({Path.GetFileName(file)}:{line})");
    }
    
    public static void Error(string message, [CallerFilePath] string file = "", [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
    {
        SetColor(ConsoleColor.Red);
        Console.WriteLine($"{Date} [ERROR]: {message} ({Path.GetFileName(file)}:{line} in {member}())");
    }
    
    private static void SetColor(ConsoleColor color)
    {
        if (ConsoleWriter.Instance != null)
            ConsoleWriter.Instance.CurrentColor = color;
        else
            Console.ForegroundColor = color;
    }

}