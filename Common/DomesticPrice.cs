using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// This class represents a price for a domestic route.
    /// </summary>
    public class DomesticPrice: Price
    {

        public DomesticPrice (Priority priority) {}


        public override string ToString ()
        {
            return String.Format("DomesticPrice[ID={0}, Priority={1}, PricePerGram={2}, PricePerCm3={3}]", ID, Priority.ToString(), PricePerGram, PricePerCm3);
        }

    }
}
