using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Application.Services.Interfaces;
using TicTacToe.Domain.Dtos;
using TicTacToe.Domain.Models;
using TicTacToe.Domain.ResponceModels;

namespace TicTacToe.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : Controller
	{
		private readonly IAuthService _authService;
		public AuthController(IAuthService authService, IHttpContextAccessor httpContext)
		{
			_authService = authService;
		}

		/// <summary>
		/// Register new user.
		/// </summary>
		/// <param name="userDto"> UserName data. </param>
		/// <returns> 200 if registration was successful. 400 if registration wasn't successful. </returns>
		[HttpPost("register")]
		[AllowAnonymous]
		public async Task<ActionResult> Register(UserDto userDto)
		{
			try
			{
				var refreshToken = await _authService.Register(userDto);
				SetRefreshToken(refreshToken);

				return Ok("Registration successful!");
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}

		}

		/// <summary>
		/// Tries to login user.
		/// </summary>
		/// <param name="loginDto"> UserDTO. </param>
		/// <returns> Login responce model. </returns>
		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<ActionResult<LoginResponceModel>> Login([FromBody] LoginDto loginDto)
		{
			try
			{
				var responce = await _authService.Login(loginDto);
				SetRefreshToken(responce.RefreshToken);

				return Ok(responce);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}

		}

		/// <summary>
		/// Generate and set Refresh token.
		/// </summary>
		/// <returns> Refresh token responce model. </returns>
		[HttpPost("refresh-token"), Authorize]
		public async Task<ActionResult<string>> RefreshToken()
		{
			try
			{
				var responce = await _authService.RefreshToken();

				SetRefreshToken(responce.RefreshToken);

				return Ok(responce);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}

		}

		/// <summary>
		/// Set refresh token to cookie.
		/// </summary>
		/// <param name="token"> Token to set. </param>
		private void SetRefreshToken(RefreshToken token)
		{
			var cookie = new CookieOptions()
			{
				HttpOnly = true,
				Expires = token.Expires,
			};

			Response.Cookies.Append("refreshToken", token.Token, cookie);
		}
	}
}
