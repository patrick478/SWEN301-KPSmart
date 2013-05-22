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

        private RouteNode origin;
        public RouteNode Origin
        {
            get { return origin; }
            set
            {
                // validation
                if (value == null)
                    throw new InvalidObjectStateException("Origin", "Origin cannot be set to null.");

                if (value.Equals(Destination))
                    throw new InvalidObjectStateException("Origin", "Origin cannot be the same as the Destination.");

                this.origin = value;
            }
        }

        private RouteNode destination;
        public RouteNode Destination
        {
            get { return destination; }
            set
            {
                // validation
                if (value == null)
                    throw new InvalidObjectStateException("Destination", "Destination cannot be set to null.");

                if (value.Equals(Origin))
                    throw new InvalidObjectStateException("Destination", "Destination cannot be the same as the Origin.");

                this.destination = value;
            }
        }




        // Standard or Air
        private Priority priority;
        public Priority Priority
        {
            get { return priority; }
            set
            {
                // validation
                if (value == null)
                    throw new InvalidObjectStateException("Priority", "Priority cannot be set to null.");

                this.priority = value;
            }
        } 

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

        // the routes included in the delivery. Note, only to be used by Josh's pathfinding.
        public List<RouteInstance> Routes { get; set; }




        private int weightInGrams;
        public int WeightInGrams
        {
            get { return weightInGrams; }
            set
            {
                // validation
                if (value <= 0)
                    throw new InvalidObjectStateException("WeightInGrams", "WeightInGrams cannot be less than or equal to 0.");

                this.weightInGrams = value;
            }
        }

        private int volumeInCm3;
        public int VolumeInCm3
        {
            get { return volumeInCm3; }
            set
            {
                // validation
                if (value <= 0)
                    throw new InvalidObjectStateException("VolumeInCm3", "VolumeInCm3 cannot be less than or equal to 0.");

                this.volumeInCm3 = value;
            }
        }

        // total price charged to the customer
        private int totalPrice;
        public int TotalPrice
        {
            get { return totalPrice; }
            set
            {
                // validation
                if (value <= 0)
                    throw new InvalidObjectStateException("TotalPrice", "TotalPrice cannot be less than or equal to 0.");

                this.totalPrice = value;
            }
        }

        // total cost paid to the delivery companies
        private int totalCost;
        public int TotalCost
        {
            get { return totalCost; }
            set
            {
                // validation
                if (value <= 0)
                    throw new InvalidObjectStateException("TotalCost", "TotalCost cannot be less than or equal to 0.");

                this.totalCost = value;
            }
        }

        // the time the delivery was requested
        private DateTime timeOfRequest;
        public DateTime TimeOfRequest
        {
            get { return timeOfRequest; }
            set
            {
                // validation
                if (value == null)
                    throw new InvalidObjectStateException("TimeOfRequest", "TimeOfRequest cannot be set to null.");

                if (TimeOfDelivery.Ticks != 0 && value >= TimeOfDelivery)
                    throw new InvalidObjectStateException("TimeOfRequest", "TimeOfRequest cannot be equal or after the TimeOfDelivery");

                this.timeOfRequest = value;
            }
        }

        // the time the delivery arrived at its destination
        private DateTime timeOfDelivery;
        public DateTime TimeOfDelivery
        {
            get { return timeOfDelivery; }
            set
            {
                // validation
                if (value == null)
                    throw new InvalidObjectStateException("TimeOfDelivery", "TimeOfDelivery cannot be set to null.");

                if (TimeOfRequest.Ticks != 0 && value <= TimeOfRequest )
                    throw new InvalidObjectStateException("TimeOfDelivery", "TimeOfDelivery cannot be the same as or before the TimeOfRequest.");

                this.timeOfDelivery = value;
            }
        }

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


        public override string ToString ()
        {
            return String.Format("Delivery[ID:{0}, Origin:{1}, Destination:{2}, Priority:{3}, TimeOfRequest:{4}, TimeOfDelivery:{5}, TotalPrice:{6}, TotalCost:{7}, WeightInGrams:{8}, VolumeInCm3:{9}, LastEdited:{10}]", ID, Origin.ToShortString(), Destination.ToShortString(), Priority.ToString(), TimeOfRequest.ToString(), TimeOfDelivery.ToString(), TotalPrice, TotalCost, WeightInGrams, VolumeInCm3, LastEdited );
        }

    }
}
