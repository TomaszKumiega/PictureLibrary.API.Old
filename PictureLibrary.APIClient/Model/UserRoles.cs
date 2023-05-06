namespace PictureLibrary.APIClient.Model
{
    [Flags]
    public enum UserRole : long
    {
        Admin = 0,
        Regular = 1,
        Hidden = 2,
    }
}
