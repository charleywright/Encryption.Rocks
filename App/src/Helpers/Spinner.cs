// Taken from https://stackoverflow.com/a/33685254

using System;
using System.Threading;
public class Spinner : IDisposable
{
  private const string Sequence = @"/-\|";
  private int counter = 0;
  private readonly int left;
  private readonly int top;
  private readonly int delay;
  private bool active;
  private readonly Thread thread;

  public Spinner(int left, int top, int delay = 100)
  {
    this.left = left;
    this.top = top;
    this.delay = delay;
    thread = new Thread(Spin);
  }

  public void Start()
  {
    active = true;
    if (!thread.IsAlive)
      thread.Start();
  }

  public void Stop()
  {
    active = false;
    Draw(' ');
  }

  private void Spin()
  {
    while (active)
    {
      Turn();
      Thread.Sleep(delay);
    }
  }

  private void Draw(char c)
  {
    Console.SetCursorPosition(left, top);
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.Write(c);
  }

  private void Turn()
  {
    Draw(Sequence[++counter % Sequence.Length]);
  }

  public void Dispose()
  {
    Stop();
  }
}