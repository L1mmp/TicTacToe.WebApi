using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Domain.Entities;

namespace TicTacToe.DataAccess.Contexts
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{ }
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Game> Games { get; set; } = null!;
		public DbSet<GameHistory> GameHistories { get; set; } = null!;
	}
}
