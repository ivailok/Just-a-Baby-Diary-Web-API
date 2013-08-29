﻿using System;
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
            var foundUser = this.GetUser(user);

            if (foundUser == null)
            {
                user.SessionKey = this.GenerateSessionKey(user.Username);
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