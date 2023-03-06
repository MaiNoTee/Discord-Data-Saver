using Discord;
using Discord.WebSocket;
using DiscordDataSaver.Structures;

namespace DiscordDataSaver.Core;

public static class CommandHandler
{
	const string Prefix = "bot, ";

	public static Task MessageReceived(SocketMessage msg)
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

	public static async Task ConnectSlashCommands(DiscordSocketClient? client, ulong guildId)
	{
		//TODO change guild commands to global on release!

		var guild = client!.GetGuild(guildId);

		await guild.CreateApplicationCommandAsync(
			new SlashCommandBuilder()
				.WithName("reply")
				.WithDescription("Reply command in DM")
				.Build());
		//example of global command
		//await client.CreateGlobalApplicationCommandAsync(
		//	new SlashCommandBuilder().WithName("reply").WithDescription("Reply command in DM").Build());

		await guild.CreateApplicationCommandAsync(
			new SlashCommandBuilder()
				.WithName("delete")
				.WithDescription("Delete number of messages")
				.AddOption(
					new SlashCommandOptionBuilder()
						.WithName("count")
						.WithDescription("count description")
						.WithRequired(true)
						.WithType(ApplicationCommandOptionType.Number))
				.Build());

		await guild.CreateApplicationCommandAsync(
			new SlashCommandBuilder()
				.WithName("save")
				.WithDescription("Save count of messages in text file")
				.AddOption(
					new SlashCommandOptionBuilder()
						.WithName("format")
						.WithDescription("Choose format file")
						.AddChoice("Txt", 1)
						.WithRequired(true)
						.WithType(ApplicationCommandOptionType.Number))
				.AddOption(
					new SlashCommandOptionBuilder()
						.WithName("count")
						.WithDescription("Count of messages")
						.WithRequired(true)
						.WithType(ApplicationCommandOptionType.Number))
				.Build());
	}

	public static async Task SlashCommandReceived(SocketSlashCommand msg)
	{
		double count;
		IEnumerable<IMessage> messages;

		switch (msg.CommandName)
		{
			case "reply":
				await msg.User.SendMessageAsync(msg.CommandName);
				await msg.RespondAsync("Message sent", ephemeral: true);
				break;
			case "delete":
				count = (double)msg.Data.Options.First().Value;
				try
				{
					messages = await msg.Channel.GetMessagesAsync(msg.Id, Direction.Before, (int)count).FlattenAsync();
					await (msg.Channel as SocketTextChannel)!.DeleteMessagesAsync(messages);
					await msg.RespondAsync($"Deleted {count} last messages", ephemeral: true);
				}
				catch (Exception e)
				{
					await msg.RespondAsync(e.Message, ephemeral: true);
				}

				break;
			case "save":
				double option = (double)msg.Data.Options.First().Value;
				count = (double)msg.Data.Options.Last().Value;
				messages = await msg.Channel.GetMessagesAsync(msg.Id, Direction.Before, (int)count).FlattenAsync();
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

				break;
		}
	}
}
