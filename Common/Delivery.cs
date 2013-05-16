//////////////////////
// Original Writer: Isabel Broome-Nicholson
// Reviewed by: 
//
// Edited by Adam: Set the class to public.
// Reviewed by: 
//////////////////////

using System;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// Represents an instance of a delivery.  It must contain at least one Route, and contained routes must be contiguous.
    /// 
    /// TODO: decide whether public get/set, or private.  
    /// </summary>
    public class Delivery: DataObject
    {

        public RouteNode Origin { get; set; }
        public RouteNode Destination { get; set; }

        // Standard or Air
        public Priority Priority { get; set; }

        // Domestic or International
        public Scope Scope 
        {
            get
            {
                if (Origin.GetType() == typeof(InternationalPort) || Destination.GetType() == typeof(InternationalPort))
                    return Scope.International;
                else
                    return Scope.Domestic;
            }
        }

        // the routes included in the delivery
        public IList<RouteInstance> Routes { get; set; }

        public int WeightInGrams { get; set; }

        public int VolumeInCm3 { get; set; }

        // total price charged to the customer
        public int TotalPrice { get; set; }

        // total cost paid to the delivery companies
        public int TotalCost { get; set; }

        // the time the delivery was requested
        public DateTime TimeOfRequest { get; set; }

        // the time the delivery arrived at its destination
        public DateTime TimeOfDelivery { get; set; }

        // Eg. 'Domestic Air', 'International Standard'
        public string CustomerFriendlyPriority
        {
            get { return Scope + " " + Priority; }
        }

        // the duration of the delivery
        public TimeSpan Duration
        {
            get { return TimeOfDelivery.Subtract(TimeOfRequest); }
        }

    }
}
