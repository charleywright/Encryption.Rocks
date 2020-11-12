using System.Collections.Generic;

public struct Host
{
  public int port;
  public string url;
  public string privateKey;
  public List<string> publicKeys;

  public Host(int port, string url, string privateKey, List<string> publicKeys)
  {
    this.port = port;
    this.url = url;
    this.privateKey = privateKey;
    this.publicKeys = publicKeys;
  }
}