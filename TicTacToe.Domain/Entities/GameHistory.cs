using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicTacToe.Domain.Models;

namespace TicTacToe.Domain.Entities
{
	public class GameHistory
	{
		public GameHistory() { }
        public GameHistory(GameSession gameSession, string turnJson)
		{
			GameId = gameSession.GameId;
			TurnsHistory = turnJson;
		}

		[Key]
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; } = null!;
		public string TurnsHistory { get; set; } = string.Empty;
    }
}
