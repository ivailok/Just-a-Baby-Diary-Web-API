using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustABabyDiaryWebAPI.DatabaseProviders
{
    public class DatabaseProvider
    {
        private const MongoClient mongoClient = new MongoClient(
                "mongodb://appharbor_2c3c18a5-d2cb-4d0a-a8cd-a7015d56fbff:l52fm3f78ogrtiavsdcfoa8i78@ds041758.mongolab.com:41758/appharbor_2c3c18a5-d2cb-4d0a-a8cd-a7015d56fbff");

        public static MongoDatabase GetMongoDatabase()
        {
            MongoServer mongoServer = mongoClient.GetServer();

            MongoDatabase mongoDatabase = mongoServer.GetDatabase("appharbor_2c3c18a5-d2cb-4d0a-a8cd-a7015d56fbff");

            return mongoDatabase;
        }

    }
}