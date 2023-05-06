namespace PictureLibrary.API.Dtos
{
    public class FindUsersDto
    {
        public required IEnumerable<GetUserDto> Users { get; set; }
    }
}
