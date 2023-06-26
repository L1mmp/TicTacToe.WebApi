using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Domain.Dtos;
using TicTacToe.Domain.Entities;
using TicTacToe.Domain.Models;

namespace TicTacToe.Domain.Mappings
{
	public class DtoToModelsProfile : Profile
	{
		public DtoToModelsProfile()
		{
			CreateMap<Player, PlayerDto>().ReverseMap();
			CreateMap<GameSession, GameSessionDto>()
				.ForMember(dest => dest.Board, opt => opt.MapFrom(src => FlattenBoard(src.Board))); ;
		}

		private string[] FlattenBoard(char[,] board)
		{
			int rows = board.GetLength(0);
			int columns = board.GetLength(1);
			string[] flattenedBoard = new string[rows * columns];

			int index = 0;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					flattenedBoard[index] = board[i, j].ToString();
					index++;
				}
			}

			return flattenedBoard;
		}
	}
}
