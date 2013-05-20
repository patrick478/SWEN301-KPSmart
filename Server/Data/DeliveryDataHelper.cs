using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Gui;

namespace Server.Data
{
    public class DeliveryDataHelper: DataHelper<Delivery>
    {
        private RouteNodeDataHelper routeNodeDataHelper;
        private DeliveryRouteInstanceDataHelper deliveryRouteInstanceDataHelper;
        
        
        public DeliveryDataHelper () 
        {
            this.routeNodeDataHelper = new RouteNodeDataHelper();
            TABLE_NAME = "deliveries";
            ID_COL_NAME = "delivery_id";     
        }


        public override Delivery Load(int id)
        {
            string sql;
            object[] row;

            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, ID_COL_NAME, id.ToString(), new string[] { "origin_id", 
                                                                                                                          "destination_id",
                                                                                                                          "priority",
                                                                                                                          "weight_in_grams",
                                                                                                                          "volume_in_cm3",
                                                                                                                          "total_price",
                                                                                                                          "total_cost",
                                                                                                                          "time_of_request",
                                                                                                                          "time_of_delivery",
                                                                                                                          "created"});
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
            Priority priority = row[2].ToString() == "Air" ? Priority.Air : Priority.Standard;
            int weight = row[3].ToInt();
            int volume = row[4].ToInt();
            int totalPrice = row[5].ToInt();
            int totalCost = row[6].ToInt();
            DateTime timeOfRequest = (DateTime)row[7];
            DateTime timeOfDelivery = (DateTime)row[8];
            DateTime created = (DateTime)row[9];


            // load origin
            var origin = routeNodeDataHelper.Load(originId);

            // load destination
            var destination = routeNodeDataHelper.Load(destinationId);

            // load routeInstances
            var routeInstances = deliveryRouteInstanceDataHelper.Load(id);

            var delivery = new Delivery
            {
                Origin = origin,
                Destination = destination,
                Priority = priority,
                WeightInGrams = weight,
                VolumeInCm3 = volume,
                TotalPrice = totalPrice,
                TotalCost = totalCost,
                TimeOfRequest = timeOfRequest,
                TimeOfDelivery = timeOfDelivery,
                ID = id,
                LastEdited = created,
                Routes = routeInstances
            };

            Logger.WriteLine("Loaded delivery: " + delivery);

            return delivery;
        }

        public override IDictionary<int, Delivery> LoadAll()
        {
            string sql;
            object[][] rows;
            Dictionary<int, Delivery> deliveries = new Dictionary<int, Delivery>();

            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                sql = SQLQueryBuilder.SelectFields(TABLE_NAME,  new string[] { "origin_id", 
                                                                               "destination_id",
                                                                               "priority",
                                                                               "weight_in_grams",
                                                                               "volume_in_cm3",
                                                                               "total_price",
                                                                               "total_cost",
                                                                               "time_of_request",
                                                                               "time_of_delivery",
                                                                               "created",
                                                                               "delivery_id"});
                rows = Database.Instance.FetchRows(sql);
            }
            // LOCK ENDS HERE

            if (rows.Length == 0)
            {
                return null;
            }


            foreach (object[] row in rows)
            {
                // get field values
                int originId = row[0].ToInt();
                int destinationId = row[1].ToInt();
                Priority priority = row[2].ToString() == "Air" ? Priority.Air : Priority.Standard;
                int weight = row[3].ToInt();
                int volume = row[4].ToInt();
                int totalPrice = row[5].ToInt();
                int totalCost = row[6].ToInt();
                DateTime timeOfRequest = (DateTime)row[7];
                DateTime timeOfDelivery = (DateTime)row[8];
                DateTime created = (DateTime)row[9];
                int id = row[10].ToInt();

                // load origin
                var origin = routeNodeDataHelper.Load(originId);

                // load destination
                var destination = routeNodeDataHelper.Load(destinationId);

                var routeInstances = deliveryRouteInstanceDataHelper.Load(id);

                var delivery = new Delivery
                {
                    Origin = origin,
                    Destination = destination,
                    Priority = priority,
                    WeightInGrams = weight,
                    VolumeInCm3 = volume,
                    TotalPrice = totalPrice,
                    TotalCost = totalCost,
                    TimeOfRequest = timeOfRequest,
                    TimeOfDelivery = timeOfDelivery,
                    ID = id,
                    LastEdited = created,
                    Routes = routeInstances
                };

                deliveries[id] = delivery;
                Logger.WriteLine(delivery.ToString());
            }

