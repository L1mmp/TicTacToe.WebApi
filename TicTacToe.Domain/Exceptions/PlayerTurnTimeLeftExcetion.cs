using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Domain.Exceptions
{
	/// <summary>
	/// Thorws if current player turn time left.
	/// </summary>
	[Serializable]
	public class PlayerTurnTimeLeftExcetion : Exception
	{
		public PlayerTurnTimeLeftExcetion() { }

		public PlayerTurnTimeLeftExcetion(string message) : base(message) { }
		
		public PlayerTurnTimeLeftExcetion(string message, Exception inner)
		: base(message, inner)
		{
		}
	}
}
