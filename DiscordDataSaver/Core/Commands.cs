using Discord;

namespace DiscordDataSaver.Core;

public static class Commands
{
	public static ApplicationCommandProperties ReplyCommandProperties()
	{
		return new SlashCommandBuilder()
			.WithName("reply")
			.WithDescription("Reply command in DM").Build();
	}

	public static ApplicationCommandProperties SaveCommandProperties()
	{
		return new SlashCommandBuilder()
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
			.Build();
	}

	public static ApplicationCommandProperties DeleteCommandProperties()
	{
		return new SlashCommandBuilder()
			.WithName("delete")
			.WithDescription("Delete number of messages")
			.AddOption(
				new SlashCommandOptionBuilder()
					.WithName("count")
					.WithDescription("Number of messages to delete")
					.WithRequired(true)
					.WithType(ApplicationCommandOptionType.Number)).Build();
	}

	public static ApplicationCommandProperties PaintMeCommandProperties()
	{
		return new SlashCommandBuilder()
			.WithName("paint-me")
			.WithDescription("Paint your role")
			.AddOption(
				new SlashCommandOptionBuilder()
					.WithName("color")
					.WithDescription("Choose color")
					.WithRequired(true)
					.WithType(ApplicationCommandOptionType.String))
			.Build();
	}
}
