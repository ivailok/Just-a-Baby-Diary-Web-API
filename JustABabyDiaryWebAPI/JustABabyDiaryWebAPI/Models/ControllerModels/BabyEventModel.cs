using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustABabyDiaryWebAPI.Models.ControllerModels
{
    public class BabyEventModel
    {
        public string Title { get; set; }

        public string Date { get; set; }

        public string Description { get; set; }

        public ICollection<Picture> PictureNames { get; set; }
    }
}