public class Message
{
  public string content;
  public string sender;
  public string aim;

  public Message(string content, string sender, string aim)
  {
    this.content = content;
    this.sender = sender;
    this.aim = aim;
  }
}