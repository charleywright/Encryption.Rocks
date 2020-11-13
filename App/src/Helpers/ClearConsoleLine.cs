using System;
public static partial class Helpers
{
  public static void ClearConsoleLine(int line)
  {
    Console.SetCursorPosition(0, line);
    int currentLineCursor = Console.CursorTop;
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write(new string(' ', Console.WindowWidth));
    Console.SetCursorPosition(0, currentLineCursor);
  }
}