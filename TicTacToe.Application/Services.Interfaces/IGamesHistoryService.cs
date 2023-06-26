using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Models;

namespace TicTacToe.Application.Services.Interfaces
{
	public interface IGamesHistoryService
	{
		Task AddGameHistory(GameSession gameSession, string JsonTurns);
	}
}
