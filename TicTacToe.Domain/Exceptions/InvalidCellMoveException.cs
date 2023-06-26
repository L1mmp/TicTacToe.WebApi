using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Domain.Exceptions
{
	/// <summary>
	/// Throws if player tries to mark used cell.
	/// </summary>
	[Serializable]
	public class InvalidCellMoveException : Exception
	{
		public InvalidCellMoveException() { }

		public InvalidCellMoveException(string message) : base(message) { }

		public InvalidCellMoveException(string message, Exception inner)
		: base(message, inner)
		{
		}
	}
}
