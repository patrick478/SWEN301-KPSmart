using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServerTests
{
    [TestClass]
    public class IzziTests
    {


        [TestMethod()]
        public void IzziTest()
        {
            List<Company> times = new List<Company> { new Company { Name = "New Zealand" } };


            IEnumerable<Company> iqueryable = new List<Company>(times);

            var country = iqueryable.ElementAt(0);
            country.Name = "piggie";

            Console.WriteLine(times.ElementAt(0).Name);


        }


    }
}
