using System.Drawing;
using Discord;
using Discord.WebSocket;
using DiscordDataSaver.Structures;

namespace DiscordDataSaver.Core;

public abstract class Handlers
{
	public static async Task HandleDeleteCommand(SocketSlashCommand msg)
	{
		double count = (double)msg.Data.Options.First().Value;
		try
		{
			IEnumerable<IMessage> messages
				= await msg.Channel.GetMessagesAsync(msg.Id, Direction.Before, (int)count).FlattenAsync();
			await (msg.Channel as SocketTextChannel)!.DeleteMessagesAsync(messages);
			await msg.RespondAsync($"Deleted {count} last messages", ephemeral: true);
		}
		catch (Exception e)
		{
			await msg.RespondAsync(e.Message, ephemeral: true);
		}
	}

	public static async Task HandleSaveCommand(SocketSlashCommand msg)
	{
		double option = (double)msg.Data.Options.First().Value;
		double count = (double)msg.Data.Options.Last().Value;
		IEnumerable<IMessage> messages
			= await msg.Channel.GetMessagesAsync(msg.Id, Direction.Before, (int)count).FlattenAsync();
		try
		{
			var customMessages = messages
				.Reverse()
				.Select(m => new CustomMsgStructure(m.Author.Username, m.CreatedAt.DateTime, m.Content))
				.ToList();
			await File.WriteAllTextAsync("messages.txt", FileConstructor.ConstructMessage(customMessages));
			await msg.RespondWithFileAsync("messages.txt", ephemeral: true);
			File.Delete("messages.txt");
		}
		catch (Exception exception)
		{
			await msg.RespondAsync(exception.Message, ephemeral: true);
		}
	}

	public static async Task HandlePaintMeCommand(SocketSlashCommand msg)
	{
		var guild = Program.Client.GetGuild(msg.GuildId!.Value);
		var userRole = guild.GetUser(msg.User.Id).Roles.OrderByDescending(x => x.Position).First();
		var color = ColorTranslator.FromHtml((string)msg.Data.Options.First().Value);
		if (userRole is not null)
		{
			await userRole.ModifyAsync(x => x.Color = new Discord.Color(color.R, color.G, color.B));
			await msg.RespondAsync("Changed color", ephemeral: true);
		}
		else await msg.RespondAsync("Role not found", ephemeral: true);
	}
}
