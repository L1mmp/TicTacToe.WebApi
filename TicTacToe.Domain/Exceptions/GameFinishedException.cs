using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Domain.Exceptions
{
	/// <summary>
	/// Thorws if try to manipulate on finished game.
	/// </summary>
	[Serializable]
	public class GameFinishedException : Exception
	{
		public GameFinishedException() { }

		public GameFinishedException(string message) : base(message) { }

		public GameFinishedException(string message, Exception inner)
		: base(message, inner)
		{
		}
	}
}
