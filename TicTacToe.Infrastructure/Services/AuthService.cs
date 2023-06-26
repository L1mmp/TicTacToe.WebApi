using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Application.Services.Interfaces;
using TicTacToe.Domain.Dtos;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Models;
using TicTacToe.Domain.ResponceModels;
using TicTacToe.Infrastructure.Providers;

namespace TicTacToe.Infrastructure.Services
{
	public class AuthService : IAuthService
	{
		private readonly IMapper _mapper;
		private readonly TokenProvider _tokenProvider;
		private readonly IUserService _userService;
		private readonly IHttpContextAccessor _httpContext;
		private readonly ILogger<AuthService> _logger;

		public AuthService(IUserService userService, TokenProvider tokenProvider, IMapper mapper, IHttpContextAccessor httpContext, ILogger<AuthService> logger)
		{
			_userService = userService;
			_tokenProvider = tokenProvider;
			_mapper = mapper;
			_httpContext = httpContext;
			_logger = logger;
		}

		public async Task<RefreshToken> GetUserRefreshToken()
		{
			var login = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Name)!.Value;

			try
			{
				var user = await _userService.GetUserByLogin(login);

				if(user is not default(User))
				{
					return new RefreshToken()
					{
						Token = user.RefreshToken,
						Created = user.TokenCreated,
						Expires = user.TokenExpires
					};
				}

				throw new NullReferenceException($"User with login {login} not found");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to get user with login: {login}", login);
				throw;
			}
		}

		public async Task<LoginResponceModel> Login(LoginDto dto)
		{
			var user = await _userService.GetUserByLogin(dto.Login);

			var responce = new LoginResponceModel
			{
				Message = "Login or password incorrect",
				IsSuccessful = false
			};

			if (user is default(User))
			{
				return responce;
			}

			var verified = VerifyPassword(dto.Password, user);

			if (verified)
			{
				var refresh = _tokenProvider.CreateRefreshToken();
				responce.IsSuccessful = true;
				responce.Message = "Login successful";
				responce.Token = _tokenProvider.CreateToken(user);
				responce.RefreshToken = refresh;


				user.RefreshToken = refresh.Token;
				user.TokenCreated = refresh.Created;
				user.TokenExpires = refresh.Expires;
			}

			await _userService.UpdateRefreshTokenAsync(user);

			return responce;
		}

		public async Task<RefreshResponceModel> RefreshToken()
		{
			var login = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Name).Value;
			var user = await _userService.GetUserByLogin(login);

			if( user is not default(User))
			{

				var token = new RefreshToken()
				{
					Token = user.RefreshToken,
					Created = user.TokenCreated,
					Expires = user.TokenExpires
				};

				var refreshToken = _httpContext.HttpContext.Request.Cookies["refreshToken"];

				var responce = new RefreshResponceModel()
				{
					Message = "Can't refresh token"
				};

				if (!token.Token.Equals(refreshToken))
				{
					responce.Message = "Invalid Refresh Token";
					return responce;
				}

				if (token.Expires <= DateTime.UtcNow)
				{
					responce.Message = "Token expired";
					return responce;
				}

				var newJwt = _tokenProvider.CreateToken(user);
				var newRefreshToken = _tokenProvider.CreateRefreshToken();


				responce.Message = "Token refreshed successfully";
				responce.Jwt = newJwt;
				responce.RefreshToken = newRefreshToken;
				return responce;
			}

			throw new NullReferenceException($"User with login {login} not found");
		}

		public async Task<RefreshToken> Register(UserDto dto)
		{
			var refreshToken = _tokenProvider.CreateRefreshToken();
			var user = _mapper.Map<User>(dto);

			user.Password = HashPassword(user.Password);

			user.RefreshToken = refreshToken.Token;
			user.TokenCreated = refreshToken.Created;
			user.TokenExpires = refreshToken.Expires;

			await _userService.AddAsync(user);

			return refreshToken;
		}

		private bool VerifyPassword(string password, User? user)
		{
			return BCrypt.Net.BCrypt.Verify(password, user.Password);
		}
		private static string HashPassword(string password)
		{
			var pass = BCrypt.Net.BCrypt.HashPassword(password);
			return pass;
		}
	}
}
