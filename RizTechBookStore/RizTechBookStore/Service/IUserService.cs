namespace RizTechBookStore.Service
{
    public interface IUserService
    {
        string GetUserId();
        bool UserLoginOrNot();
    }
}