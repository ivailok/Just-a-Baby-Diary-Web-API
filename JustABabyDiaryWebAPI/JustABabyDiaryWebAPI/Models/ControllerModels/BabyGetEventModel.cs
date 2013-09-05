using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustABabyDiaryWebAPI.Models.ControllerModels
{
    public class BabyGetEventModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }

        public string Description { get; set; }

        public ICollection<Picture> PictureNames { get; set; }

    }
}