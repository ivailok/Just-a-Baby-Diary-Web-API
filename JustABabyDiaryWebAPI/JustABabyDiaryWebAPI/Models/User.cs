using System;
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

        public string AuthCode { get; set; }

        public ICollection<BabyProfile> BabyProfiles { get; set; }

        [BsonConstructor]
        public User()
        {
            this.BabyProfiles = new List<BabyProfile>();
        }

        [BsonConstructor]
        public User(string username, string authCode)
        {
            this.Username = username;
            this.AuthCode = authCode;
            this.BabyProfiles = new List<BabyProfile>();

        }
    }
}