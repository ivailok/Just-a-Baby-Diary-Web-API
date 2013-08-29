using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JustABabyDiaryWebAPI.Models.ControllerModels;
using JustABabyDiaryWebAPI.Models;

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

        public HttpResponseMessage PostBabyProfile(BabyProfileModel babyModel)
        {
            HttpResponseMessage responseMsg = this.PerformOperationAndHandleExceptions(
                () =>
                {
                    BabyProfile profile = new BabyProfile
                    {
                        Name=babyModel.Name,
                        BirthDay=babyModel.BirthDay,

                    
                    };

                    var response = this.Request.CreateResponse(HttpStatusCode.Created);
                    return response;
                }
            );

            return responseMsg;
        }
    }
}
