using System;
using System.Threading;

public static partial class Helpers
{
  public static void UpdateTimestamp()
  {
    Timer timer = new Timer(UpdateTimeStampCallback, "", TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
  }

  private static void UpdateTimeStampCallback(object state)
  {
    if (Statics.connectedServer.socket.Connected)
    {
      DrawTypePrompt();
    }
  }
}