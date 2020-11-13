using System;
public static partial class Helpers
{
  public static void DrawTypePrompt()
  {
    Helpers.ClearConsoleLine(Console.WindowHeight);
    Console.SetCursorPosition(0, Console.WindowHeight);
    DateTime now = DateTime.Now;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Write($"[{now.ToString("HH:mm:ss")}] ");
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Write($"({Statics.connectedServer.ClientId} - {Statics.connectedServer.ClientName}) ");
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write($"> {Statics.currentMsg}");
  }
}