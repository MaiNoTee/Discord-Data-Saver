using System.Drawing;
using Discord;
using Discord.WebSocket;
using DiscordDataSaver.Commands;
using DiscordDataSaver.Structures;
using static DiscordDataSaver.Commands.Commands;

namespace DiscordDataSaver.Core;

public class CommandHandler
{
	const string Prefix = "bot, ";

	public static Task MessageReceived(SocketMessage msg) // context command
	{
		if (!msg.Content.StartsWith(Prefix) || msg.Author.IsBot) return Task.CompletedTask;
		var command = msg.Content[Prefix.Length..];
		switch (command)
		{
			case "guild":
				//msg.Channel.SendMessageAsync(msg.Channel.Id.ToString());
				//msg.Channel.SendMessageAsync(msg.Application.Id.ToString());
				break;
		}

		return Task.CompletedTask;
	}

	public static async Task ConnectSlashCommands(DiscordSocketClient client, ulong guildId)
	{
		//TODO change guild commands to global on release!

		var guild = client.GetGuild(guildId);

		await GuildReplyCommand(guild);
		await GuildDeleteCommand(guild);
		await GuildSaveCommand(guild);
		await GuildPaintMeCommand(guild);

		//await GlobalCommands.GlobalPaintMeCommand(client);

		//example of global command
		//await client.CreateGlobalApplicationCommandAsync(
		//	new SlashCommandBuilder().WithName("reply").WithDescription("Reply command in DM").Build());
	}

	public static async Task SlashCommandReceived(SocketSlashCommand msg)
	{
		switch (msg.CommandName)
		{
			case "reply":
				await msg.User.SendMessageAsync(msg.CommandName);
				await msg.RespondAsync("Message sent", ephemeral: true);
				break;
			case "delete":
				await HandleDeleteCommand(msg);
				break;
			case "save":
				await HandleSaveCommand(msg);
				break;
			case "paint-me":
				await HandlePaintMeCommand(msg);
				break;
		}
	}

	static async Task HandleDeleteCommand(SocketSlashCommand msg)
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

	static async Task HandleSaveCommand(SocketSlashCommand msg)
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

	static async Task HandlePaintMeCommand(SocketSlashCommand msg)
	{
		var guild = Program.Client.GetGuild(msg.GuildId!.Value);
		var currentChannel = guild.Channels.ToList().Find(x => x.Id == msg.GuildId!.Value);
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
