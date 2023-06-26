using System.Text.Json.Serialization;

namespace TicTacToe.Domain.Dtos
{
	public class LoginDto
	{
		[JsonPropertyName("login")]
		public string Login { get; set; } = null!;

		[JsonPropertyName("password")]
		public string Password { get; set; } = null!;
	}
}