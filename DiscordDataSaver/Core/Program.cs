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
	static DiscordSocketClient? _client;
	static ulong _testGuildId;

	public static async Task Main()
	{
		var config = new DiscordSocketConfig { GatewayIntents = GatewayIntents.All };
		_client = new DiscordSocketClient(config);
		_client.MessageReceived += CommandHandler.MessageReceived;
		_client.SlashCommandExecuted += CommandHandler.SlashCommandReceived;
		
		_client.Log += Logger.Log;
		_client.Ready += Ready;

		string token = JsonConvert.DeserializeObject<ConfigStructure>(
			await File.ReadAllTextAsync("config.json"))!.Token;
		_testGuildId = Convert.ToUInt64(
			JsonConvert.DeserializeObject<ConfigStructure>(
				await File.ReadAllTextAsync("config.json"))!.Guild);

		await using (var db = new DatabaseContext())
		{
			User tom = new User { Name = "Tom", RegDate = DateTime.Now, Tag = 1212 };
			if (!db.Entry(tom).IsKeySet)
				await db.AddAsync(
					tom); // проверка на наличие Тома в базе данных. Без неё происходит конфликт из-за правила уникальных ключей.
			await db.SaveChangesAsync();
			Console.WriteLine("Внесены изменения в базу данных");
		}

		await _client.LoginAsync(TokenType.Bot, token);
		await _client.StartAsync();

		Console.ReadKey();
	}

	private static async Task Ready()
	{
		await CommandHandler.ConnectSlashCommands(_client, _testGuildId);
	}
}
