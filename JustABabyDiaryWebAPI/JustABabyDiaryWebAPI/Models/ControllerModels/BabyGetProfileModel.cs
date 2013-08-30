using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustABabyDiaryWebAPI.Models.ControllerModels
{
    public class BabyGetProfileModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime BirthDay { get; set; }

        public string Gender { get; set; }

        public string Mother { get; set; }

        public string Father { get; set; }

        public string PictureName { get; set; }

        public string TownOfBirth { get; set; }

        public int BirthWeight { get; set; }

        public int Height { get; set; }
    }
}