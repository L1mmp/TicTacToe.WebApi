using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Domain.Entities
{
	public class User
	{
		[Key]
		public Guid Id { get; set; }
		public string? UserName { get; set; }
		public string? Password { get; set; }
		public string? FullName { get; set; }
		public int UserRating { get; set; }
		public string RefreshToken { get; set; }
		public DateTime TokenCreated { get; set; }
		public DateTime TokenExpires { get; set; }
	}
}
