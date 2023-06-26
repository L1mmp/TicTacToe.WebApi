using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Domain.Dtos
{
	public class GameSessionDto
	{
		public string GameId { get; set; }
		public PlayerDto Player1 { get; set; }
		public PlayerDto Player2 { get; set; }
		public string CurrentPlayerId { get; set; }
		public string[] Board { get; set; }
		public bool IsGameFinished { get; set; }
		public bool IsFirstMove { get; set; }
		public string Winner { get; set; }
		public DateTime LastTurnDate { get; set; }
	}

}
