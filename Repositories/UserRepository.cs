using System;
using System.Collections.Generic;
using BargainBot.Model;

namespace BargainBot.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User Create(User obj)
        {
            return obj;
        }

        public User Get(Guid id)
        {
            return new User();
        }

        public User Update(User obj)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<User> Get()
        {
            throw new NotImplementedException();
        }
    }

    public interface IUserRepository : IRepository<User>
    {
    }
}