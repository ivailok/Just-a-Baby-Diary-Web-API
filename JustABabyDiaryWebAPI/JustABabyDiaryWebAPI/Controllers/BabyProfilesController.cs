using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JustABabyDiaryWebAPI.Models.ControllerModels;
using JustABabyDiaryWebAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Web.Http.ValueProviders;
using BloggingSystem.WebAPI.Attributes;

namespace JustABabyDiaryWebAPI.Controllers
{
    public class BabyProfilesController : BaseController
    {
        private MongoDatabase db;

        public BabyProfilesController()
        {
            DatabaseProviders.DatabaseProvider provider = new DatabaseProviders.DatabaseProvider();
            this.db = provider.GetMongoDatabase();
        }

        [HttpPost]
        public HttpResponseMessage PostBabyProfile([FromBody]BabyProfileModel babyModel,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
        {
            HttpResponseMessage responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    var usersWithSpecificId = from u in this.db.GetCollection<User>("usersInfo").AsQueryable()
                                              where u.SessionKey == sessionKey
                                              select u;
                    User selectedUser = usersWithSpecificId.FirstOrDefault();

                    if (selectedUser==null)
                    {
                        throw new NullReferenceException("User is logged out or does not exist!");
                    }

                    BabyProfile babyProfile = new BabyProfile()
                    {
                        Name=babyModel.Name,
                        BirthDay=babyModel.BirthDay,
                        Mother=babyModel.Mother,
                        Father=babyModel.Father,
                        Gender=babyModel.Gender,
                        BirthWeight=babyModel.BirthWeight,
                        Height=babyModel.Height,
                        TownOfBirth=babyModel.TownOfBirth,
                        PictureName=babyModel.PictureName
                    };
                    var collection = this.db.GetCollection("user" + selectedUser.Id.ToString());
                    collection.Insert<BabyProfile>(babyProfile);

                    var response = this.Request.CreateResponse(HttpStatusCode.Created, babyProfile.Id.ToString());
                    return response;
                }
            );

            return responseMsg;
        }
    }
}
