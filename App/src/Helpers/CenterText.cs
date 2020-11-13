using System;

public static partial class Helpers
{
  public static string CenterText(string text, char seperator = ' ')
  {
    int buffer = (int)Math.Floor((double)(Console.WindowWidth - text.Length) / 2);
    return $"{new string(seperator, buffer)}{text}";
  }
}