            return deliveries;         
        }

        public override IDictionary<int, Delivery> LoadAll(DateTime snapshotTime)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not relevant
        /// </summary>
        /// <param name="obj"></param>
        public override void Delete(int id)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not relevant
        /// </summary>
        /// <param name="obj"></param>
        public override void Delete(Delivery obj)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not relevant
        /// </summary>
        /// <param name="obj"></param>
        public override void Update(Delivery obj)
        {
            throw new NotSupportedException();
        }

        public override void Create(Delivery delivery)
        {
            object[] row;

            // load ids of fields
            int origin_id = routeNodeDataHelper.GetId(delivery.Origin);
            int destination_id = routeNodeDataHelper.GetId(delivery.Destination);
            string priority = delivery.Priority == null ? "" : delivery.Priority.ToString();

            // check all the fields
            if (origin_id == 0)
                throw new DatabaseException(String.Format("Origin could not be found: {0}", delivery.Origin));

            if (destination_id == 0)
                throw new DatabaseException(String.Format("Destination could not be found: {0}", delivery.Destination));

            if (origin_id == destination_id)
                throw new DatabaseException("Origin and destination cannot be the same");

            if (priority == "")
                throw new DatabaseException("Priority cannot be null");

            if (delivery.WeightInGrams == 0)
                throw new DatabaseException(String.Format("Delivery weight cannot be 0: {0}", delivery));

            if (delivery.VolumeInCm3 == 0)
                throw new DatabaseException(String.Format("Delivery volume cannot be 0: {0}", delivery));

            if (delivery.Duration.Ticks == 0)
                throw new DatabaseException(String.Format("Delivery duration cannot be 0: {0}", delivery));

            if (delivery.TotalPrice == 0)
                throw new DatabaseException(String.Format("Total price cannot be 0: {0}", delivery));

            if (delivery.TotalCost == 0)
                throw new DatabaseException(String.Format("Total cost cannot be 0: {0}", delivery));

            if (delivery.TimeOfRequest.Ticks == 0)
                throw new DatabaseException(String.Format("TimeOfRequest cannot be null: {0}", delivery));

            if (delivery.TimeOfDelivery.Ticks == 0)
                throw new DatabaseException(String.Format("TimeOfDelivery cannot be null: {0}", delivery));

            if (delivery.Routes == null || delivery.Routes.Count == 0)
                throw new DatabaseException("There needs to be at least one route instance in the delivery: " + delivery);
            
            int eventId;

            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                // get event number
                var sql = SQLQueryBuilder.SaveEvent(ObjectType.Delivery, EventType.Create);
                eventId = Database.Instance.InsertQuery(sql).ToInt();

                // insert the route record
                sql = SQLQueryBuilder.CreateNewRecord(TABLE_NAME,
                                                          ID_COL_NAME,
                                                          new string[] 
                                                              { 
                                                                  EVENT_ID, 
                                                                  "origin_id", 
                                                                  "destination_id", 
                                                                  "priority", 
                                                                  "weight_in_grams", 
                                                                  "volume_in_cm3", 
                                                                  "total_cost",
                                                                  "total_price",
                                                                  "time_of_request",
                                                                  "time_of_delivery"                                                           
                                                              },
                                                          new string[] 
                                                              { 
                                                                  eventId.ToString(), 
                                                                  origin_id.ToString(), 
                                                                  destination_id.ToString(),
                                                                  delivery.Priority.ToString(),
                                                                  delivery.WeightInGrams.ToString(),
                                                                  delivery.VolumeInCm3.ToString(),
                                                                  delivery.TotalCost.ToString(),
                                                                  delivery.TotalPrice.ToString(),
                                                                  delivery.TimeOfRequest.ToString("yyyy-MM-dd HH:mm:ss"),
                                                                  delivery.TimeOfDelivery.ToString("yyyy-MM-dd HH:mm:ss")
                                                              });
                long inserted_id = Database.Instance.InsertQuery(sql);

                // get id and LastEdited
                var fields = new string[] { ID_COL_NAME, "created" };
                sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, "id", inserted_id.ToString(), fields);
                row = Database.Instance.FetchRow(sql);
            }
            // LOCK ENDS HERE

            // save route instances
            deliveryRouteInstanceDataHelper.Create(delivery.ID, eventId, delivery.Routes);

            // set id and lastedited
            delivery.ID = row[0].ToInt();
            delivery.LastEdited = (DateTime)row[1];

            Logger.WriteLine("Created delivery: " + delivery);
        }

        /// <summary>
        /// Not relevant
        /// </summary>
        /// <param name="delivery"></param>
        /// <returns></returns>
        public override int GetId(Delivery delivery)
        {
            throw new NotSupportedException();
        }
    }
}
