using AutoMapper;
using Microsoft.Extensions.Logging;
using TicTacToe.Application.Services.Interfaces;
using TicTacToe.DataAccess.Repos.Interfaces;
using TicTacToe.Domain.Dtos;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Models;

namespace TicTacToe.Infrastructure.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepo;
		private readonly IMapper _mapper;
		private readonly ILogger<IUserService> _logger;

		public UserService(IUserRepository userRepo, IMapper mapper, ILogger<IUserService> logger)
		{
			_userRepo = userRepo;
			_mapper = mapper;
			_logger = logger;
		}
		public async Task<bool> UnRegisterUser(UserDto dto)
		{
			var entity = (await _userRepo.GetByConditionAsync(x => x.UserName == dto.UserName)).FirstOrDefault();

			if (entity == null)
			{
				return false;
			}

			try
			{
				await _userRepo.DeleteByIdAsync(entity!.Id);
			}
			catch (Exception)
			{
				throw;
			}

			return true;
		}

		private async Task<bool> IsUserExists(UserDto dto)
		{
			return (await _userRepo.GetByConditionAsync(x => x.UserName == dto.UserName)).Any();
		}

		public async Task<bool> AddAsync(UserDto dto)
		{
			var isExists = await IsUserExists(dto);

			if (!isExists)
			{
				return false;
			}

			var entity = _mapper.Map<User>(dto);

			return (await _userRepo.AddAsync(entity)).State == Microsoft.EntityFrameworkCore.EntityState.Added;
		}

		public async Task<bool> AddAsync(User entity)
		{
			var user = await _userRepo.GetByIdAsync(entity.Id);

			if (user != null)
			{
				return false;
			}

			return (await _userRepo.AddAsync(entity)).State == Microsoft.EntityFrameworkCore.EntityState.Added;
		}

		public async Task<User> GetUserByLogin(string login)
		{
			try
			{
				var user = (await _userRepo.GetByConditionAsync(x => x.UserName == login)).FirstOrDefault() ?? default(User);
				return user;
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task UpdateRefreshTokenAsync(User user)
		{
			await _userRepo.UpdateAsync(user);
		}

		public async Task<User> GetUserById(Guid userId)
		{
			return await _userRepo.GetByIdAsync(userId);
		}

		public async Task UpdateUserAsync(User user)
		{
			await _userRepo.UpdateAsync(user);
		}

		public async Task UpdateUsersStat(GameSession gameSession)
		{
			if (gameSession.Winner != Guid.Empty)
			{
				var user = await _userRepo.GetByIdAsync(gameSession.Winner);

				user.UserRating += 2;

				await _userRepo.UpdateAsync(user);
			}
			else
			{
				var user = await _userRepo.GetByIdAsync(gameSession.Player1.UserId);
				var secondUser = await _userRepo.GetByIdAsync(gameSession.Player2.UserId);

				user.UserRating += 1;
				secondUser.UserRating += 1;

				await _userRepo.UpdateAsync(user);
				await _userRepo.UpdateAsync(secondUser);
			}

		}
	}
}
