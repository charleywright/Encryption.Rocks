using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public static partial class Menu
{
    public static Task Start()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;
        Console.Clear();

        ConsoleForegroundColor(Statics.InputState, 0);
        Console.WriteLine($"\n  ╔{new string('═', Console.BufferWidth - 6)}╗");
        Console.WriteLine($"  ║ Server URL: {Statics.ServerUrl}{new string(' ', Console.BufferWidth - 19 - Statics.ServerUrl.Length)}║");
        Console.WriteLine($"  ╚{new string('═', Console.BufferWidth - 6)}╝\n");

        ConsoleForegroundColor(Statics.InputState, 1);
        Console.Write($"  ╔{new string('═', Console.BufferWidth - 30)}╗");
        ConsoleForegroundColor(Statics.InputState, 2);
        Console.WriteLine($"  ╔{new string('═', 20)}╗");
        ConsoleForegroundColor(Statics.InputState, 1);
        Console.Write($"  ║ Room hash: {Statics.RoomHash}{new string(' ', Console.BufferWidth - 42 - Statics.RoomHash.Length)}║");
        ConsoleForegroundColor(Statics.InputState, 2);
        Console.WriteLine($"  ║        Join        ║");
        ConsoleForegroundColor(Statics.InputState, 1);
        Console.Write($"  ╚{new string('═', Console.BufferWidth - 30)}╝");
        ConsoleForegroundColor(Statics.InputState, 2);
        Console.WriteLine($"  ╚{new string('═', 20)}╝");

        switch (Statics.InputState)
        {
            case 1:
                Console.SetCursorPosition(15 + Statics.RoomHash.Length, 6);
                break;
            case 2:
                Console.SetCursorPosition(Console.BufferWidth - 11, 6);
                break;
            default:
                Console.SetCursorPosition(16 + Statics.ServerUrl.Length, 2);
                break;
        }

        ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
        switch (consoleKeyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                Statics.InputState = 0;
                break;
            case ConsoleKey.RightArrow:
                Statics.InputState++;
                if (Statics.InputState > 2) Statics.InputState = 2;
                break;
            case ConsoleKey.DownArrow:
                if (Statics.InputState == 0) Statics.InputState = 1;
                break;
            case ConsoleKey.LeftArrow:
                Statics.InputState--;
                if (Statics.InputState < 0) Statics.InputState = 0;
                break;
            case ConsoleKey.Enter:
                Statics.ProgramState = 1;
                break;
            case ConsoleKey.Backspace:
                switch (Statics.InputState)
                {
                    case 0:
                        if (Statics.ServerUrl.Length > 0) Statics.ServerUrl = Statics.ServerUrl.Substring(0, Statics.ServerUrl.Length - 1);
                        break;
                    case 1:
                        if (Statics.RoomHash.Length > 0) Statics.RoomHash = Statics.RoomHash.Substring(0, Statics.RoomHash.Length - 1);
                        break;
                }
                break;
            default:
                List<char> validChars = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ':', '/', '.', '-' };
                if (validChars.Contains(consoleKeyInfo.KeyChar))
                {
                    switch (Statics.InputState)
                    {
                        case 0:
                            if (Statics.ServerUrl.Length < Console.BufferWidth - 19)
                                Statics.ServerUrl += consoleKeyInfo.KeyChar;
                            break;
                        case 1:
                            if (Statics.RoomHash.Length < Console.BufferWidth - 42)
                                Statics.RoomHash += consoleKeyInfo.KeyChar;
                            break;
                    }
                }
                break;
        }
        return Task.CompletedTask;
    }
}