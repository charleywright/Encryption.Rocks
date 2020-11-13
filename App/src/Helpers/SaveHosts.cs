using System.IO;
using static Newtonsoft.Json.JsonConvert;

public static partial class Helpers
{
  public static void SaveHosts(string fileName = "hosts.json")
  {
    Statics.hosts.RemoveAt(Statics.hosts.Count - 1);
    string fileJson = SerializeObject(Statics.hosts);
    File.WriteAllText(fileName, fileJson);
    Statics.hosts.Add(new Host(0, "[New Connection]"));
  }
}