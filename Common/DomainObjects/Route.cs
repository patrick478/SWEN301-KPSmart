namespace Common.DomainObjects
{
    public class Route
    {

        public Route(Company company, TransportType transportType, Destination origin, Destination destination)
        {
            Company = company;
            TransportType = transportType;
            Origin = origin;
            Destination = destination;
        }

        // fields that determine uniqueness of the Route
        //----------------------------------------------
        public TransportType TransportType { get; private set; }
        public Company Company { get; private set; }
        public Destination Origin { get; private set; }
        public Destination Destination { get; private set; }


        // other fields
        //-------------

        // domestic or international
        public Scope Scope { get; private set; }

        //in hours  (should this be in minutes?)
        public int Duration { get; private set; }

        // in grams
        public int MaxWeight { get; private set; }

        // In cubic cm
        public int MaxVolume { get; private set; }

        // in cents  (should this be a double?)
        public int CostPerGram { get; private set; }

        // in cents  (should this be a double?)
        public int CostPerCm3 { get; private set; }


    }
}
