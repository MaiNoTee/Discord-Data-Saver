using Discord;

namespace DiscordDataSaver.Utils;

public static class Logger
{
    public static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}