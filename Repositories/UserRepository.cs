using System;
using BargainBot.Model;

namespace BargainBot.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User Create(User obj)
        {
            return obj;
        }

        public User Retreive(Guid id)
        {
            throw new NotImplementedException();
        }

        public User Update(User obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }

    public interface IUserRepository : IRepository<User>
    {
    }
}