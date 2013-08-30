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
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))]string sessionKey, string babyProfileId, string eventId)
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
                        Title=babyEventModel.Title,
                        Date=babyEventModel.Date,
                        Description=babyEventModel.Description,
                        PictureNames=babyEventModel.PictureNames
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
        public HttpResponseMessage UpdateBabyProfile([FromBody]BabyEventModel babyEventModel,
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

                   var babyEvents = this.db.GetCollection<BabyEvent>("baby" + babyProfileId);
                   var babyEventWithSpecificId = from ev in babyEvents.AsQueryable()
                                                  where ev.Id.ToString() == eventId
                                                  select ev;

                   var selectedBabyEvent = babyEventWithSpecificId.FirstOrDefault();

                   ChangePropertiesOfBabyProfile(babyEventModel,selectedBabyEvent);

                   var response = this.Request.CreateResponse(HttpStatusCode.OK);
                   return response;
               }
           );

            return responseMsg;
        }

        private void ChangePropertiesOfBabyProfile(BabyEventModel babyEventModel,BabyEvent selectedBabyEvent)
        {
            if (babyEventModel.Title!=null)
            {
                selectedBabyEvent.Title = babyEventModel.Title;
            }
            if (babyEventModel.Description!=null)
            {
                selectedBabyEvent.Description = babyEventModel.Description;
            }
            if (babyEventModel.Date!=null)
            {
                selectedBabyEvent.Date = babyEventModel.Date;
            }
        }
    }
}
