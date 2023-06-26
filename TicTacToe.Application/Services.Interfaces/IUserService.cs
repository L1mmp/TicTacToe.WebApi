using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Domain.Dtos;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Models;

namespace TicTacToe.Application.Services.Interfaces
{
	public interface IUserService
	{
		public Task<bool> AddAsync(UserDto dto);
		public Task<bool> AddAsync(User entity);
		public Task<User> GetUserById(Guid userId);
		public Task<User> GetUserByLogin(string login);
		public Task<bool> UnRegisterUser(UserDto dto);
		public Task UpdateRefreshTokenAsync(User user);
		public Task UpdateUserAsync(User user);
		Task UpdateUsersStat(GameSession gameSession);
	}
}
