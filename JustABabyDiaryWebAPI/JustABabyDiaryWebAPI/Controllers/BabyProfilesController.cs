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

                    if (selectedUser == null)
                    {
                        throw new NullReferenceException("User is logged out or does not exist!");
                    }

                    BabyProfile babyProfile = new BabyProfile()
                    {
                        Name = babyModel.Name,
                        BirthDay = babyModel.BirthDay,
                        Mother = babyModel.Mother,
                        Father = babyModel.Father,
                        Gender = babyModel.Gender,
                        BirthWeight = babyModel.BirthWeight,
                        Height = babyModel.Height,
                        TownOfBirth = babyModel.TownOfBirth,
                        PictureName = babyModel.PictureName
                    };
                    var collection = this.db.GetCollection("user" + selectedUser.Id.ToString());
                    collection.Insert<BabyProfile>(babyProfile);
                    this.db.CreateCollection("baby" + babyProfile.Id);

                    var response = this.Request.CreateResponse(HttpStatusCode.Created, babyProfile.Id.ToString());
                    return response;
                }
            );

            return responseMsg;
        }

        [HttpPut]
        public HttpResponseMessage UpdateBabyProfile([FromBody]BabyProfileModel babyModel,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey, string name)
        {
            HttpResponseMessage responseMsg = this.PerformOperationAndHandleExceptions(
               () =>
               {
                   var usersWithSpecificSessionKey = from u in this.db.GetCollection<User>("usersInfo").AsQueryable()
                                                     where u.SessionKey == sessionKey
                                                     select u;
                   User selectedUser = usersWithSpecificSessionKey.FirstOrDefault();

                   if (selectedUser == null)
                   {
                       throw new NullReferenceException("User is logged out or does not exist!");
                   }

                   var babyCollection = this.db.GetCollection<BabyProfile>("user" + selectedUser.Id.ToString());
                   var babyProfilesWithSpecific = from p in babyCollection.AsQueryable()
                                                  where p.Name == name
                                                  select p;

                   var selectedBabyProfile = babyProfilesWithSpecific.FirstOrDefault();

                   ChangePropertiesOfBabyProfile(babyModel, babyCollection, selectedBabyProfile);

                   var response = this.Request.CreateResponse(HttpStatusCode.OK);
                   return response;
               }
           );

            return responseMsg;
        }

        [HttpGet]
        public HttpResponseMessage GetAllBabyProfiles(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey)
        {
            HttpResponseMessage responseMsg = this.PerformOperationAndHandleExceptions(
               () =>
               {
                   var usersWithSpecificId = from u in this.db.GetCollection<User>("usersInfo").AsQueryable()
                                             where u.SessionKey == sessionKey
                                             select u;
                   User selectedUser = usersWithSpecificId.FirstOrDefault();

                   if (selectedUser == null)
                   {
                       throw new NullReferenceException("User is logged out or does not exist!");
                   }

                   var babyCollection = this.db.GetCollection<BabyProfile>("user" + selectedUser.Id.ToString()).AsQueryable();
                   IEnumerable<BabyGetProfileModel> visibleModels;

                   visibleModels =
                       from item in babyCollection
                       select new BabyGetProfileModel()
                       {
                           Id = item.Id.ToString(),
                           Name = item.Name,
                           Mother = item.Mother,
                           Father = item.Father,
                           BirthDay = item.BirthDay,
                           BirthWeight = item.BirthWeight,
                           Gender = item.Gender,
                           Height = item.Height,
                           PictureName = item.PictureName,
                           TownOfBirth = item.TownOfBirth
                       };

                   var response = this.Request.CreateResponse(HttpStatusCode.OK, visibleModels);
                   return response;
               }
           );

            return responseMsg;
        }

        private void ChangePropertiesOfBabyProfile(BabyProfileModel babyModel,
            MongoCollection babyCollection,BabyProfile selectedBabyProfile)
        {

            if (babyModel.Name != null)
            {
                var query = new QueryDocument { { "Name", selectedBabyProfile.Name } };
                var update = new UpdateDocument { { "$set", new BsonDocument("Name", babyModel.Name) } };
                babyCollection.Update(query, update);
            }
            if (babyModel.Mother != null)
            {
                var query = new QueryDocument { { "Mother", selectedBabyProfile.Mother } };
                var update = new UpdateDocument { { "$set", new BsonDocument("Mother", babyModel.Mother) } };
                babyCollection.Update(query, update);
            }
            if (babyModel.Father != null)
            {
                var query = new QueryDocument { { "Father", selectedBabyProfile.Father } };
                var update = new UpdateDocument { { "$set", new BsonDocument("Father", babyModel.Father) } };
                babyCollection.Update(query, update);
            }
            if (babyModel.BirthDay != null)
            {
                var query = new QueryDocument { { "BirthDay", selectedBabyProfile.BirthDay } };
                var update = new UpdateDocument { { "$set", new BsonDocument("BirthDay", babyModel.BirthDay) } };
                babyCollection.Update(query, update);
            }
            if (babyModel.BirthWeight != 0)
            {
                var query = new QueryDocument { { "BirthWeight", selectedBabyProfile.BirthWeight } };
                var update = new UpdateDocument { { "$set", new BsonDocument("BirthWeight", babyModel.BirthWeight) } };
                babyCollection.Update(query, update);
            }
            if (babyModel.Gender != null)
            {
                var query = new QueryDocument { { "Gender", selectedBabyProfile.Gender } };
                var update = new UpdateDocument { { "$set", new BsonDocument("Gender", babyModel.Gender) } };
                babyCollection.Update(query, update);
            }
            if (babyModel.Height != 0)
            {
                var query = new QueryDocument { { "Height", selectedBabyProfile.Height } };
                var update = new UpdateDocument { { "$set", new BsonDocument("Height", babyModel.Height) } };
                babyCollection.Update(query, update);
            }
            if (babyModel.PictureName != null)
            {
                var query = new QueryDocument { { "PictureName", selectedBabyProfile.PictureName } };
                var update = new UpdateDocument { { "$set", new BsonDocument("PictureName", babyModel.PictureName) } };
                babyCollection.Update(query, update);
            }
            if (babyModel.TownOfBirth != null)
            {
                var query = new QueryDocument { { "TownOfBirth", selectedBabyProfile.TownOfBirth } };
                var update = new UpdateDocument { { "$set", new BsonDocument("TownOfBirth", babyModel.TownOfBirth) } };
                babyCollection.Update(query, update);
            }
        }
    }
}
