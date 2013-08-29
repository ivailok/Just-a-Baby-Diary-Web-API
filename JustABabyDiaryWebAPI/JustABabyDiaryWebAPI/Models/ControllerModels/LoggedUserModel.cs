using System;
using System.Linq;

namespace JustABabyDiaryWebAPI.Models.ControllerModels
{
    public class LoggedUserModel
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string SessionKey { get; set; }
    }
}