using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Domain.Exceptions
{
	/// <summary>
	/// Throws if not current player turn.
	/// </summary>
	[Serializable]
	public class NotCurrentPlayerTurnException : Exception
	{
		public NotCurrentPlayerTurnException() { }

		public NotCurrentPlayerTurnException(string message) : base(message) { }

		public NotCurrentPlayerTurnException(string message, Exception inner)
		: base(message, inner)
		{
		}
	}
}
