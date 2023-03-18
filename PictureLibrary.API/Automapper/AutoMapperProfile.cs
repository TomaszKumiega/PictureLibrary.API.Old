using AutoMapper;
using PictureLibrary.API.Dtos;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.Model;

namespace PictureLibrary.API.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LibraryDto, Library>();
            CreateMap<UserRegisterDto, AddUserCommand>();
        }
    }
}
