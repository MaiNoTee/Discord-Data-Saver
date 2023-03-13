using Discord;
using Discord.WebSocket;
using static DiscordDataSaver.Core.Commands;
using static DiscordDataSaver.Core.Handlers;

namespace DiscordDataSaver.Core;

public class MainCommandHandler
{

	const string Prefix = "bot, ";

	static SocketGuild Guild { get; set; } = null!;
	static DiscordSocketClient Client { get; set; } = null!;

	public MainCommandHandler(DiscordSocketClient client, ulong guildId)
	{
		Client = client;
		Guild = Client.GetGuild(guildId);
	}

	static async Task MakeGuildCommand(ApplicationCommandProperties commandProperties) =>
		await Guild.CreateApplicationCommandAsync(commandProperties);

	static async Task MakeGlobalCommand(ApplicationCommandProperties commandProperties) =>
		await Client.CreateGlobalApplicationCommandAsync(commandProperties);

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

	public async Task ConnectSlashCommands()
	{

		await MakeGuildCommand(ReplyCommandProperties());
		await MakeGuildCommand(SaveCommandProperties());
		await MakeGuildCommand(DeleteCommandProperties());
		await MakeGuildCommand(PaintMeCommandProperties());
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
}
