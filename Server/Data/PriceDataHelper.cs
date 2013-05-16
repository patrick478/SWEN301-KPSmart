using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Gui;
using Server.Business;
using System.Data.SQLite;

namespace Server.Data
{
    public class PriceDataHelper: DataHelper<Price>
    {
        private RouteNodeDataHelper routeNodeDataHelper;

        public PriceDataHelper () {

            this.routeNodeDataHelper = new RouteNodeDataHelper();
            TABLE_NAME = "prices";
            ID_COL_NAME = "price_id";
        }


        public override Price Load(int id)
        {
            throw new NotImplementedException();
        }

        public override IDictionary<int, Price> LoadAll()
        {
            throw new NotImplementedException();
        }

        public override IDictionary<int, Price> LoadAll(DateTime snapshotTime)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Delete(Price obj)
        {
            throw new NotImplementedException();
        }

        public override void Update(Price price)
        {
            // check it has an id
            if (price.ID == 0)
                throw new DatabaseException("Cannot update a price with no ID.");

            // check the key fields aren't changing
            var existingPrice= Load(price.ID);
            if (!price.Origin.Equals(existingPrice.Origin))
                throw new DatabaseException("Cannot modify the key field 'Origin' of the price.");
            if (!price.Destination.Equals(existingPrice.Destination))
                throw new DatabaseException("Cannot modify the key field 'Destination' of the price.");
            if (price.Priority != existingPrice.Priority)
                throw new DatabaseException("Cannot modify the key field 'Priority' of the price.");

            // check that some field is changing
            var pricePerGramSame = price.PricePerGram == existingPrice.PricePerGram;
            var pricePerCm3Same = price.PricePerCm3 == existingPrice.PricePerCm3;
            if (pricePerCm3Same && pricePerGramSame)
                throw new NoChangeException();

            // load ids of fields
            int origin_id = routeNodeDataHelper.GetId(price.Origin);
            int destination_id = routeNodeDataHelper.GetId(price.Destination);

            // check all the fields
            if (origin_id == 0)
                throw new DatabaseException(String.Format("Origin could not be found: {0}", price.Origin));

            if (destination_id == 0)
                throw new DatabaseException(String.Format("Destination could not be found: {0}", price.Destination));

            if (origin_id == destination_id)
                throw new DatabaseException("Origin and destination cannot be the same");

            if (price.Priority == null)
                throw new DatabaseException(String.Format("Priority should not be null: {0}", price));

            if (price.PricePerGram <= 0)
                throw new DatabaseException(String.Format("Route pricePerGram cannot be less than or equal to 0: {0}", price));

            if (price.PricePerCm3 <= 0)
                throw new DatabaseException(String.Format("Route pricePerCm3 cannot be less than or equal to 0: {0}", price));

            string sql;
            object[] row;
            long eventId = 0;

            //LOCK BEGINS HERE
            lock (Database.Instance)
            {

                // check that a price with same key fields doesn't already exist.
                sql = SQLQueryBuilder.SelectFieldsWhereFieldsEqual(TABLE_NAME, new[] { "origin_id", "destination_id", "priority"}, new string[] { price.Origin.ID.ToString(), price.Destination.ID.ToString(), price.Priority.ToString() }, new[] { "price_id" });
                row = Database.Instance.FetchRow(sql);

                if (row.Length == 0)
                    throw new DatabaseException("No route with that [origin, destination, priority] combination exists: " + row[0].ToInt());

                // create a transaction
                SQLiteTransaction transaction = Database.Instance.BeginTransaction();
                try
                {

                    // get event number
                    sql = SQLQueryBuilder.SaveEvent(ObjectType.Route, EventType.Update);
                    eventId = Database.Instance.InsertQuery(sql, transaction);

                    // deactivate all previous records
                    sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}={2}", TABLE_NAME, ID_COL_NAME,
                                        price.ID);
                    Database.Instance.InsertQuery(sql, transaction);


                    // insert the route record
                    sql = SQLQueryBuilder.InsertFields(TABLE_NAME,
                                                              new string[] 
                                                              { 
                                                                  EVENT_ID, 
                                                                  "route_id",
                                                                  "active",
                                                                  "origin_id", 
                                                                  "destination_id", 
                                                                  "priority", 
                                                                  "price_per_gram",
                                                                  "price_per_cm3"                                                            
                                                              },
                                                              new string[] 
                                                              { 
                                                                  eventId.ToString(), 
                                                                  price.ID.ToString(),
                                                                  "1",
                                                                  origin_id.ToString(), 
                                                                  destination_id.ToString(),
                                                                  price.Priority.ToString(),
                                                                  price.PricePerGram.ToString(),
                                                                  price.PricePerCm3.ToString()
                                                              });
                    long inserted_id = Database.Instance.InsertQuery(sql);

                    // commit transaction
                    transaction.Commit();


                    // get id and LastEdited
                    var fields = new string[] { "created" };
                    sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, "id", inserted_id.ToString(), fields);
                    row = Database.Instance.FetchRow(sql);


                }
                catch (SQLiteException de)
                {
                    Console.WriteLine("Got here");
                    transaction.Rollback();
                    Console.WriteLine("Rollback complete");
                    transaction.Dispose();
                    throw de;
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Exception occured during Country.Update() - rolling back:");
                    Logger.WriteLine(e.Message);
                }

            }
            // LOCK ENDS HERE


            // update last edited
            price.LastEdited = (DateTime)row[0];
            Logger.WriteLine("Updated price: " + price);
        }

        public override void Create(Price price)
        {
            object[] row;
            int ID;

            // check it is legal
            int priceId = GetId(price);

            if (priceId != 0)
                throw new DatabaseException("A price with that origin, destination, and priority already exists: " + priceId );


            // check values
            if (price.Origin == null)
                throw new DatabaseException("Origin cannot be null: " + price);

            if (price.Destination == null)
                throw new DatabaseException("Destination cannot be null: " + price);

            if (price.Priority == null)
                throw new DatabaseException("Priority cannot be null: " + price);

            if (price.PricePerCm3 == 0)
                throw new DatabaseException("Price per cm3 cannot be zero: " + price);

            if (price.PricePerGram == 0)
                throw new DatabaseException("Price per gram cannot be zero: " + price);

            // load ids of origin and destination
            int originId = routeNodeDataHelper.GetId(price.Origin);
            int destinationId = routeNodeDataHelper.GetId(price.Destination);


            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                // get event number
                var sql = SQLQueryBuilder.SaveEvent(ObjectType.Price, EventType.Create);
                long eventId = Database.Instance.InsertQuery(sql);

                // insert the record
                sql = SQLQueryBuilder.CreateNewRecord(TABLE_NAME,
                                                          ID_COL_NAME,
                                                          new string[] { EVENT_ID, "origin_id", "destination_id", "priority", "price_per_gram", "price_per_cm3" },
                                                          new string[] { eventId.ToString(), originId.ToString(), destinationId.ToString(), price.Priority.ToString(), price.PricePerGram.ToString(), price.PricePerCm3.ToString() });
                long inserted_id = Database.Instance.InsertQuery(sql);

                // get id and LastEdited
                var fields = new string[] { ID_COL_NAME, "created" };
                sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, "id", inserted_id.ToString(), fields);
                row = Database.Instance.FetchRow(sql);
                
            }
            // LOCK ENDS HERE

            // set id and lastedited
            price.ID = row[0].ToInt();
            price.LastEdited = (DateTime)row[1];

            Logger.WriteLine("Created price: " + price);
        }

        public override int GetId(Price price)
        {
            int id = 0;

            // load ids of origin, destination, and company in case they aren't initialised.
            var originId = routeNodeDataHelper.GetId(price.Origin).ToString();
            var destinationId = routeNodeDataHelper.GetId(price.Destination).ToString();

            //LOCK BEGINS HERE
            lock (Database.Instance)
            {
                // get id of matching record
                var sql = SQLQueryBuilder.SelectFieldsWhereFieldsEqual(TABLE_NAME, new[] { "origin_id", "destination_id", "priority" }, new[] { originId, destinationId, price.Priority.ToString() }, new[] { ID_COL_NAME });
                id = Database.Instance.FetchNumberQuery(sql).ToInt();
            }
            //LOCK ENDS HERE

            // set id in route
            price.ID = id;

            // return result
            return id;
        }
    }
}
