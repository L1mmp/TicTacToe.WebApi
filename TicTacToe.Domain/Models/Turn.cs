namespace TicTacToe.Domain.Models
{
	public class Turn
	{
		public Guid PlayerId { get; set; }
		public int Row { get; set; }
		public int Column { get; set; }
		public int Order { get; set; }
	}
}