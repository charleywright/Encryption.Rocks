using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using SocketIOClient;
public static partial class Helpers
{
  public static async Task<Task> JoinServer(Host serverHost)
  {
    Statics.connectedServer = new Server();
  getClientName:
    Statics.connectedServer.ClientName = "";
    Console.SetCursorPosition(0, 3 + (Statics.hosts.Count / 2));
    Console.Write("What would you like your name to be? ");
    Statics.connectedServer.ClientName = Console.ReadLine();
    if (Statics.connectedServer.ClientName == "") goto getClientName;

    Statics.connectedServer.Host = serverHost;
    Statics.connectedToServer = true;
    Statics.currentMsg = "";
    Console.SetCursorPosition(0, 4 + (Statics.hosts.Count / 2));
    Console.Write("Connecting ");
    Spinner connectingSpinner = new Spinner(12, 4 + (Statics.hosts.Count / 2));
    connectingSpinner.Start();
    string url = $"{serverHost.url}:{serverHost.port}";
    Statics.connectedServer.socket = new SocketIO(url);

    Statics.connectedServer.socket.OnDisconnected += (sender, e) =>
    {
      Statics.connectedToServer = false;
      Menu.MainMenu();
    };

    Statics.connectedServer.socket.On("auth-needed", async data =>
    {
      Statics.connectedServer.ClientKeyPair = Helpers.GenKeyPair();
      AuthNeeded req = new AuthNeeded();
      req.ClientKey = Statics.connectedServer.ClientKeyPair.Public;
      req.ClientName = Statics.connectedServer.ClientName;
      string json = JsonConvert.SerializeObject(req);
      await Statics.connectedServer.socket.EmitAsync("client-auth", json);
    });

    Statics.connectedServer.socket.On("auth-succesful", async data =>
    {
      AuthResponse response = JsonConvert.DeserializeObject<AuthResponse>(data.GetValue<string>());
      Statics.connectedServer.ServerKey = response.ServerKey;
      Statics.connectedServer.ClientId = response.ClientId;
      connectingSpinner.Dispose();
      Console.Clear();
      Helpers.DrawHeader(url);
      Helpers.DrawTypePrompt();
      Helpers.UpdateTimestamp();

      // Testing, remove for build
      System.Threading.Thread.Sleep(500);
      await Statics.connectedServer.socket.EmitAsync("testage");
    });

    Statics.connectedServer.socket.On("user-join", data =>
    {
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.SetCursorPosition(0, Console.BufferHeight);
      Helpers.ClearConsoleLine(Console.BufferHeight);
      DateTime now = DateTime.Now;
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Write($"[{now.ToString("HH:mm:ss")}] ");
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.Write($"{data.GetValue<string>()} has joined the channel.\n");
      Console.ForegroundColor = ConsoleColor.White;
      Helpers.DrawHeader(url);
      Helpers.DrawTypePrompt();
    });

    Statics.connectedServer.socket.On("new-message", async data =>
    {
      List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(data.GetValue<string>());
      List<Message> likelyMessages = messages.Where(el => el.aim == Statics.connectedServer.ClientId).ToList();
      likelyMessages[0].content = Helpers.RSADecrypt(likelyMessages[0].content, Statics.connectedServer.ClientKeyPair.Private);
      Console.SetCursorPosition(0, Console.BufferHeight);
      DateTime now = DateTime.Now;
      Console.ForegroundColor = ConsoleColor.Red;
      Console.Write($"[{now.ToString("HH:mm:ss")}] ");
      Console.ForegroundColor = ConsoleColor.Magenta;
      Console.Write($"({likelyMessages[0].sender}) ");
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write(likelyMessages[0].content + "\n");
      Helpers.DrawHeader(url);
      Helpers.DrawTypePrompt();
    });

    await Statics.connectedServer.socket.ConnectAsync();

    while (Statics.connectedToServer)
    {
      ConsoleKeyInfo keyInfo = Console.ReadKey(true);
      switch (keyInfo.Key)
      {
        case ConsoleKey.Backspace:
          if (Statics.currentMsg.Length > 0)
          {
            Statics.currentMsg = Statics.currentMsg.Substring(0, Statics.currentMsg.Length - 1);
          }
          break;
        case ConsoleKey.Enter:
          Message message = new Message(Helpers.RSAEncrypt(Statics.currentMsg, Statics.connectedServer.ServerKey), Statics.connectedServer.ClientId, "");
          string json = JsonConvert.SerializeObject(message);
          await Statics.connectedServer.socket.EmitAsync("message-send", json);
          Statics.currentMsg = "";
          break;
        default:
          Statics.currentMsg += keyInfo.KeyChar;
          break;
      }
      Helpers.DrawTypePrompt();
    }
    return Task.CompletedTask;
  }
}