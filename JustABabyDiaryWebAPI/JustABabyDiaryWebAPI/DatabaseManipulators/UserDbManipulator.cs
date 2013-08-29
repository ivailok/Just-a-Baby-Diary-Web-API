﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace JustABabyDiaryWebAPI.DatabaseManipulators
{
    public class UserDbManipulator
    {
        private MongoDatabase db;

        public UserDbManipulator()
        {
            DatabaseProviders.DatabaseProvider provider = new DatabaseProviders.DatabaseProvider();
            this.db = provider.GetMongoDatabase();
        }
    }
}