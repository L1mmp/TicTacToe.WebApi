using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Domain.Dtos;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Application.Services.Interfaces
{
	public interface IGamesService
	{
		ValueTask<Guid> CreateNewGame(GameDto dto);
		ValueTask DeleteGameById(Guid id);
		Task<IEnumerable<Game>> GetAllGames();
		Task<IEnumerable<Game>> GetAllActiveGames();
		ValueTask UpdateGameById(Guid id, GameDto dto);
		Task<Game> GetGameById(Guid gameId);
		Task CloseGameById(Guid gameId);
		Task JoinSecondUserToGameByIdAsync(Guid guid, Guid userId);
	}
}
