using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustABabyDiaryWebAPI.Models.ControllerModels
{
    public class BabyProfileModel
    {
        [BsonId]
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }

        [BsonIgnoreIfNull]
        public string Name { get; set; }

        //public DateTime? BirthDay { get; set; }

        [BsonIgnoreIfNull]
        public string Gender { get; set; }

        [BsonIgnoreIfNull]
        public string Mother { get; set; }

        [BsonIgnoreIfNull]
        public string Father { get; set; }

        [BsonIgnoreIfNull]
        public string PictureName { get; set; }

        [BsonIgnoreIfNull]
        public string TownOfBirth { get; set; }

        public int BirthWeight { get; set; }

        [BsonIgnoreIfNull]
        public int Height { get; set; }

        [BsonConstructor]
        public BabyProfileModel(string name, DateTime birthday, string gender, string mother, string father,
            string pictureName, string townOfBirth, int weight, int height)
        {
            this.Name = name;
            //this.BirthDay = birthday;
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