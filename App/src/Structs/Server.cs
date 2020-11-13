using SocketIOClient;
public class Server
{
  public Host Host;
  public string ServerKey;
  public RSAKeyPair ClientKeyPair;
  public string ClientName;
  public string ClientId;
  public SocketIO socket;
}