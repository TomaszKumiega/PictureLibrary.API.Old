namespace PictureLibrary.Model.Users
{
    [Flags]
    public enum UserRole : long
    {
        Admin = 0,
        Regular = 1,
        Hidden = 2,
    }
}
