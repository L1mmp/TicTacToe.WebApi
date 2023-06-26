using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.Domain.Dtos;
using TicTacToe.Domain.Entities;

namespace TicTacToe.Domain.Mappings
{
	public class DtoToEntitesProfile : Profile
	{
		public DtoToEntitesProfile()
		{
			CreateMap<User, UserDto>().ReverseMap();
			CreateMap<Game, GameDto>().ReverseMap();
		}
	}
}
