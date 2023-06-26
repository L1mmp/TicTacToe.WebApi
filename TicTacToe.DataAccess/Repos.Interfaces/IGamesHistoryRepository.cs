using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Domain.Entities;

namespace TicTacToe.DataAccess.Repos.Interfaces
{
	public interface IGamesHistoryRepository : IBaseRepository<GameHistory>
	{
	}
}
