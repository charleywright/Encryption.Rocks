using System.Threading;
using System.Collections.Generic;

public static class Statics
{
  public static Renderer renderer { get; set; }
  public static Timer serverJoinTimeout { get; set; }
  public static List<Host> hosts { get; set; }
  public static Server connectedServer { get; set; }
  public static int selectedHost { get; set; }
  public static int newHostCursorPosition { get; set; }
  public static string newHostProtocol { get; set; }
  public static string newHostDomain { get; set; }
  public static string newHostPort { get; set; }
  public static string currentMsg { get; set; }
  public static bool connectedToServer { get; set; }
}