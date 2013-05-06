using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Gui;

namespace Server.Data
{
    public class RouteNodeDataHelper: DataHelper<RouteNode>
    {

        private const string TABLE_NAME = "route_nodes";
        private const string ID_COL_NAME = "route_node_id";
        private CountryDataHelper countryDataHelper;

        public RouteNodeDataHelper()
        {
            this.countryDataHelper = new CountryDataHelper();
        }


        public override RouteNode Load(int id)
        {

            string sql;
            object[] row;

            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, ID_COL_NAME, id.ToString(), new string[] {"country_id", "name", "created" });
                row = Database.Instance.FetchRow(sql);
            }
            // LOCK ENDS HERE

            if (row.Length == 0)
            {
                return null;
            }

            // get info out of row
            int country_id = row[0].ToInt();
            string name = row[1] as string;
            DateTime created = (DateTime)row[2];

            // load the country
            var country = countryDataHelper.Load(country_id);

            // create the routeNode
            RouteNode routeNode;
            if (name == String.Empty)
            {
                routeNode = new InternationalPort(country) {ID = country_id, LastEdited = created};
            }
            else
            {
                routeNode = new DistributionCentre(name){ID = country_id, LastEdited = created};
            }

            Logger.WriteLine("Loaded routeNode: " + routeNode);
            return routeNode;
        }

        public override IDictionary<int, RouteNode> LoadAll()
        {
            string sql;
            object[][] rows;

            // BEGIN LOCK HERE
            lock (Database.Instance)
            {
                sql = SQLQueryBuilder.SelectFields(TABLE_NAME, new string[] { ID_COL_NAME, "country_id", "name", "created" });
                rows = Database.Instance.FetchRows(sql);
            }
            // END LOCK HERE
            Logger.WriteLine("Loaded {0} routeNodes:", rows.Length);

            var results = new Dictionary<int, RouteNode>();
            foreach (object[] row in rows)
            {
                // extract data
                int id = row[0].ToInt();
                int country_id = row[1].ToInt();
                string name = row[2] as string;
                DateTime created = (DateTime)row[3];

                // country
                var country = countryDataHelper.Load(country_id);

                // make routeNode
                if(name == String.Empty)
                    results[id] = new InternationalPort(country){ID = id, LastEdited = created};

                if(name != String.Empty)
                    results[id] = new DistributionCentre(name) {ID=id, LastEdited = created};

                Logger.WriteLine(results[id].ToString());
            }

            return results;
        }

        public override IDictionary<int, RouteNode> LoadAll(DateTime snapshotTime)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            if (Load(id) == null)
                throw new DatabaseException(String.Format("There is no active record with country_id='{0}'", id));

            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                // get event number
                var sql = SQLQueryBuilder.SaveEvent(ObjectType.RouteNode, EventType.Delete);
                long eventId = Database.Instance.InsertQuery(sql);

                // set all entries to inactive
                sql = String.Format("UPDATE `{0}` SET active=0 WHERE {1}={2}", TABLE_NAME, ID_COL_NAME, id);
                Database.Instance.InsertQuery(sql);

                // insert new 'deleted' row
                sql = SQLQueryBuilder.InsertFields(TABLE_NAME,
                                                   new string[] { EVENT_ID, ID_COL_NAME, "active", "country_id", "name" },
                                                   new string[] { eventId.ToString(), id.ToString(), "-1", "", "" });
                Database.Instance.InsertQuery(sql);
            }
            // LOCK ENDS HERE

            Logger.WriteLine("Deleted routeNode: " + id);
        }

        public override void Delete(RouteNode obj)
        {
            if (obj.ID == 0)
            {
                int id = GetId(obj);

                if (id == 0)
                    throw new DatabaseException("There is no active record matching that routeNode to delete: " + obj);

                Delete(id);
            }

            Delete(obj.ID);
        }

        public override void Update(RouteNode routeNode)
        {
            throw new NotImplementedException("Cannot update a route node.");    
        }

        public override void Create(RouteNode routeNode)
        {
            object[] row;
            int ID;

            // check it is legal
            int routeNodeId = GetId(routeNode);

            if (routeNodeId != 0)
                throw new DatabaseException(String.Format("That route node already exists: {0}", routeNodeId));

            InternationalPort internationalPort = routeNode as InternationalPort;
            DistributionCentre distributionCentre = routeNode as DistributionCentre;

            int country_id = countryDataHelper.GetId(routeNode.Country);
            string name = distributionCentre == null ? "" : distributionCentre.Name; 

            // LOCK BEGINS HERE
            lock (Database.Instance)
            {
                // get event number
                var sql = SQLQueryBuilder.SaveEvent(ObjectType.RouteNode, EventType.Create);
                long eventId = Database.Instance.InsertQuery(sql);

                // insert the record
                sql = SQLQueryBuilder.CreateNewRecord(TABLE_NAME,
                                                          ID_COL_NAME,
                                                          new string[] { EVENT_ID, "country_id", "name" },
                                                          new string[] { eventId.ToString(), country_id.ToString(), name });
                long inserted_id = Database.Instance.InsertQuery(sql);

                // get id and LastEdited
                var fields = new string[] { ID_COL_NAME, "created" };
                sql = SQLQueryBuilder.SelectFieldsWhereFieldEquals(TABLE_NAME, "id", inserted_id.ToString(), fields);
                row = Database.Instance.FetchRow(sql);
                long id = (long)row[0];
                ID = (int)id;
            }
            // LOCK ENDS HERE

            // set id and lastedited
            routeNode.ID = (int)ID;
            routeNode.LastEdited = (DateTime)row[1];

            Logger.WriteLine("Created routeNode: " + routeNode);
        }

        public override int GetId(RouteNode routeNode)
        {
            long id = 0;

            string sql;

            var internationalPort = routeNode as InternationalPort;
            var distributionCentre = routeNode as DistributionCentre;


            if (internationalPort != null)
            {
                sql = SQLQueryBuilder.SelectFieldsWhereFieldLike(TABLE_NAME, "country_id", internationalPort.Country.ID.ToString(), new[] { ID_COL_NAME });
            }
            else
            {
                sql = SQLQueryBuilder.SelectFieldsWhereFieldLike(TABLE_NAME, "name", distributionCentre.Name, new[] { ID_COL_NAME });
            }

            //LOCK BEGINS HERE
            lock (Database.Instance)
            {
                id = Database.Instance.FetchNumberQuery(sql);
            }
            //LOCK ENDS HERE

            // set id in country
            routeNode.ID = (int)id;

            // return result
            return routeNode.ID;
        }
    }
}
