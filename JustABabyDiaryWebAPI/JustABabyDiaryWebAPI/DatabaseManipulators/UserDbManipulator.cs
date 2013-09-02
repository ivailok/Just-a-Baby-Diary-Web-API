using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustABabyDiaryWebAPI.Models;
using JustABabyDiaryWebAPI.Models.ControllerModels;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Text;

namespace JustABabyDiaryWebAPI.DatabaseManipulators
{
    public class UserDbManipulator
    {
        private const string SessionKeyChars =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM";
        private const int SessionKeyLength = 50;
        private static readonly Random rand = new Random();

        private MongoDatabase db;
        private MongoCollection<User> usersCollection;

        public UserDbManipulator()
        {
            DatabaseProviders.DatabaseProvider provider = new DatabaseProviders.DatabaseProvider();
            this.db = provider.GetMongoDatabase();
            this.usersCollection = db.GetCollection<User>("usersInfo");
        }

        private string GenerateSessionKey(string userName)
        {
            StringBuilder skeyBuilder = new StringBuilder(SessionKeyLength);
            skeyBuilder.Append(userName);
            while (skeyBuilder.Length < SessionKeyLength)
            {
                var index = rand.Next(SessionKeyChars.Length);
                skeyBuilder.Append(SessionKeyChars[index]);
            }
            return skeyBuilder.ToString();
        }

        public User Register(User user)
        {
            var foundUserByUsername = this.GetUserByUsername(user.Username);

            if (foundUserByUsername == null)
            {
                var foundUserByEmail = this.GetUserByEmail(user.Email);
                if (foundUserByEmail == null)
                {
                    user.SessionKey = this.GenerateSessionKey(user.Username);
                    this.usersCollection.Insert<User>(user);

                    this.db.CreateCollection("user" + user.Id.ToString());

                    return user;
                }
                throw new ArgumentException("Email is already taken.");
            }
            else
            {
                throw new ArgumentException("Username is already taken.");
            }
        }

        public void Logout(string sessionKey)
        {
            var foundUserBySessionKey = this.GetUserBySessionKey(sessionKey);

            if (foundUserBySessionKey == null)
            {
                throw new ArgumentException("Session has expired.");
            }

            var query = new QueryDocument {
                { "Username", foundUserBySessionKey.Username },
            };
            var update = new UpdateDocument {
                { "$set", new BsonDocument("SessionKey", "") }
            };
            this.usersCollection.Update(query, update);
        }

        public User Login(UserLoginModel model)
        {
            User user = this.GetUserByUsername(model.Username);
            
            if (user == null)
            {
                throw new ArgumentException("Invalid username.");
            }

            if (user.AuthCode != model.AuthCode)
            {
                throw new ArgumentException("Invalid password.");
            }

            user.SessionKey = this.GenerateSessionKey(model.Username);

            var query = new QueryDocument {
                { "Username", user.Username },
            };
            var update = new UpdateDocument {
                { "$set", new BsonDocument("SessionKey", user.SessionKey) }
            };
            this.usersCollection.Update(query, update);

            return user;
        }

        private User GetUserBySessionKey(string sessionKey)
        {
            return this.usersCollection.AsQueryable()
                .Where(u => u.SessionKey == sessionKey)
                .Select(u => u).FirstOrDefault();
        }

        private User GetUserByUsername(string username)
        {
            return this.usersCollection.AsQueryable()
                .Where(u => u.Username == username)
                .Select(u => u).FirstOrDefault();
        }

        private User GetUserByEmail(string email)
        {
            return this.usersCollection.AsQueryable()
                .Where(u => u.Email == email)
                .Select(u => u).FirstOrDefault();
        }
    }
}