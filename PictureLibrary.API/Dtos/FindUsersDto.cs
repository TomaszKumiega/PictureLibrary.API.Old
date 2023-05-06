namespace PictureLibrary.API.Dtos
{
    public class FindUsersDto
    {
        public required IEnumerable<UserDto> Users { get; set; }
    }
}
