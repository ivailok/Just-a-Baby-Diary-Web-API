using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustABabyDiaryWebAPI.Models;
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

        private MongoCollection<User> usersCollecion;

        public UserDbManipulator()
        {
            DatabaseProviders.DatabaseProvider provider = new DatabaseProviders.DatabaseProvider();
            MongoDatabase db = provider.GetMongoDatabase();
            this.usersCollecion = db.GetCollection<User>("usersInfo");
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
                    this.usersCollecion.Insert<User>(user);
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

            foundUserBySessionKey.SessionKey = null;
            var query = new QueryDocument();
            //this.usersCollecion.Find(
        }

        private User GetUserBySessionKey(string sessionKey)
        {
            return this.usersCollecion.AsQueryable()
                .Single(u => u.SessionKey == sessionKey);
        }

        private User GetUserByUsername(string username)
        {
            return this.usersCollecion.AsQueryable()
                .Where(u => u.Username == username)
                .Select(u => u).FirstOrDefault();
        }

        private User GetUserByEmail(string email)
        {
            return this.usersCollecion.AsQueryable()
                .Where(u => u.Email == email)
                .Select(u => u).FirstOrDefault();
        }
    }
}