using System;
public static partial class Menu
{
  public static void MainMenu(object state = null)
  {
    Console.Clear();
    try { Statics.serverJoinTimeout.Dispose(); } catch (Exception) { };
    Statics.connectedToServer = false;
    Console.WriteLine("Join a host with Enter, Delete a host with d, Refresh the host list with r, Add a new host with the option or n, Exit with q\n");

    if (Statics.selectedHost < 0) Statics.selectedHost = 0;
    if (Statics.selectedHost + 1 > Statics.hosts.Count) Statics.selectedHost = Statics.hosts.Count - 1;

    int columnWidth = Convert.ToInt32(Math.Floor((double)Console.WindowWidth / 2));
    for (int i = 0; i < Statics.hosts.Count; i++)
    {
      Host host = Statics.hosts[i];

      if (i == Statics.selectedHost)
      {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.Black;
      }

      string msg;
      if (i != Statics.hosts.Count - 1) { msg = $"{host.url}:{host.port}"; } else msg = $"{host.url}";
      Console.Write($"{msg}{new string(' ', columnWidth - msg.Length)}");
      Console.ResetColor();
      if (i % 2 != 0 && i != 0) Console.WriteLine();
    }

    ConsoleKeyInfo choice = Console.ReadKey(true);
    char choiceChar = choice.KeyChar;
    switch (choice.Key)
    {
      case ConsoleKey.Tab:
        if (Statics.selectedHost == Statics.hosts.Count - 1)
        {
          Statics.selectedHost = 0;
        }
        else Statics.selectedHost++;
        MainMenu();
        break;
      case ConsoleKey.UpArrow:
        Statics.selectedHost -= 2;
        MainMenu();
        break;
      case ConsoleKey.DownArrow:
        Statics.selectedHost += 2;
        MainMenu();
        break;
      case ConsoleKey.LeftArrow:
        Statics.selectedHost--;
        MainMenu();
        break;
      case ConsoleKey.RightArrow:
        Statics.selectedHost++;
        MainMenu();
        break;
      case ConsoleKey.Enter:
        if (Statics.selectedHost + 1 == Statics.hosts.Count)
        {
          Statics.newHostCursorPosition = 0;
          Statics.newHostProtocol = "http";
          Statics.newHostPort = "80";
          Statics.newHostDomain = "";
          NewHost();
        }
        else
        {
          Helpers.JoinServer(Statics.hosts[Statics.selectedHost]).Wait();
        }
        break;
      case ConsoleKey.D:
        if (Statics.selectedHost != Statics.hosts.Count - 1)
        {
          Statics.hosts.RemoveAt(Statics.selectedHost);
          Helpers.SaveHosts();
        }
        MainMenu();
        break;
      case ConsoleKey.R:
        Statics.hosts = Helpers.LoadHosts();
        Statics.hosts.Add(new Host(0, "[New Connection]"));
        MainMenu();
        break;
      case ConsoleKey.N:
        Statics.newHostCursorPosition = 0;
        Statics.newHostProtocol = "http";
        Statics.newHostPort = "80";
        NewHost();
        break;
      case ConsoleKey.Q:
        Console.Clear();
        Environment.Exit(0);
        break;
      default:
        MainMenu();
        break;
    }
  }
}