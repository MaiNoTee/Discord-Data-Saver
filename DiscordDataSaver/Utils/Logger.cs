using Discord;

namespace DiscordDataSaver.Utils;

public static class Logger
{
	public static Task Log(LogMessage msg)
	{
		Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} [{msg.Severity}] {msg.Message}");
		return Task.CompletedTask;
	}
}
