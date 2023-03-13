using Discord;
using Discord.WebSocket;
using DiscordDataSaver.Entities;
using DiscordDataSaver.Services.Database;
using DiscordDataSaver.Structures;
using DiscordDataSaver.Utils;
using Newtonsoft.Json;

namespace DiscordDataSaver.Core;

public static class Program
{
	public static DiscordSocketClient Client;
	static ulong _testGuildId;
	static string? _token = string.Empty;

	public static async Task Main()
	{
		var config = new DiscordSocketConfig { GatewayIntents = GatewayIntents.All };
		Client = new DiscordSocketClient(config);
		Client.MessageReceived += MainCommandHandler.MessageReceived;
		Client.SlashCommandExecuted += MainCommandHandler.SlashCommandReceived;

		Client.Log += Logger.Log;
		Client.Ready += Ready;

		try
		{
			_token = JsonConvert.DeserializeObject<ConfigStructure>(
				await File.ReadAllTextAsync("Configs/config.json"))!.Token;
			_testGuildId = Convert.ToUInt64(
				JsonConvert.DeserializeObject<ConfigStructure>(
					await File.ReadAllTextAsync("Configs/config.json"))!.Guild);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}

		// Test adding user to db
		await using (var db = new DatabaseContext())
		{
			User tom = new User { Name = "Tom", RegDate = DateTime.Now, Tag = 1212 };
			if (!db.Entry(tom).IsKeySet)
				await db.AddAsync(
					tom); // проверка на наличие Тома в базе данных. Без неё происходит конфликт из-за правила уникальных ключей.
			await db.SaveChangesAsync();
			Console.WriteLine("Внесены изменения в базу данных");
		}

		if (_token == "" || _testGuildId == 0) Console.WriteLine("Set Token or GuildId. Check config.json");
		else
		{
			await Client.LoginAsync(TokenType.Bot, _token);
			await Client.StartAsync();
		}

		Console.ReadKey();
	}

	static async Task Ready()
	{
		var commandHandler = new MainCommandHandler(Client, _testGuildId);
		await commandHandler.ConnectSlashCommands();
	}
}
