using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using BloggingSystem.WebAPI.Attributes;
using JustABabyDiaryWebAPI.Models;
using MongoDB.Driver;
using JustABabyDiaryWebAPI.Models.ControllerModels;
using JustABabyDiaryWebAPI.DatabaseManipulators;

namespace JustABabyDiaryWebAPI.Controllers
{
    public class UsersController : BaseController
    {
        private const int MinUsernameLength = 6;
        private const int MaxUsernameLength = 30;
        private const int MinDisplayNameLength = 6;
        private const int MaxDisplayNameLength = 30;
        private const string ValidUsernameCharacters =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_.";
        private const string ValidDisplayNameCharacters =
            "qwertyuioplkjhgfdsazxcvbnmQWERTYUIOPLKJHGFDSAZXCVBNM1234567890_. -";

        private const int Sha1Length = 40;

        private UserDbManipulator manipulator;

        public UsersController()
        {
            manipulator = new UserDbManipulator();
        }

        [ActionName("register")]
        public HttpResponseMessage PostRegisterUser(UserRegisterModel userModel)
        {
            HttpResponseMessage responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    this.ValidateUser(userModel);

                    User user = new User()
                    {
                        Username = userModel.Username,
                        Nickname = userModel.DisplayName,
                        AuthCode = userModel.AuthCode,
                        Email = userModel.Email
                    };

                    User registeredUser = this.manipulator.Register(user);

                    var loggedModel = new LoggedUserModel()
                    {
                        Id = registeredUser.Id.ToString(),
                        Nickname = registeredUser.Nickname,
                        SessionKey = registeredUser.SessionKey
                    };

                    var response = this.Request.CreateResponse(HttpStatusCode.Created, loggedModel);
                    return response;
                }
            );

            return responseMsg;
        }

        [ActionName("logout")]
        public HttpResponseMessage PutLogoutUser(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
        {
            HttpResponseMessage responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    this.manipulator.Logout(sessionKey);
                    HttpResponseMessage response = this.Request.CreateResponse(HttpStatusCode.OK);
                    return response;
                });

            return responseMsg;
        }

        //[ActionName("login")]
        //public HttpResponseMessage PostLoginUser(UserLoginModel userModel)
        //{
        //    HttpResponseMessage responseMsg = this.PerformOperationAndHandleExceptions(
        //        () =>
        //        {
        //            User user = new User()
        //            {
        //                Username = userModel.Username,
        //                AuthCode = userModel.AuthCode
        //            };

        //            User registeredUser = this.db.Login(user);

        //            var loggedModel = new LoggedUserModel()
        //            {
        //                DisplayName = registeredUser.DisplayName,
        //                SessionKey = registeredUser.SessionKey
        //            };

        //            var response = this.Request.CreateResponse(HttpStatusCode.Created, loggedModel);
        //            return response;
        //        }
        //    );

        //    return responseMsg;
        //}

        private void ValidateUser(UserRegisterModel userModel)
        {
            this.ValidateUsername(userModel.Username);
            this.ValidateDisplayName(userModel.DisplayName);
            this.ValidateAuthCode(userModel.AuthCode);
        }

        private void ValidateAuthCode(string authCode)
        {
            if (authCode == null || authCode.Length != Sha1Length)
            {
                throw new ArgumentOutOfRangeException("Password should be encrypted");
            }
        }

        private void ValidateDisplayName(string displayName)
        {
            if (displayName == null)
            {
                throw new ArgumentNullException("Display name cannot be null");
            }
            else if (displayName.Length < MinDisplayNameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Display name must be at least {0} characters long",
                    MinDisplayNameLength));
            }
            else if (displayName.Length > MaxDisplayNameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Display name must be less than {0} characters long",
                    MaxDisplayNameLength));
            }
            else if (displayName.Any(ch => !ValidDisplayNameCharacters.Contains(ch)))
            {
                throw new ArgumentOutOfRangeException(
                    "Display name must contain only Latin letters, digits .,_");
            }
        }

        private void ValidateUsername(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException("Username cannot be null");
            }
            else if (username.Length < MinUsernameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Username must be at least {0} characters long",
                    MinUsernameLength));
            }
            else if (username.Length > MaxUsernameLength)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Username must be less than {0} characters long",
                    MaxUsernameLength));
            }
            else if (username.Any(ch => !ValidUsernameCharacters.Contains(ch)))
            {
                throw new ArgumentOutOfRangeException(
                    "Username must contain only Latin letters, digits .,_");
            }
        }
    }
}
