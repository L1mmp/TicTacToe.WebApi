using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Domain.Models;

namespace TicTacToe.Domain.ResponceModels
{
	public class RefreshResponceModel
	{
		public string Jwt { get; set; } = string.Empty;
		public string Message { get; set; } = string.Empty;
		public RefreshToken RefreshToken { get; set; } = new RefreshToken();
	}
}
