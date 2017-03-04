using BargainBot.Model;

namespace BargainBot.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(MyContext context) : base(context)
        {
            
        }
    }

    internal interface IUserRepository
    {
    }
}