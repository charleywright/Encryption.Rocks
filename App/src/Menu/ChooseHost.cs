using System;
public static partial class Menu
{
  public static void MainMenu()
  {
    Console.Clear();
    Console.WriteLine("Please select a host, or create a new one\n");

    if (Statics.selectedHost < 0) Statics.selectedHost = 0;
    if (Statics.selectedHost + 1 > Statics.hosts.Count) Statics.selectedHost = Statics.hosts.Count - 1;

    int columnWidth = Convert.ToInt32(Math.Floor((double)Console.WindowWidth / 2));
    for (int i = 0; i < Statics.hosts.Count; i++)
    {
      Host host = Statics.hosts[i];

      if (i == Statics.selectedHost)
      {
        Console.BackgroundColor = ConsoleColor.DarkRed;
        Console.ForegroundColor = ConsoleColor.White;
      }

      string msg;
      if (i != Statics.hosts.Count - 1) { msg = $"{host.url}:{host.port}"; } else msg = $"{host.url}";
      Console.Write($"{msg}{new string(' ', columnWidth - msg.Length)}");
      Console.ResetColor();
      if (i % 2 != 0 && i != 0) Console.WriteLine();
    }

    ConsoleKeyInfo choice = Console.ReadKey();
    char choiceChar = choice.KeyChar;
    Console.WriteLine(choice.Key == ConsoleKey.RightArrow);
    switch (choice.Key)
    {
      case ConsoleKey.UpArrow:
        Statics.selectedHost -= 2;
        Menu.MainMenu();
        break;
      case ConsoleKey.DownArrow:
        Statics.selectedHost += 2;
        Menu.MainMenu();
        break;
      case ConsoleKey.LeftArrow:
        Statics.selectedHost--;
        Menu.MainMenu();
        break;
      case ConsoleKey.RightArrow:
        Statics.selectedHost++;
        Menu.MainMenu();
        break;
      case ConsoleKey.Enter:
        // Validate host & attempt to join
        Console.Clear();
        if (Statics.selectedHost + 1 == Statics.hosts.Count)
        {
          Console.WriteLine("Created new host totally lmfao");
        }
        else
        {
          Host chosenOne = Statics.hosts[Statics.selectedHost];
          Console.WriteLine($"{chosenOne.url}:{chosenOne.port}");
        }
        break;
    }
  }
}