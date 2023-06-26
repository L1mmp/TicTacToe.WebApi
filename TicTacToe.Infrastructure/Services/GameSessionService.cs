using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Application.Services.Interfaces;
using TicTacToe.Domain.Dtos;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Exceptions;
using TicTacToe.Domain.Models;

namespace TicTacToe.Infrastructure.Services
{
	public class GameSessionService : IGameSessionService
	{
		private ConcurrentDictionary<Guid, GameSession> _games = new();

        public GameSession CreateNewGame(GameDto gameDto, Guid createdGameGuid)
		{
			var player1 = new Player()
			{
				Side = 'X',
				UserId = gameDto.User1Id
			};
			var player2 = new Player()
			{
				Side = 'O'
			};

			var gameSession = new GameSession(createdGameGuid, player1, player2);

			_games[gameSession.GameId]= gameSession;

			return gameSession;
		}

		public GameSession JoinGame(Guid gameId, User user)
		{
			if(!_games.TryGetValue(gameId, out var gameSession))
			{
				throw new GameNotFoundException($"Game with id not found{gameId}");
			}

			var player2 = new Player()
			{
				UserId = user.Id,
				Side = gameSession.Player1.Side == 'X' ? 'O' : 'X'
			};

			gameSession.Player2 = player2;

			_games[gameId] = gameSession;

			return gameSession;
		}

		public GameSession MakeMove(Guid gameId, MoveRequest move, Guid playerId)
		{
			if(!_games.TryGetValue(gameId, out var gameSession))
			{
				throw new GameNotFoundException($"Game with id not found{gameId}");
			}

			gameSession!.MakeMove(move.Row, move.Column, playerId);

			return gameSession;
		}

		public void RemoveSession(Guid gameId)
		{
			_games.TryRemove(gameId, out _);
		}
	}
}
