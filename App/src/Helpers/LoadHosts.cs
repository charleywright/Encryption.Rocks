using System.IO;
using System.Collections.Generic;
using static Newtonsoft.Json.JsonConvert;

public static partial class Helpers
{
  public static List<Host> LoadHosts(string fileName = "hosts.json")
  {
    if (File.Exists(fileName))
    {
      string fileJson = File.ReadAllText(fileName);
      return DeserializeObject<List<Host>>(fileJson);
    }
    else return new List<Host>();
  }
}