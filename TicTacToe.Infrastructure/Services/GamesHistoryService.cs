using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Application.Services.Interfaces;
using TicTacToe.DataAccess.Repos;
using TicTacToe.DataAccess.Repos.Interfaces;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Models;

namespace TicTacToe.Infrastructure.Services
{
	public class GamesHistoryService : IGamesHistoryService
	{
		private readonly IGamesHistoryRepository _repository;

		public GamesHistoryService(IGamesHistoryRepository repository)
		{
			_repository = repository;
		}

		public async Task AddGameHistory(GameSession gameSession, string turnsJson)
		{
			var history = new GameHistory(gameSession, turnsJson);

			await _repository.AddAsync(history);
		}
	}
}
