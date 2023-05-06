using AutoMapper;
using PictureLibrary.API.Dtos;
using PictureLibrary.DataAccess.Commands;
using PictureLibrary.DataAccess.Queries;
using PictureLibrary.Model;

namespace PictureLibrary.API.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LibraryDto, Library>();
            CreateMap<UserRegisterDto, AddUserCommand>();
            CreateMap<RefreshTokensDto, RefreshTokensQuery>();
            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Id, x => x.Ignore())
                .ForMember(dest => dest.PasswordHash, x => x.MapFrom((dto) => Array.Empty<byte>()))
                .ForMember(dest => dest.PasswordSalt, x => x.MapFrom((dto) => Array.Empty<byte>()));
            CreateMap<User, GetUserDto>();
        }
    }
}
