using System;
using System.IO;
using System.Text;

namespace Engine.Debugging;

public record ConsoleLog(string Text, ConsoleColor Color);

public class ConsoleWriter : TextWriter
{
    public static ConsoleWriter Instance { get; private set; }
    
    private readonly TextWriter _original;
    public event Action<ConsoleLog>? OnWrite;
    public override Encoding Encoding => Encoding.UTF8;
    
    public ConsoleColor CurrentColor { get; set; } = ConsoleColor.Gray;
    
    public ConsoleWriter(TextWriter original)
    {
        _original = original;
        Instance = this;
    }

    public override void WriteLine(string? value)
    {
        _original.WriteLine(value);

        OnWrite?.Invoke(new ConsoleLog(value ?? string.Empty, CurrentColor));
    }

    public override void Write(string? value)
    {
        _original.Write(value);

        if (value != null)
            OnWrite?.Invoke(new ConsoleLog(value, CurrentColor));
    }
    
    public static System.Numerics.Vector4 GetImGuiColor(ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Black => new(0, 0, 0, 1),
            ConsoleColor.DarkBlue => new(0, 0, 0.5f, 1),
            ConsoleColor.DarkGreen => new(0, 0.5f, 0, 1),
            ConsoleColor.DarkCyan => new(0, 0.5f, 0.5f, 1),
            ConsoleColor.DarkRed => new(0.5f, 0, 0, 1),
            ConsoleColor.DarkMagenta => new(0.5f, 0, 0.5f, 1),
            ConsoleColor.DarkYellow => new(0.5f, 0.5f, 0, 1),
            ConsoleColor.Gray => new(0.75f, 0.75f, 0.75f, 1),
            ConsoleColor.DarkGray => new(0.5f, 0.5f, 0.5f, 1),
            ConsoleColor.Blue => new(0, 0, 1, 1),
            ConsoleColor.Green => new(0, 1, 0, 1),
            ConsoleColor.Cyan => new(0, 1, 1, 1),
            ConsoleColor.Red => new(1, 0, 0, 1),
            ConsoleColor.Magenta => new(1, 0, 1, 1),
            ConsoleColor.Yellow => new(1, 1, 0, 1),
            ConsoleColor.White => new(1, 1, 1, 1),
            _ => new(1, 1, 1, 1)
        };
    }
}