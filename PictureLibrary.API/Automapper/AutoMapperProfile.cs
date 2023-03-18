using AutoMapper;
using PictureLibrary.Api.Dtos;
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
