using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JustABabyDiaryWebAPI.Controllers
{
    public class BabyProfilesController : ApiController
    {
        private MongoDatabase db;

        public BabyProfilesController()
        {
            DatabaseProviders.DatabaseProvider provider = new DatabaseProviders.DatabaseProvider();
            this.db = provider.GetMongoDatabase();
        }
    }
}
