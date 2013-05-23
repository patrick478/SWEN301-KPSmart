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

        public override string ToNetString()
        {
            return NetCodes.BuildObjectNetString(base.ToNetString(), Priority.ToNetString(), Convert.ToString(PricePerGram), Convert.ToString(PricePerCm3));
        }

        public static DomesticPrice ParseNetString(string objectDef)
        {
            string[] tokens = objectDef.Split(NetCodes.SEPARATOR_FIELD);
            int count = 0;
            int id = Convert.ToInt32(tokens[count++]);
            Priority priority = PriorityExtensions.ParseNetString(tokens[count++]);
            int weightPrice = Convert.ToInt32(tokens[count++]);
            int volumePrice = Convert.ToInt32(tokens[count++]);
            return new DomesticPrice(priority) { ID = id, PricePerGram = weightPrice, PricePerCm3 = volumePrice };
        }

    }
}
