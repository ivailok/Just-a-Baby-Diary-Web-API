﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace JustABabyDiaryWebAPI.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public string AuthCode { get; set; }

        public string SessionKey { get; set; }

        public string Email { get; set; }

        public User()
        {
        }

        [BsonConstructor]
        public User(string username, string nickname, string authCode, string email)
        {
            this.Username = username;
            this.Nickname = nickname;
            this.AuthCode = authCode;
            this.Email = email;
        }
    }
}