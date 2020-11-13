using System;
using System.Collections.Generic;
public static partial class Menu
{
  public static void NewHost()
  {
    Console.Clear();
    Console.ResetColor();
    string newHostString = $"";
    if (!String.IsNullOrEmpty(Statics.newHostDomain)) newHostString += Statics.newHostProtocol + "://" + Statics.newHostDomain + ":" + Statics.newHostPort;
    Console.Write($" Please enter the host's details in the provided fields. {(newHostString.Length != 0 ? "Host: " : "")}");
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write($"{(newHostString.Length != 0 ? newHostString : "")}\n");
    Console.ResetColor();

    if (Statics.newHostCursorPosition < 0) Statics.newHostCursorPosition = 0;
    if (Statics.newHostCursorPosition > 4) Statics.newHostCursorPosition = 4;
    // Console.WriteLine($" ╔{new string('═', Console.WindowWidth - 4)}╗");
    // string newHostString = $"";
    // if (!String.IsNullOrEmpty(Statics.newHostDomain)) newHostString += Statics.newHostDomain + ":" + Statics.newHostPort;
    // Console.WriteLine($" ║ Host: {newHostString}{new string(' ', Console.WindowWidth - 12 - newHostString.Length)} ║");
    // Console.WriteLine($" ╚{new string('═', Console.WindowWidth - 4)}╝");

    if (Statics.newHostCursorPosition == 0) { Console.ForegroundColor = ConsoleColor.DarkRed; } else Console.ResetColor();
    Console.Write($"\n ╔{new string('═', 32)}╗ ");
    if (Statics.newHostCursorPosition == 1) { Console.ForegroundColor = ConsoleColor.DarkRed; } else Console.ResetColor();
    Console.Write($" ╔{new string('═', Console.WindowWidth - 76)}╗ ");
    if (Statics.newHostCursorPosition == 2) { Console.ForegroundColor = ConsoleColor.DarkRed; } else Console.ResetColor();
    Console.Write($" ╔{new string('═', 32)}╗\n");
    if (Statics.newHostCursorPosition == 0) { Console.ForegroundColor = ConsoleColor.DarkRed; } else Console.ResetColor();
    Console.Write($" ║ Protocol: {Statics.newHostProtocol}{new string(' ', 21 - Statics.newHostProtocol.Length)}║ ");
    if (Statics.newHostCursorPosition == 1) { Console.ForegroundColor = ConsoleColor.DarkRed; } else Console.ResetColor();
    Console.Write($" ║ Hostname: {Statics.newHostDomain}{new string(' ', Console.WindowWidth - 87 - Statics.newHostDomain.Length)}║ ");
    if (Statics.newHostCursorPosition == 2) { Console.ForegroundColor = ConsoleColor.DarkRed; } else Console.ResetColor();
    Console.Write($" ║ Port: {Statics.newHostPort}{new string(' ', 25 - Statics.newHostPort.Length)}║\n");
    if (Statics.newHostCursorPosition == 0) { Console.ForegroundColor = ConsoleColor.DarkRed; } else Console.ResetColor();
    Console.Write($" ╚{new string('═', 32)}╝ ");
    if (Statics.newHostCursorPosition == 1) { Console.ForegroundColor = ConsoleColor.DarkRed; } else Console.ResetColor();
    Console.Write($" ╚{new string('═', Console.WindowWidth - 76)}╝ ");
    if (Statics.newHostCursorPosition == 2) { Console.ForegroundColor = ConsoleColor.DarkRed; } else Console.ResetColor();
    Console.Write($" ╚{new string('═', 32)}╝\n\n");

    int spaceBetweenBtns = 20;
    int btnBuffer = (int)Math.Floor((double)(Console.WindowWidth - spaceBetweenBtns - 18) / 2);
    Console.Write(new string(' ', btnBuffer));
    if (Statics.newHostCursorPosition == 3)
    {
      Console.BackgroundColor = ConsoleColor.DarkRed;
      Console.ForegroundColor = ConsoleColor.Black;
    }
    else Console.ResetColor();
    Console.Write("<CANCEL>");
    Console.ResetColor();
    Console.Write(new string(' ', spaceBetweenBtns));
    if (Statics.newHostCursorPosition == 4)
    {
      Console.BackgroundColor = ConsoleColor.DarkRed;
      Console.ForegroundColor = ConsoleColor.Black;
    }
    Console.Write("<ADD HOST>");
    Console.ResetColor();

    switch (Statics.newHostCursorPosition)
    {
      case 0:
        Console.SetCursorPosition(13 + Statics.newHostProtocol.Length, 3);
        break;
      case 1:
        Console.SetCursorPosition(49 + Statics.newHostDomain.Length, 3);
        break;
      case 2:
        Console.SetCursorPosition(Console.WindowWidth - 27 + Statics.newHostPort.Length, 3);
        break;
      default:
        Console.SetCursorPosition(Console.WindowWidth, Console.WindowHeight);
        break;
    }

    ConsoleKeyInfo choice = Console.ReadKey();
    char choiceChar = choice.KeyChar;
    switch (choice.Key)
    {
      case ConsoleKey.Tab:
        if (Statics.newHostCursorPosition == 3)
        {
          Statics.newHostCursorPosition = 0;
        }
        else Statics.newHostCursorPosition++;
        NewHost();
        break;
      case ConsoleKey.UpArrow:
        Statics.newHostCursorPosition = 1;
        NewHost();
        break;
      case ConsoleKey.DownArrow:
        Statics.newHostCursorPosition = 4;
        NewHost();
        break;
      case ConsoleKey.LeftArrow:
        Statics.newHostCursorPosition--;
        NewHost();
        break;
      case ConsoleKey.RightArrow:
        Statics.newHostCursorPosition++;
        NewHost();
        break;
      case ConsoleKey.Enter:
        switch (Statics.newHostCursorPosition)
        {
          case 3:
            MainMenu();
            break;
          case 4:
            Console.Clear();
            if (!String.IsNullOrEmpty(Statics.newHostDomain))
            {
              Host host = new Host();
              host.port = Convert.ToInt32(Statics.newHostPort);
              host.url = $"{Statics.newHostProtocol}://{Statics.newHostDomain}";
              Statics.hosts.RemoveAt(Statics.hosts.Count - 1);
              Statics.hosts.Add(host);
              Statics.hosts.Add(new Host(0, "[New Connection]"));
              Helpers.SaveHosts();
              Console.WriteLine($"Added {host.url}:{host.port} to the hosts list");
              System.Threading.Thread.Sleep(1000);
              MainMenu();
            }
            else
            {
              Console.WriteLine("Host cannot be empty");
              Statics.newHostCursorPosition = 1;
              System.Threading.Thread.Sleep(1000);
              NewHost();
            }
            break;
          default:
            NewHost();
            break;
        }
        break;
      default:
        switch (Statics.newHostCursorPosition)
        {
          case 0:
            if (Statics.newHostProtocol == "http" && choice.KeyChar == 's') Statics.newHostProtocol = "https";
            if (Statics.newHostProtocol == "https" && choice.Key == ConsoleKey.Backspace) Statics.newHostProtocol = "http";
            NewHost();
            break;
          case 1:
            if (choice.Key == ConsoleKey.Backspace)
            {
              if (Statics.newHostDomain.Length != 0)
                Statics.newHostDomain = Statics.newHostDomain.Substring(0, Statics.newHostDomain.Length - 1);
            }
            else
            {
              Statics.newHostDomain += choice.KeyChar;
            }
            NewHost();
            break;
          case 2:
            if (choice.Key == ConsoleKey.Backspace)
            {
              if (Statics.newHostPort.Length != 0)
                Statics.newHostPort = Statics.newHostPort.Substring(0, Statics.newHostPort.Length - 1);
            }
            else
            {
              List<char> validChars = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
              if (validChars.Contains(choice.KeyChar)) Statics.newHostPort += choice.KeyChar;
            }
            NewHost();
            break;
        }
        break;
    }
  }
}