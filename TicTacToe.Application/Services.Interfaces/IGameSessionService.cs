using TicTacToe.Domain.Dtos;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Models;

namespace TicTacToe.Application.Services.Interfaces
{
	public interface IGameSessionService
	{
		GameSession CreateNewGame(GameDto gameDto, Guid createdGameGuid);
		GameSession JoinGame(Guid gameId, User user);
		GameSession MakeMove(Guid gameId, MoveRequest move, Guid playerId);
		void RemoveSession(Guid gameId);
	}
}