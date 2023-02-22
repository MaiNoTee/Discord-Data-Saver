using System.Text;
using DiscordDataSaver.Structures;

namespace DiscordDataSaver.Core;

public static class FileConstructor
{
	public static string ConstructMessage(List<CustomMsgStructure> messages)
	{
		var sb = new StringBuilder();
		foreach (var msg in messages)
		{
			sb.AppendLine(
				$"{msg.Username}  {msg.Date.Day}.{msg.Date.Month}.{msg.Date.Year} {msg.Date.TimeOfDay.Hours}:{msg.Date.TimeOfDay.Minutes}" +
				$"\n  {msg.MessageText}");
		}

		return sb.ToString();
	}
}
