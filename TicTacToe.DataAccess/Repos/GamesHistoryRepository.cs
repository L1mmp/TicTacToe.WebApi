using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.DataAccess.Contexts;
using TicTacToe.DataAccess.Repos.Interfaces;
using TicTacToe.Domain.Entities;

namespace TicTacToe.DataAccess.Repos
{
	public class GamesHistoryRepository : BaseRepository<GameHistory>, IGamesHistoryRepository
	{
		public GamesHistoryRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
		}
	}
}
