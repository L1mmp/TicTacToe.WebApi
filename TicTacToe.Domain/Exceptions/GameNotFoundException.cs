using System.Runtime.Serialization;

namespace TicTacToe.Domain.Exceptions
{
	/// <summary>
	/// Thorws if game not found.
	/// </summary>
	[Serializable]
	public class GameNotFoundException : Exception
	{
		public GameNotFoundException()
		{
		}

		public GameNotFoundException(string? message) : base(message)
		{
		}

		public GameNotFoundException(string? message, Exception? innerException) : base(message, innerException)
		{
		}

		protected GameNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}