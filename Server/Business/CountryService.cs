
using System.Collections.Generic;
using Common;
using Server.Data;

namespace Server.Business
{
    public class CountryService: Service
    {

        private CurrentState state;
        private DataHelper<Country> dataHelper;


        public CountryService(CurrentState state, DataHelper<Country> dataHelper)
        {
            this.state = state;
            this.dataHelper = dataHelper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Country name</param>
        /// <param name="code">3 letter code</param>
        /// <returns>the created object, with ID field, and LastEdited initialised</returns>
        /// <exception cref="DatabaseException"></exception>
        /// <exception cref="InvalidObjectStateException"></exception>
        public Country CreateCountry (string name, string code) 
        {
            // throws an exception if invalid
            var newCountry = new Country {Name = name, Code = code};

            // throws a database exception if invalid
            dataHelper.Create(newCountry);

            state.SaveCountry(newCountry);

            // does it return the country, or 
            return newCountry;
        }


        public Country LoadCountry(string name)
        {
            //todo
            return null;
        }

        public void DeleteCountry(int id)
        {
            // check the country isn't used in any routes

            // remove from db

            // remove from state         

        }


        public Country EditCountry(int id, string code)
        {
            //todo
            return null;
        }


        public IEnumerable<Country> GetAllCountries()
        {
            //todo
            return null;
        }





    }
}
