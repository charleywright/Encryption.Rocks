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
      Statics.renderer = new Renderer(url);
    });

    Statics.connectedServer.socket.On("user-join", data =>
    {
      Statics.renderer.RenderUserUpdate($"{data.GetValue<string>()} has joined the channel");
    });

    Statics.connectedServer.socket.On("user-leave", data =>
    {
      Statics.renderer.RenderUserUpdate($"{data.GetValue<string>()} has left the channel");
    });

    Statics.connectedServer.socket.On("new-message", async data =>
    {
      List<Message> messages = JsonConvert.DeserializeObject<List<Message>>(data.GetValue<string>());
      List<Message> likelyMessages = messages.Where(el => el.aim == Statics.connectedServer.ClientId).ToList();
      likelyMessages[0].content = Helpers.RSADecrypt(likelyMessages[0].content, Statics.connectedServer.ClientKeyPair.Private);
      Statics.renderer.RenderMessage(likelyMessages[0]);
    });

    try
    {
      await Statics.connectedServer.socket.ConnectAsync();
    }
    catch (Exception)
    {
      connectingSpinner.Dispose();
      Console.ResetColor();
      Console.Clear();
      Console.WriteLine($"Could not connect to server: {url}");
      System.Threading.Thread.Sleep(1000);
      Menu.MainMenu();
    };

    while (Statics.connectedToServer)
    {
      ConsoleKeyInfo keyInfo = Console.ReadKey(true);
      switch (keyInfo.Key)
      {
        case ConsoleKey.L:
          if (Statics.renderer.CommandMode)
          {
            Statics.connectedToServer = false;
            await Statics.connectedServer.socket.EmitAsync("manual-disconnect", Statics.connectedServer.ClientName);
            await Statics.connectedServer.socket.DisconnectAsync();
            Menu.MainMenu();
          }
          else Statics.currentMsg += keyInfo.KeyChar;
          break;
        case ConsoleKey.Q:
          if (Statics.renderer.CommandMode)
          {
            await Statics.connectedServer.socket.EmitAsync("manual-disconnect", Statics.connectedServer.ClientName);
            await Statics.connectedServer.socket.DisconnectAsync();
            Environment.Exit(0);
          }
          else Statics.currentMsg += keyInfo.KeyChar;
          break;
        case ConsoleKey.Backspace:
          if (Statics.currentMsg.Length > 0)
          {
            Statics.currentMsg = Statics.currentMsg.Substring(0, Statics.currentMsg.Length - 1);
          }
          break;
        case ConsoleKey.Enter:
          if (Statics.currentMsg.Length > 0)
          {

            Message message = new Message(Helpers.RSAEncrypt(Statics.currentMsg, Statics.connectedServer.ServerKey), Statics.connectedServer.ClientId, "");
            string json = JsonConvert.SerializeObject(message);
            await Statics.connectedServer.socket.EmitAsync("message-send", json);
            Statics.currentMsg = "";
          }
          break;
        default:
          if (keyInfo.KeyChar == ':')
          {
            if (Statics.renderer.CommandMode) Statics.currentMsg += ":";
            Statics.renderer.CommandMode = !Statics.renderer.CommandMode;
          }
          else
          {
            Statics.currentMsg += keyInfo.KeyChar;
          }
          break;
      }
      Statics.renderer.RenderInput();
    }
    return Task.CompletedTask;
  }
}