using System;
using System.Threading;

public class Renderer
{
  private string ServerUrl;
  private Thread renderThread;
  public bool CommandMode;
  public Renderer(string url)
  {
    this.ServerUrl = url;
    this.CommandMode = false;
    Console.Clear();
    this.TopText();
    this.RenderInput();
  }

  private void TopText()
  {
    if (Statics.connectedToServer)
    {
      Console.SetCursorPosition(0, 0);
      Console.ResetColor();
      this.ClearConsoleLine(0);
      Console.WriteLine(Helpers.CenterText($"Connected as {Statics.connectedServer.ClientName} to {this.ServerUrl}"));
    }
  }

  private void ClearConsoleLine(int line)
  {
    Console.SetCursorPosition(0, line);
    int currentLineCursor = Console.CursorTop;
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write(new string(' ', Console.WindowWidth));
    Console.SetCursorPosition(0, currentLineCursor);
  }

  public void RenderInput(object state = null)
  {
    if (Statics.connectedToServer)
    {
      this.TopText();
      Console.ResetColor();
      Console.SetCursorPosition(0, Console.BufferHeight);
      this.ClearConsoleLine(Console.BufferHeight);
      if (this.CommandMode)
      {
        Console.Write($":");
      }
      else
      {
        Console.Write($"> {Statics.currentMsg}");
      }
    }
  }

  public void RenderMessage(Message message)
  {
    if (Statics.connectedToServer)
    {
      Console.SetCursorPosition(0, Console.BufferHeight);
      this.ClearConsoleLine(Console.BufferHeight);
      DateTime now = DateTime.Now;
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Write($"[{now.ToString("HH:mm:ss")}] ");
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.Write($"({message.sender}) ");
      Console.ResetColor();
      Console.Write(message.content + "\n");
      this.RenderInput();
    }
  }

  public void RenderUserUpdate(string message)
  {
    if (Statics.connectedToServer)
    {
      Console.SetCursorPosition(0, Console.BufferHeight);
      this.ClearConsoleLine(Console.BufferHeight);
      Console.ForegroundColor = ConsoleColor.DarkGray;
      Console.Write(message + "\n");
      this.RenderInput();
    }
  }
}