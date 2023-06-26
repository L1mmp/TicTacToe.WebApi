using Microsoft.EntityFrameworkCore;
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
	public class GamesRepository : BaseRepository<Game>, IGamesRepository
	{
		public GamesRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
		}
	}
}
