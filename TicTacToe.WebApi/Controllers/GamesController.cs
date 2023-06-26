using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TicTacToe.Application.Services.Interfaces;
using TicTacToe.Domain.Dtos;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Models;
using TicTacToe.Infrastructure.Services;
using System.Timers;
using System.Threading;
using AutoMapper;
using TicTacToe.Domain.Exceptions;
using Newtonsoft.Json;

namespace TicTacToe.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GamesController : ControllerBase
	{
		private readonly IGamesService _gamesService;
		private readonly ILogger<GamesController> _logger;
		private readonly IGameSessionService _gameSessionService;
		private readonly IGamesHistoryService _gameHistoryService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public GamesController(IGamesService gamesService, ILogger<GamesController> logger, IGameSessionService gameSessionService, IUserService userService, IMapper mapper, IGamesHistoryService gameHistoryService)
		{
			_gamesService = gamesService;
			_logger = logger;
			_gameSessionService = gameSessionService;
			_userService = userService;
			_mapper = mapper;
			_gameHistoryService = gameHistoryService;
		}

		[HttpPost("create")]
		[Authorize]
		public async Task<IActionResult> CreateGame([FromBody] GameDto gameDto)
		{
			try
			{
				gameDto.User1Id = Guid.Parse(HttpContext.User.FindFirst(x => x.Type == "Id")?.Value ?? "");

				var createdGameId = await _gamesService.CreateNewGame(gameDto);

				var gameSession = _gameSessionService.CreateNewGame(gameDto, createdGameId);
				var mapped = _mapper.Map<GameSessionDto>(gameSession);

				return StatusCode(201, mapped);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to create a new game.");
				return StatusCode(500, ex);
			}


		}

		[HttpGet("games")]
		[Authorize]
		public async Task<IActionResult> GetAllActiveGames()
		{
			var games = await _gamesService.GetAllActiveGames();

			return Ok(games);
		}

		[HttpPost("join")]
		[Authorize]
		public async Task<IActionResult> JoinGame([FromQuery] string gameId)
		{
			var userId = Guid.Parse(HttpContext.User.FindFirst(x => x.Type == "Id")?.Value ?? "");
			var guid = Guid.Parse(gameId);
			var user = await _userService.GetUserById(userId);

			if (user == null)
			{
				return BadRequest($"User with id: {userId} not found");
			}

			try
			{
				await _gamesService.JoinSecondUserToGameByIdAsync(guid, user.Id);
				var gameSession = _gameSessionService.JoinGame(guid, user);

				return Ok(_mapper.Map<GameSessionDto>(gameSession));
			}
			catch (GameNotFoundException ex)
			{
				return NotFound(ex);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex);
			}
		}

		[HttpPost("move")]
		[Authorize]
		public async Task<ActionResult<GameSession>> MakeMove(Guid gameId, MoveRequest move)
		{
			var playerId = Guid.Parse(HttpContext.User.FindFirst(x => x.Type == "Id")?.Value ?? "");

			try
			{
				var gameSession = _gameSessionService.MakeMove(gameId, move, playerId);


				if (gameSession.IsGameFinished)
				{
					await _userService.UpdateUsersStat(gameSession);
					await _gamesService.CloseGameById(gameSession.GameId);
					_gameSessionService.RemoveSession(gameSession.GameId);

					var turnsOfGame = JsonConvert.SerializeObject(gameSession.Turns);
					await _gameHistoryService.AddGameHistory(gameSession, turnsOfGame);
				}


				return Ok(_mapper.Map<GameSessionDto>(gameSession));
			}
			catch (GameNotFoundException ex)
			{
				_logger.LogError(ex, "Game with Id: {gameId} not found", gameId);

				return NotFound(ex);
			}
			catch (PlayerTurnTimeLeftExcetion ex)
			{
				return BadRequest(ex);
			}
			catch (GameFinishedException ex)
			{
				return BadRequest(ex);
			}
			catch (InvalidCellMoveException ex)
			{
				return BadRequest(ex);
			}
			catch (NotCurrentPlayerTurnException ex)
			{
				return BadRequest(ex);
			}
		}
	}
}
