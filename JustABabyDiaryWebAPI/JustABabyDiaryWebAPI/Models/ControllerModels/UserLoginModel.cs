using System;
using System.Linq;

namespace JustABabyDiaryWebAPI.Models.ControllerModels
{
    public class UserLoginModel
    {
        public string Username { get; set; }
        public string AuthCode { get; set; }
    }
}