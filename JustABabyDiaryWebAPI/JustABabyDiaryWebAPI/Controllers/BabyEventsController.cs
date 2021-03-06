﻿using System.Web.Script.Serialization;
using BloggingSystem.WebAPI.Attributes;
using JustABabyDiaryWebAPI.Models;
using JustABabyDiaryWebAPI.Models.ControllerModels;
using MongoDB.Driver;
using System;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using MongoDB.Bson;

namespace JustABabyDiaryWebAPI.Controllers
{
    public class BabyEventsController : BaseController
    {
        private MongoDatabase db;

        public BabyEventsController()
        {
            DatabaseProviders.DatabaseProvider provider = new DatabaseProviders.DatabaseProvider();
            this.db = provider.GetMongoDatabase();
        }

        [HttpPost]
        public HttpResponseMessage PostBabyEvent([FromBody]BabyEventModel babyEventModel,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey, string babyProfileId)
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

                    BabyEvent babyEvent = new BabyEvent()
                    {
                        Title = babyEventModel.Title,
                        Date = babyEventModel.Date,
                        Description = babyEventModel.Description,
                        PictureNames = babyEventModel.PictureNames
                    };

                    var collection = this.db.GetCollection("baby" + babyProfileId);
                    collection.Insert<BabyEvent>(babyEvent);

                    var response = this.Request.CreateResponse(HttpStatusCode.Created, babyEvent.Id.ToString());
                    return response;
                }
            );

            return responseMsg;
        }


        [HttpPut]
        [ActionName("update")]
        public HttpResponseMessage UpdateBabyEvent([FromBody]BabyEventModel babyEventModel,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey, string babyProfileId, string eventId)
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
                   var eventIdObj = new ObjectId(eventId);

                   var babyEvents = this.db.GetCollection<BabyEvent>("baby" + babyProfileId);
                   var babyEventWithSpecificId = from ev in babyEvents.AsQueryable()
                                                 where ev.Id == eventIdObj
                                                 select ev;

                   var selectedBabyEvent = babyEventWithSpecificId.FirstOrDefault();

                   ChangePropertiesOfBabyProfile(babyEventModel, selectedBabyEvent, babyEvents);

                   var response = this.Request.CreateResponse(HttpStatusCode.OK);
                   return response;
               }
           );

            return responseMsg;
        }

        [HttpPut]
        [ActionName("addpicture")]
        public HttpResponseMessage UpdateAddPhotoToEvent([FromBody]Picture newPicture,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey, string babyProfileId, string eventId)
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
                     var eventIdObj = new ObjectId(eventId);

                     var babyEvents = this.db.GetCollection<BabyEvent>("baby" + babyProfileId);
                     var babyEventWithSpecificId = from ev in babyEvents.AsQueryable()
                                                   where ev.Id == eventIdObj
                                                   select ev;
                     if (newPicture == null)
                     {
                          throw new NullReferenceException("Unexisting Picture!");
                     }

                     var selectedBabyEvent = babyEventWithSpecificId.FirstOrDefault();

                     var query = new QueryDocument { { "_id", selectedBabyEvent.Id } };
                     var update = new UpdateDocument { { "$push", new BsonDocument("PictureNames", new BsonDocument("UrlName", newPicture.UrlName)) } };
                     babyEvents.Update(query, update);

                     var response = this.Request.CreateResponse(HttpStatusCode.OK);
                     return response;
                }
            );

            return responseMsg;
        }

        [HttpPut]
        [ActionName("removepicture")]
        public HttpResponseMessage UpdateRemovePhotoFromEvent([FromBody]Picture picture,
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey, string babyProfileId, string eventId)
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
                    var eventIdObj = new ObjectId(eventId);

                    var babyEvents = this.db.GetCollection<BabyEvent>("baby" + babyProfileId);
                    var babyEventWithSpecificId = from ev in babyEvents.AsQueryable()
                                                  where ev.Id == eventIdObj
                                                  select ev;
                    if (picture == null)
                    {
                        throw new NullReferenceException("Unexisting Picture!");
                    }

                    var selectedBabyEvent = babyEventWithSpecificId.FirstOrDefault();
                   
                    var query = new QueryDocument { { "_id", selectedBabyEvent.Id } , { "PictureNames.UrlName", picture.UrlName } };
                    var update = new UpdateDocument { { "$pop", new BsonDocument("PictureNames", new BsonDocument("UrlName", picture.UrlName)) } };
                    babyEvents.Update(query, update);

                    var response = this.Request.CreateResponse(HttpStatusCode.OK);
                    return response;
                }
            );

            return responseMsg;
        }

        [HttpGet]
        public HttpResponseMessage GetAllBabyEvents(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey, string babyProfileId)
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

                   var babyCollection = this.db.GetCollection<BabyEvent>("baby" + babyProfileId).AsQueryable();
                   IEnumerable<BabyGetEventModel> visibleEventsModels;

                   visibleEventsModels =
                       from item in babyCollection
                       select new BabyGetEventModel()
                       {
                           Id = item.Id.ToString(),
                           Title=item.Title,
                           Description=item.Description,
                           Date=item.Date,
                           PictureNames=item.PictureNames
                       };

                   var response = this.Request.CreateResponse(HttpStatusCode.OK, visibleEventsModels);
                   return response;
               }
           );

            return responseMsg;
        }

        private void ChangePropertiesOfBabyProfile(
            BabyEventModel babyEventModel, BabyEvent selectedBabyEvent, MongoCollection babyEvents)
        {
            if (babyEventModel.Title != null)
            {
                var query = new QueryDocument { { "_id", selectedBabyEvent.Id } };
                var update = new UpdateDocument { { "$set", new BsonDocument("Title", babyEventModel.Title) } };
                babyEvents.Update(query, update);
            }
            if (babyEventModel.Description != null)
            {
                var query = new QueryDocument { { "_id", selectedBabyEvent.Id } };
                var update = new UpdateDocument { { "$set", new BsonDocument("Description", babyEventModel.Description) } };
                babyEvents.Update(query, update);
            }
            if (babyEventModel.Date != null)
            {
                var query = new QueryDocument { { "_id", selectedBabyEvent.Id } };
                var update = new UpdateDocument { { "$set", new BsonDocument("Date", babyEventModel.Date) } };
                babyEvents.Update(query, update);
            }
        }
    }
}
