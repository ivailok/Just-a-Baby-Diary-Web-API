using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustABabyDiaryWebAPI.Models
{
    public class BabyProfile
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDay { get; set; }

        public string Gender { get; set; }

        public string Mother { get; set; }

        public string Father { get; set; }

        public string PictureName { get; set; }

        public string TownOfBirth { get; set; }

        public int BirthWeight { get; set; }

        public int Height { get; set; }

        public BabyProfile()
        {
        }

        public BabyProfile(string name, DateTime birthday, string gender, string mother, string father,
            string pictureName, string townOfBirth, int weight, int height)
        {
            this.Name = name;
            this.BirthDay = birthday;
            this.Gender = gender;
            this.Mother = mother;
            this.Father = father;
            this.PictureName = pictureName;
            this.TownOfBirth = townOfBirth;
            this.BirthWeight = weight;
            this.Height = height;
        }
    }
}