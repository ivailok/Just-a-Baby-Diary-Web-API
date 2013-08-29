using System;
using System.Linq;

namespace JustABabyDiaryWebAPI.Models.ControllerModels
{
    public class LoggedUserModel
    {
        public string DisplayName { get; set; }
        public string SessionKey { get; set; }
    }
}