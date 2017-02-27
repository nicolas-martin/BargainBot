using BargainBot.Model;

namespace BargainBot.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository()
        {
            
        }
    }

    internal interface IUserRepository
    {
    }
}