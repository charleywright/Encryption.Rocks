using System;
public static partial class Helpers
{
  public static void Setup()
  {
    Console.Clear();
    Statics.selectedHost = 0;
    Statics.hosts = Helpers.LoadHosts();
    Host createNewHost = new Host(0, "Create new host", "", new System.Collections.Generic.List<string>());
    Statics.hosts.Add(createNewHost);
  }
}