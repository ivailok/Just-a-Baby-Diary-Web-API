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
            string primeConnectionString = "mongodb://appharbor_83641b3e-2d74-46aa-9a7c-2c8fe346e9bd:83vv6tk99o2a690rr31jig2j13@ds043168.mongolab.com:43168/appharbor_83641b3e-2d74-46aa-9a7c-2c8fe346e9bd";
            string testConnectionString = "mongodb://127.0.0.1/JustABabyDiary";

            MongoClient mongoClient = new MongoClient(primeConnectionString);
            MongoServer mongoServer = mongoClient.GetServer();

            this.mongoDatabase = mongoServer.GetDatabase("appharbor_83641b3e-2d74-46aa-9a7c-2c8fe346e9bd");
        }

        public MongoDatabase GetMongoDatabase()
        {
            return this.mongoDatabase;
        }
    }
}