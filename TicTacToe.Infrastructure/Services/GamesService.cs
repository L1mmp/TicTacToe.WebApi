using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Application.Services.Interfaces;
using TicTacToe.DataAccess.Repos.Interfaces;
using TicTacToe.Domain.Dtos;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Infrastructure.Services
{
	public class GamesService : IGamesService
	{
		private readonly IGamesRepository _gamesRepository;
		private readonly ILogger<GamesService> _logger;
		private readonly IMapper _mapper;

		public GamesService(IMapper mapper, IGamesRepository gamesRepository, ILogger<GamesService> logger)
		{
			_gamesRepository = gamesRepository;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task CloseGameById(Guid gameId)
		{

			try
			{
				var game = await _gamesRepository.GetByIdAsync(gameId);

				game.IsActive = false;

				await _gamesRepository.UpdateAsync(game);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to close game: {Id}", gameId);
				throw;
			}

		}

		public async ValueTask<Guid> CreateNewGame(GameDto dto)
		{
			var entity = _mapper.Map<Game>(dto);

			try
			{
				var added = await _gamesRepository.AddAsync(entity);

				return added.Entity.Id;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to create a new game: {Id}", entity);
				throw;
			}
		}

		public async ValueTask DeleteGameById(Guid id)
		{
			try
			{
				await _gamesRepository.DeleteByIdAsync(id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to delete game with ID: {Id}", id);
				throw;
			}
		}

		public async Task<IEnumerable<Game>> GetAllActiveGames()
		{
			try
			{
				var entities = await _gamesRepository.GetByConditionAsync(x => x.IsActive);
				return entities;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to get active games");
				throw;
			}

		}

		public async Task<IEnumerable<Game>> GetAllGames()
		{
			try
			{
				var entities = await _gamesRepository.GetAllAsync();
				return entities;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to get games");
				throw;
			}
		}

		public async Task<Game> GetGameById(Guid gameId)
		{
			try
			{
				var entity = await _gamesRepository.GetByIdAsync(gameId);
				return entity;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to get active games");
				throw;
			}
		}

		public async Task JoinSecondUserToGameByIdAsync(Guid guid, Guid userId)
		{
			var game = await _gamesRepository.GetByIdAsync(guid);

			game.User2Id = userId;

			await _gamesRepository.UpdateAsync(game);
		}

		public async ValueTask UpdateGameById(Guid id, GameDto dto)
		{
			try
			{
				var entity = await _gamesRepository.GetByIdAsync(id);

				_mapper.Map(dto, entity);

				await _gamesRepository.UpdateAsync(entity);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to update game with ID: {@id}", id);
				throw;
			}
		}
	}
}
