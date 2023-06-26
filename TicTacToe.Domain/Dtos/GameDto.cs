namespace TicTacToe.Domain.Dtos
{
	public class GameDto
	{
		public string? Name { get; set; }
		public bool IsActive { get; set; }

		public Guid User1Id { get; set; }
		public Guid User2Id { get; set;}
	}
}