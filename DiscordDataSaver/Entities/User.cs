using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordDataSaver.Entities;

[Table("Users")]
public class User
{
	public string? Name { get; set; }
	public DateTime RegDate { get; set; }
	[Key] public int Tag { get; set; }
}
