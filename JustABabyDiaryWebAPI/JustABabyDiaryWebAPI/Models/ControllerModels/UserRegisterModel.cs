using System;
using System.Linq;

namespace JustABabyDiaryWebAPI.Models.ControllerModels
{
    public class UserRegisterModel
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string AuthCode { get; set; }
        public string Email { get; set; }
    }
}