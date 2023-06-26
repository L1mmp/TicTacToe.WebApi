using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicTacToe.Domain.Entities
{
	public class Game
	{
		[Key]
        public Guid Id { get; set; }
		public string? Name { get; set; }
		public bool IsActive { get; set; }
		public Guid User1Id { get; set; }
		public Guid User2Id { get; set; }
	}
}