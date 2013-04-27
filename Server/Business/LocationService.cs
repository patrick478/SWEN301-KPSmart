using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Data;

namespace Server.Business
{
    public class LocationService: Service<RouteNode>
    {
        public LocationService(CurrentState state) : base(state, new RouteNodeDataHelper())
        {
            // initialise current routeNodes
            //var routeNodes = dataHelper.LoadAll();
            var routeNodes = new Dictionary<int, RouteNode>();
            state.InitialiseRouteNodes(routeNodes);
        }

        public override RouteNode Get(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<RouteNode> GetAll()
        {
            throw new NotImplementedException();
        }

        public override bool Exists(RouteNode obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
