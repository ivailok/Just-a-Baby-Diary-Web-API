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
            var foundUser = this.usersCollecion.AsQueryable().Single(u => u.Username == user.Username);

            if (foundUser == null)
            {
                var result = this.usersCollecion.Insert<User>(user);
            }
            else
            {
                throw new ArgumentException("Username is already taken.");
            }
        }
    }
}