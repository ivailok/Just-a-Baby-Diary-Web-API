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

        public string Date { get; set; }

        public string Description { get; set; }

        public ICollection<Picture> PictureNames { get; set; }

        public BabyEvent()
        {
            this.PictureNames = new List<Picture>(); 
        }
    }
}