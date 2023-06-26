using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Exceptions;

namespace TicTacToe.Domain.Models
{
	public class GameSession
	{
		private bool _isFirstMove;
		private int _turnOrder = 1;
		public List<Turn> Turns = new();

		public Player Player1 { get; set; }
		public Player Player2 { get; set; }
		public Guid GameId { get; private set; }
		public Guid CurrentPlayerId { get; private set; }
		public char[,]? Board { get; private set; }
		public bool IsGameFinished { get; private set; }
		public Guid Winner { get; private set; }
		public DateTime LastTurnDate { get; private set; }


		public GameSession(
			Guid gameId,
			Player player1,
			Player player2)
		{
			Player1 = player1;
			Player2 = player2;
			GameId = gameId;
			Board = new char[3, 3];
			IsGameFinished = false;
			GameId = gameId;
			CurrentPlayerId = player1.UserId;
			_isFirstMove = true;
		}

		public void MakeMove(int row, int column, Guid playerId)
		{
			ValidateMove(row, column, playerId);

			if (_isFirstMove)
			{
				_isFirstMove = false;
			}


			Board![row, column] = CurrentPlayerId == Player1.UserId ? 'X' : 'O';

			var turn = new Turn()
			{
				Column = column,
				Row = row,
				PlayerId = playerId,
				Order = _turnOrder
			};

			Turns.Add(turn);

			IncreaseTurnOrder(1);

			if (IsPlayerWinning(Player1))
			{
				IsGameFinished = true;
				Winner = CurrentPlayerId;
			}
			else if (IsBoardFull())
			{
				IsGameFinished = true;
			}

			CurrentPlayerId =
				CurrentPlayerId == Player1.UserId
				? Player2.UserId
				: Player1.UserId;

			LastTurnDate = DateTime.UtcNow;
		}

		private void ValidateMove(int row, int column, Guid playerId)
		{
			if (!_isFirstMove && DateTime.UtcNow - LastTurnDate > TimeSpan.FromSeconds(15))
			{
				throw new PlayerTurnTimeLeftExcetion("Your time for turn left. You lost");
			}

			if (CurrentPlayerId != playerId)
			{
				throw new NotCurrentPlayerTurnException("Its not your turn. Wait until oppenent make move");
			}

			if (IsGameFinished)
			{
				throw new GameFinishedException("Game is already finished.");
			}

			if (Board![row, column] != '\0')
			{
				throw new PlayerTurnTimeLeftExcetion("Invalid move. Cell is already occupied.");
			}
		}

		private bool IsPlayerWinning(Player player)
		{
			// Check rows
			for (int row = 0; row < 3; row++)
			{
				if (Board![row, 0] == Board[row, 1] && Board[row, 1] == Board[row, 2] && Board[row, 0] == player.Side)
				{
					return true;
				}
			}

			// Check columns
			for (int col = 0; col < 3; col++)
			{
				if (Board![0, col] == Board[1, col] && Board[1, col] == Board[2, col] && Board[0, col] == player.Side)
				{
					return true;
				}
			}

			// Check diagonals
			if ((Board![0, 0] == Board[1, 1] && Board[1, 1] == Board[2, 2] && Board[0, 0] == player.Side) ||
				(Board[0, 2] == Board[1, 1] && Board[1, 1] == Board[2, 0] && Board[0, 2] == player.Side))
			{
				return true;
			}

			return false;
		}

		private bool IsBoardFull()
		{
			for (int row = 0; row < 3; row++)
			{
				for (int col = 0; col < 3; col++)
				{
					if (Board[row, col] == '\0')
					{
						return false;
					}
				}
			}
			return true;
		}


		private void IncreaseTurnOrder(int value)
		{
			Interlocked.Add(ref _turnOrder, value);
		}
	}
}