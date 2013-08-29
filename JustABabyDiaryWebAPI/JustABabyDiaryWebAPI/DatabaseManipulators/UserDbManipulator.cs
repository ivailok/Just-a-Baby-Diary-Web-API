using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustABabyDiaryWebAPI.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace JustABabyDiaryWebAPI.DatabaseManipulators
{
    public class UserDbManipulator
    {
        private MongoCollection<User> usersCollecion;

        public UserDbManipulator()
        {
            DatabaseProviders.DatabaseProvider provider = new DatabaseProviders.DatabaseProvider();
            MongoDatabase db = provider.GetMongoDatabase();
            this.usersCollecion = db.GetCollection<User>("usersInfo");
        }

        public User Register(User user)
        {
            var foundUser = this.GetUser(user);

            if (foundUser == null)
            {
                this.usersCollecion.Insert<User>(user);
                return user;
            }
            else
            {
                throw new ArgumentException("Username is already taken.");
            }
        }

        private User GetUser(User user)
        {
            return this.usersCollecion.AsQueryable()
                .Where(u => u.Username == user.Username)
                .Select(u => u).FirstOrDefault();
        }
    }
}