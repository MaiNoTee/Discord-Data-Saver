namespace DiscordDataSaver.Structures;

public struct CustomMsgStructure
{
	public string Username { get; }
	public DateTime Date { get; }
	public string MessageText { get; }
	
	public CustomMsgStructure(string username, DateTime date, string messageText)
	{
		Username = username;
		Date = date;
		MessageText = messageText;
	}

}
