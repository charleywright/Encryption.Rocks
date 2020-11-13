using System;
public static partial class Helpers
{
  public static void Setup()
  {
    Console.Clear();
    Statics.selectedHost = 0;
    Statics.newHostDomain = "";
    Statics.hosts = Helpers.LoadHosts();
    Host createNewHost = new Host(0, "[New Connection]");
    Statics.hosts.Add(createNewHost);
  }
}