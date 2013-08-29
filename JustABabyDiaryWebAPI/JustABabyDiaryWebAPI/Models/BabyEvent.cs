using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustABabyDiaryWebAPI.Models
{
    public class BabyEvent
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public ICollection<string> PictureNames { get; set; }

        [BsonConstructor]
        public BabyEvent()
        {
            this.PictureNames = new List<string>(); 
        }
    }
}