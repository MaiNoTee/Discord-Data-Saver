using Discord;
using Discord.WebSocket;

namespace DiscordDataSaver.Commands;

public class Commands
{
	public static async Task GuildReplyCommand(SocketGuild guild)
	{
		await guild.CreateApplicationCommandAsync(
			new SlashCommandBuilder()
				.WithName("reply")
				.WithDescription("Reply command in DM")
				.Build());
	}

	public static async Task GuildSaveCommand(SocketGuild guild)
	{
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

	public static async Task GuildDeleteCommand(SocketGuild guild)
	{
		await guild.CreateApplicationCommandAsync(
			new SlashCommandBuilder()
				.WithName("delete")
				.WithDescription("Delete number of messages")
				.AddOption(
					new SlashCommandOptionBuilder()
						.WithName("count")
						.WithDescription("Number of messages to delete")
						.WithRequired(true)
						.WithType(ApplicationCommandOptionType.Number))
				.Build());
	}

	public static async Task GuildPaintMeCommand(SocketGuild guild)
	{
		await guild.CreateApplicationCommandAsync(
			new SlashCommandBuilder()
				.WithName("paint-me")
				.WithDescription("Paint your role")
				.AddOption(
					new SlashCommandOptionBuilder()
						.WithName("color")
						.WithDescription("Choose color")
						.WithRequired(true)
						.WithType(ApplicationCommandOptionType.String))
				.Build());
	}
}
