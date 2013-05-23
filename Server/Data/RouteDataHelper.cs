//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// 
//////////////////////

using System;
using System.Collections.Generic;
using Common;
using Server.Gui;
using System.Data.SQLite;
using System.Linq;
using Server.Business;

namespace Server.Data
{
    /// <summary>
    /// The idea is this class is used to extract Routes from the DB, and save routes to the DB.  
    /// It should be used by the RouteService class.
    /// 
    /// Maybe it returns the DateTime instead of void so StateSnapshot can be updated???
    /// </summary>
    public class RouteDataHelper: DataHelper<Route>
    {

        private PriceDataHelper priceDataHelper;
        private DepartureTimeDataHelper departureTimeDataHelper;
        private RouteNodeDataHelper routeNodeDataHelper;
        private CompanyDataHelper companyDataHelper;

        public RouteDataHelper()
        {
            TABLE_NAME = "routes";
            ID_COL_NAME = "route_id";

            priceDataHelper = new PriceDataHelper();
            departureTimeDataHelper = new DepartureTimeDataHelper();
            routeNodeDataHelper = new RouteNodeDataHelper();
            companyDataHelper = new CompanyDataHelper();
        }

        public override Route Load(int id)
        {
            return Load(id, false);
        }

        public Route Load(int id, bool includeInactive) 
        {
            string sql;
            object[] row;

            // LOCK BEGINS HERE
            lock (Database.Instance)
            {

                if (!includeInactive)
                {

                    sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, ID_COL_NAME, id.ToString(), new string[] { "origin_id", 
                                                                                                                            "destination_id",
                                                                                                                            "company_id",
                                                                                                                            "transport_type",
                                                                                                                            "duration",
                                                                                                                            "max_weight",
                                                                                                                            "max_volume",
                                                                                                                            "cost_per_cm3",
                                                                                                                            "cost_per_gram",
                                                                                                                            "created" });
                }
                else {
                    // query to include deleted routes
                    sql = "SELECT origin_id, destination_id, company_id, transport_type, duration, max_weight, max_volume, cost_per_cm3, cost_per_gram, created, active FROM `routes` WHERE active!=-1 GROUP BY route_id ORDER BY event_id DESC";
                }

                row = Database.Instance.FetchRow(sql);
            }
            // LOCK ENDS HERE

            if (row.Length == 0)
            {
                return null;
            }

            // get field values
            int originId = row[0].ToInt();
            int destinationId = row[1].ToInt();
            int companyId = row[2].ToInt();
            string transType = row[3] as string;
            int duration = row[4].ToInt();
            int maxWeight = row[5].ToInt();
            int maxVolume = row[6].ToInt();
            int costPerCm3 = row[7].ToInt();
            int costPerGram = row[8].ToInt();
            DateTime created = (DateTime)row[9];
            bool active = includeInactive && (((int)row[10]) == 0) ? false : true;  // if we also want to bring back inactive records, check, otherwise it is active.


            // load origin
            var origin = routeNodeDataHelper.Load(originId);

            // load destination
            var destination = routeNodeDataHelper.Load(destinationId);

            // load company
            var company = companyDataHelper.Load(companyId);

            // load transportType
            var transportType = transType.ParseTransportTypeFromString();

            // load departure times
            var departureTimes = departureTimeDataHelper.Load(id);


            var route = new Route
            {
                Origin = origin,
                Destination = destination,
                Company = company,
                TransportType = transportType,
                Duration = duration,
                MaxWeight = maxWeight,
                MaxVolume = maxVolume,
                CostPerCm3 = costPerCm3,
                CostPerGram = costPerGram,
                DepartureTimes = departureTimes,
                ID = id,
                LastEdited = created,
                Active = active
            };

            //Logger.WriteLine("Loaded route: " + route);

            return route;       
        }


