﻿using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JustABabyDiaryWebAPI.Models.ControllerModels;
using JustABabyDiaryWebAPI.Models;
using MongoDB.Bson;

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
        public HttpResponseMessage PostBabyProfile([FromBody]BabyProfileModel babyModel, [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string userId)
        {
            HttpResponseMessage responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    BabyProfile babyProfile = new BabyProfile
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

                    this.db.CreateCollection(userId.ToString());

                    var response = this.Request.CreateResponse(HttpStatusCode.Created, babyProfile.Id);
                    return response;
                }
            );

            return responseMsg;
        }
    }
}
