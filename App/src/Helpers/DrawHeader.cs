using System;
public static partial class Helpers
{
  public static void DrawHeader(string url)
  {
    Console.ResetColor();
    Console.SetCursorPosition(0, 0);
    Console.WriteLine(Helpers.CenterText($"Connected as {Statics.connectedServer.ClientName} to {url}"));
  }
}