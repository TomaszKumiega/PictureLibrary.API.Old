using AutoMapper;
using PictureLibrary.Api.Dtos;
using PictureLibrary.Model;

namespace PictureLibrary.API.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NewLibraryDto, Library>();
        }
    }
}