        public override IDictionary<int, Route> LoadAll()
        {
            string sql;
            object[][] rows;

            // BEGIN LOCK HERE
            lock (Database.Instance)
            {

                sql = SQLQueryBuilder.SelectFields(TABLE_NAME, new string[] { 
                                                                              "origin_id", 
                                                                              "destination_id",
                                                                              "company_id",
                                                                              "transport_type",
                                                                              "duration",
                                                                              "max_weight",
                                                                              "max_volume",
                                                                              "cost_per_cm3",
                                                                              "cost_per_gram",
                                                                              "created",
                                                                              "route_id"});
                rows = Database.Instance.FetchRows(sql);
            }
            // END LOCK HERE
            Logger.WriteLine("Loaded {0} routes:", rows.Length);

            var results = new Dictionary<int, Route>();
            foreach (object[] row in rows)
            {
                // get field values
                int originId = row[0].ToInt();
                int destinationId = row[1].ToInt();
                int companyId = row[2].ToInt();
                string transType = row[3] as string;
                int duration = row[4].ToInt();
                int maxWeight = row[5].ToInt();
                int maxVolume = row[6].ToInt();
                int costPerCm3 = row[7].ToInt();
                int costPerGram = row[8].ToInt();
                DateTime created = (DateTime)row[9];
                int id = row[10].ToInt();


                // load origin
                var origin = routeNodeDataHelper.Load(originId);

                // load destination
                var destination = routeNodeDataHelper.Load(destinationId);

                // load company
                var company = companyDataHelper.Load(companyId);

                // load transportType
                var transportType = transType.ParseTransportTypeFromString();

                // load departure times
                var departureTimes = departureTimeDataHelper.Load(id);

                var route = new Route
                {
                    Origin = origin,
                    Destination = destination,
                    Company = company,
                    TransportType = transportType,
                    Duration = duration,
                    MaxWeight = maxWeight,
                    MaxVolume = maxVolume,
                    CostPerCm3 = costPerCm3,
                    CostPerGram = costPerGram,
                    DepartureTimes = departureTimes,
                    ID = id,
                    LastEdited = created
                };

                Logger.WriteLine(route.ToString());

                // add route to results
                results.Add((int)id, route);
            }

            return results;
        }

        public override IDictionary<int, Route> LoadAll(DateTime snapshotTime)
        {
            string sql;
            object[][] rows;

            // BEGIN LOCK HERE
            lock (Database.Instance)
            {

                sql = SQLQueryBuilder.SelectFieldsAtDateTime(TABLE_NAME, new string[] { 
                                                                              "origin_id", 
                                                                              "destination_id",
                                                                              "company_id",
                                                                              "transport_type",
                                                                              "duration",
                                                                              "max_weight",
                                                                              "max_volume",
                                                                              "cost_per_cm3",
                                                                              "cost_per_gram",
                                                                              "created",
                                                                              "route_id"}, ID_COL_NAME, snapshotTime);
                rows = Database.Instance.FetchRows(sql);
            }
            // END LOCK HERE
            Logger.WriteLine("Loaded {0} routes:", rows.Length);

            var results = new Dictionary<int, Route>();
            foreach (object[] row in rows)
            {
                // get field values
                int originId = row[0].ToInt();
                int destinationId = row[1].ToInt();
                int companyId = row[2].ToInt();
                string transType = row[3] as string;
                int duration = row[4].ToInt();
                int maxWeight = row[5].ToInt();
                int maxVolume = row[6].ToInt();
                int costPerCm3 = row[7].ToInt();
                int costPerGram = row[8].ToInt();
                DateTime created = (DateTime)row[9];
                int id = row[10].ToInt();


                // load origin
                var origin = routeNodeDataHelper.Load(originId);

                // load destination
                var destination = routeNodeDataHelper.Load(destinationId);

                // load company
                var company = companyDataHelper.Load(companyId);

                // load transportType
                var transportType = transType.ParseTransportTypeFromString();

                // load departure times
                var departureTimes = departureTimeDataHelper.Load(id);

                var route = new Route
                {
                    Origin = origin,
                    Destination = destination,
                    Company = company,
                    TransportType = transportType,
                    Duration = duration,
                    MaxWeight = maxWeight,
                    MaxVolume = maxVolume,
                    CostPerCm3 = costPerCm3,
                    CostPerGram = costPerGram,
                    DepartureTimes = departureTimes,
                    ID = id,
                    LastEdited = created
                };

                Logger.WriteLine(route.ToString());

                // add route to results
                results.Add((int)id, route);
            }

            return results;
        }

