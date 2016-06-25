namespace RouteConstraints.Models
{
    public interface IUserRepository
    {
        User GetUserByHandle(string handle);
        User GetUserByEmail(string email);
    }
}
