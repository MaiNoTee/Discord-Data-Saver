using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordDataSaver.Entities;

[Table("Servers")]
public class Server
{
	[Key] public string Id { get; set; }
	public string Name { get; set; }
	//public string[] Users { get; set; }
}
