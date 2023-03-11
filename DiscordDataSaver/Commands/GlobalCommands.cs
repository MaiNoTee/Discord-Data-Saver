using Discord;
using Discord.WebSocket;

namespace DiscordDataSaver.Commands;

public class GlobalCommands
{
	public static async Task GlobalReplyCommand(DiscordSocketClient client)
	{
		await client.CreateGlobalApplicationCommandAsync(
			new SlashCommandBuilder()
				.WithName("reply")
				.WithDescription("Reply command in DM")
				.Build());
	}

	public static async Task GlobalSaveCommand(DiscordSocketClient client)
	{
		await client.CreateGlobalApplicationCommandAsync(
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

	public static async Task GlobalDeleteCommand(DiscordSocketClient client)
	{
		await client.CreateGlobalApplicationCommandAsync(
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
	}
	
	public static async Task GlobalPaintMeCommand(DiscordSocketClient client)
	{
		await client.CreateGlobalApplicationCommandAsync(
			new SlashCommandBuilder()
				.WithName("paint")
				.WithDescription("Paint your role")
				.AddOption(
					new SlashCommandOptionBuilder()
						.WithName("color")
						.WithDescription("Choose color")
						.WithRequired(true))
				.Build());
	}
}