        public override void Delete(int id)
        {
            if (Load(id) == null)
                throw new DatabaseException(String.Format("There is no active record with id='{0}'", id));

            long eventId = 0;

            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                // get event number
                var sql = SQLQueryBuilder.SaveEvent(ObjectType.Route, EventType.Delete);
                eventId = Database.Instance.InsertQuery(sql);

                // set all entries to inactive
                sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}={2}", TABLE_NAME, ID_COL_NAME, id);
                Database.Instance.InsertQuery(sql);

                // insert new 'deleted' row
                sql = SQLQueryBuilder.InsertFields(TABLE_NAME,
                                                   new string[] { EVENT_ID, ID_COL_NAME, "active", "origin_id", "destination_id", "company_id", "transport_type", "duration", "max_weight", "max_volume", "cost_per_cm3", "cost_per_gram" },
                                                   new string[] { eventId.ToString(), id.ToString(), "-1", "0", "0", "0", "", "0", "0", "0", "0", "0" });
                Database.Instance.InsertQuery(sql);
            }
            // LOCK ENDS HERE


            departureTimeDataHelper.Delete(id, eventId.ToInt());


            Logger.WriteLine("Deleted route: " + id);
        }

        public override void Delete(Route route)
        {
            if (route.ID == 0)
            {
                int id = GetId(route);

                if (id == 0)
                    throw new DatabaseException("There is no active record matching that route to delete: " + route);
            }

            Delete(route.ID);
        }

        public override void Update(Route route)
        {
            // check it has an id
            if (route.ID == 0)
                throw new DatabaseException("Cannot update a route with no ID.");

            // check the key fields aren't changing
            var existingRoute = Load(route.ID);
            if (!route.Origin.Equals(existingRoute.Origin))
                throw new DatabaseException("Cannot modify the key field 'Origin' of the route.");
            if (!route.Destination.Equals(existingRoute.Destination))
                throw new DatabaseException("Cannot modify the key field 'Destination' of the route.");
            if (!route.Company.Equals(existingRoute.Company))
                throw new DatabaseException("Cannot modify the key field 'Company' of the route.");
            if (route.TransportType != existingRoute.TransportType)
                throw new DatabaseException("Cannot modify the key field 'TransportType' of the route.");

            // check that some field is changing
            var durationSame = route.Duration == existingRoute.Duration;
            var maxWeightSame = route.MaxWeight == existingRoute.MaxWeight;
            var maxVolumeSame = route.MaxVolume == existingRoute.MaxVolume;
            var costPerGramSame = route.CostPerGram == existingRoute.CostPerGram;
            var costPerCm3Same = route.CostPerCm3 == existingRoute.CostPerCm3;
            route.DepartureTimes.Sort();
            existingRoute.DepartureTimes.Sort();
            var departureTimesSame = route.DepartureTimes.AsQueryable().SequenceEqual<WeeklyTime>(existingRoute.DepartureTimes);
            if (durationSame && maxWeightSame && maxVolumeSame && costPerCm3Same && costPerGramSame && departureTimesSame)
                throw new NoChangeException();

            // load ids of fields
            int origin_id = routeNodeDataHelper.GetId(route.Origin);
            int destination_id = routeNodeDataHelper.GetId(route.Destination);
            int company_id = companyDataHelper.GetId(route.Company);
            string transport_type = route.TransportType == null ? "" : Enum.GetName(typeof(TransportType), route.TransportType);

            // check all the fields
            if (origin_id == 0)
                throw new DatabaseException(String.Format("Origin could not be found: {0}", route.Origin));

            if (destination_id == 0)
                throw new DatabaseException(String.Format("Destination could not be found: {0}", route.Destination));

            if (origin_id == destination_id)
                throw new DatabaseException("Origin and destination cannot be the same");

            if (company_id == 0)
                throw new DatabaseException(String.Format("Company could not be found: {0}", route.Company));

            if (transport_type == String.Empty)
                throw new DatabaseException(String.Format("Route must have a transport type: {0}", route));

            if (route.Duration == 0)
                throw new DatabaseException(String.Format("Route duration cannot be 0: {0}", route));

            if (route.DepartureTimes.Count == 0)
                throw new DatabaseException(String.Format("Route must have at least one departureTime: {0}", route));

            if (route.MaxWeight <= 0)
                throw new DatabaseException(String.Format("Route maxWeight cannot be less than or equal to 0: {0}", route));

            if (route.MaxVolume <= 0)
                throw new DatabaseException(String.Format("Route maxVolume cannot be less than or equal to 0: {0}", route));

            if (route.CostPerGram <= 0)
                throw new DatabaseException(String.Format("Route costPerGram cannot be less than or equal to 0: {0}", route));

            if (route.CostPerCm3 <= 0)
                throw new DatabaseException(String.Format("Route costPerCm3 cannot be less than or equal to 0: {0}", route));

            string sql;
            object[] row;
            long eventId = 0;

            //LOCK BEGINS HERE
            lock (Database.Instance)
            {

                // check that a route with same key fields doesn't already exist.
                sql = SQLQueryBuilder.SelectFieldsWhereFieldsEqual(TABLE_NAME, new []{"origin_id", "destination_id", "company_id", "transport_type"}, new string []{route.Origin.ID.ToString(), route.Destination.ID.ToString(), route.Company.ID.ToString(), route.TransportType.ToString()}, new []{"route_id"});
                row = Database.Instance.FetchRow(sql);

                if (row.Length == 0)
                    throw new DatabaseException("No route with that [origin, destination, company, transportType] combination already exists: " + row[0].ToInt());

                // create a transaction
                SQLiteTransaction transaction = Database.Instance.BeginTransaction();
                try
                {

                    // get event number
                    sql = SQLQueryBuilder.SaveEvent(ObjectType.Route, EventType.Update);
                    eventId = Database.Instance.InsertQuery(sql, transaction);

                    // deactivate all previous records
                    sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}={2}", TABLE_NAME, ID_COL_NAME,
                                        route.ID);
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
                                                                  "company_id", 
                                                                  "transport_type", 
                                                                  "duration", 
                                                                  "max_weight", 
                                                                  "max_volume", 
                                                                  "cost_per_gram",
                                                                  "cost_per_cm3"                                                            
                                                              },
                                                              new string[] 
                                                              { 
                                                                  eventId.ToString(), 
                                                                  route.ID.ToString(),
                                                                  "1",
                                                                  origin_id.ToString(), 
                                                                  destination_id.ToString(),
                                                                  company_id.ToString(),
                                                                  transport_type,
                                                                  route.Duration.ToString(),
                                                                  route.MaxWeight.ToString(),
                                                                  route.MaxVolume.ToString(),
                                                                  route.CostPerGram.ToString(),
                                                                  route.CostPerCm3.ToString()
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

            // update delivery times
            departureTimeDataHelper.Update(route.ID, eventId.ToInt(), route.DepartureTimes);

            // update last edited
            route.LastEdited = (DateTime)row[0];
            Logger.WriteLine("Updated route: " + route);

        }

        public override void Create(Route route)
        {
            object[] row;

            // check it is legal
            int routeNodeId = GetId(route);
            if (routeNodeId != 0)
                throw new DatabaseException(String.Format("That route already exists: {0}", routeNodeId));

            // load ids of fields
            int origin_id = routeNodeDataHelper.GetId(route.Origin);
            int destination_id = routeNodeDataHelper.GetId(route.Destination);
            int company_id = companyDataHelper.GetId(route.Company);
            string transport_type = route.TransportType == null ? "" : Enum.GetName(typeof(TransportType), route.TransportType);

            // check all the fields
            if (origin_id == 0)
                throw new DatabaseException(String.Format("Origin could not be found: {0}", route.Origin));

            if (destination_id == 0)
                throw new DatabaseException(String.Format("Destination could not be found: {0}", route.Destination));

            if (origin_id == destination_id)
                throw new DatabaseException("Origin and destination cannot be the same");

            if (company_id == 0)
                throw new DatabaseException(String.Format("Company could not be found: {0}", route.Company));

            if (transport_type == String.Empty)
                throw new DatabaseException(String.Format("Route must have a transport type: {0}", route));

            if (route.DepartureTimes.Count == 0)
                throw new DatabaseException(String.Format("Route must have at least one departureTime: {0}", route));

            if (route.Duration == 0)
                throw new DatabaseException(String.Format("Route duration cannot be 0: {0}", route));

            if (route.MaxWeight <= 0)
                throw new DatabaseException(String.Format("Route maxWeight cannot be less than or equal to 0: {0}", route));
            
            if (route.MaxVolume <= 0)
                throw new DatabaseException(String.Format("Route maxVolume cannot be less than or equal to 0: {0}", route));

            if (route.CostPerGram <= 0)
                throw new DatabaseException(String.Format("Route costPerGram cannot be less than or equal to 0: {0}", route));

            if (route.CostPerCm3 <= 0)
                throw new DatabaseException(String.Format("Route costPerCm3 cannot be less than or equal to 0: {0}", route));

            int eventId;

            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                // get event number
                var sql = SQLQueryBuilder.SaveEvent(ObjectType.Route, EventType.Create);
                eventId = Database.Instance.InsertQuery(sql).ToInt();

                // insert the route record
                sql = SQLQueryBuilder.CreateNewRecord(    TABLE_NAME,
                                                          ID_COL_NAME,
                                                          new string[] 
                                                              { 
                                                                  EVENT_ID, 
                                                                  "origin_id", 
                                                                  "destination_id", 
                                                                  "company_id", 
                                                                  "transport_type", 
                                                                  "duration", 
                                                                  "max_weight", 
                                                                  "max_volume", 
                                                                  "cost_per_gram",
                                                                  "cost_per_cm3"                                                            
                                                              },
                                                          new string[] 
                                                              { 
                                                                  eventId.ToString(), 
                                                                  origin_id.ToString(), 
                                                                  destination_id.ToString(),
                                                                  company_id.ToString(),
                                                                  transport_type,
                                                                  route.Duration.ToString(),
                                                                  route.MaxWeight.ToString(),
                                                                  route.MaxVolume.ToString(),
                                                                  route.CostPerGram.ToString(),
                                                                  route.CostPerCm3.ToString()
                                                              });
                long inserted_id = Database.Instance.InsertQuery(sql);

                // get id and LastEdited
                var fields = new string[] { ID_COL_NAME, "created" };
                sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, "id", inserted_id.ToString(), fields);
                row = Database.Instance.FetchRow(sql);
            }
            // LOCK ENDS HERE

            // set id and lastedited
            route.ID = row[0].ToInt();
            route.LastEdited = (DateTime)row[1];

            // save all the departure times
            departureTimeDataHelper.Create(route.ID, eventId, route.DepartureTimes);
                        
            Logger.WriteLine("Updated route: " + route);
        }

        public override int GetId(Route route)
        {
            long id = 0;

            // load ids of origin, destination, and company in case they aren't initialised.
            var originId = routeNodeDataHelper.GetId(route.Origin).ToString();
            var destinationId = routeNodeDataHelper.GetId(route.Destination).ToString();
            var companyId = companyDataHelper.GetId(route.Company).ToString();


            //LOCK BEGINS HERE
            lock (Database.Instance)
            {
                // get id of matching record
                var sql = SQLQueryBuilder.SelectFieldsWhereFieldsEqual(TABLE_NAME, new []{"origin_id", "destination_id", "company_id", "transport_type"}, new[] {originId, destinationId, companyId, route.TransportType.ToString()}, new[]{ID_COL_NAME}); 
                id = Database.Instance.FetchNumberQuery(sql);
            }
            //LOCK ENDS HERE

            // set id in route
            route.ID = id.ToInt();

            // return result
            return route.ID;
        }
    }
}
