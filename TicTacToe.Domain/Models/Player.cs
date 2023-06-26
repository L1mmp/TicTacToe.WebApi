using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe.Domain.Models
{
	public class Player
	{
		public Guid UserId { get; set; }
		public char Side { get; set; }
    }
}
