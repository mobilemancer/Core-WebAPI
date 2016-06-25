using System.Collections.Concurrent;
using System.Linq;

namespace RouteConstraints.Models
{
    public class UserRepository : IUserRepository
    {

        private ConcurrentDictionary<string, User> users { get; set; }
        private static int id;

        public UserRepository()
        {
            users = new ConcurrentDictionary<string, User>();
            Seed();
        }


        public User GetUserByEmail(string email)
        {
            return users.Values.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserByHandle(string handle)
        {
            return users.Values.FirstOrDefault(u => u.Handle == handle);
        }



        private void Seed()
        {
            var user = new User
            {
                Handle = "mobilemancer",
                Email = "fake@mobilemancer.com",
                FirstName = "Andreas",
                LastName = "Wänqvist",
                Id = id++
            };
            users.TryAdd(user.Handle, user);
        }
    }
}
