using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustABabyDiaryWebAPI.DatabaseProviders
{
    public class DatabaseProvider
    {
        private MongoDatabase mongoDatabase;

        public DatabaseProvider()
        {
            this.InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            string primeConnectionString = "mongodb://appharbor_2c3c18a5-d2cb-4d0a-a8cd-a7015d56fbff:l52fm3f78ogrtiavsdcfoa8i78@ds041758.mongolab.com:41758/appharbor_2c3c18a5-d2cb-4d0a-a8cd-a7015d56fbff";
            string testConnectionString = "mongodb://127.0.0.1/JustABabyDiary";

            MongoClient mongoClient = new MongoClient(testConnectionString);
            MongoServer mongoServer = mongoClient.GetServer();

            this.mongoDatabase = mongoServer.GetDatabase("appharbor_2c3c18a5-d2cb-4d0a-a8cd-a7015d56fbff");
        }

        public MongoDatabase GetMongoDatabase()
        {
            return this.mongoDatabase;
        }
    }
}