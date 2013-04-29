
using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Server.Data;

namespace Server.Business
{
    public class CountryService: Service<Country>
    {
        public CountryService(CurrentState state): base(state, new CountryDataHelper())
        {
            if (!state.CountriesInitialised)
            {
                // initialise the countries of the state
                var countries = dataHelper.LoadAll();
                state.InitialiseCountries(countries);
            }
        }

        /// <summary>
        /// Creates a new country with the given name and code.
        /// </summary>
        /// <param name="name">Country name</param>
        /// <param name="code">3 letter code</param>
        /// <returns>the created object, with ID field, and LastEdited initialised</returns>
        /// <exception cref="DatabaseException"></exception>
        /// <exception cref="InvalidObjectStateException"></exception>
        public Country Create(string name, string code) 
        {
            // throws an exception if invalid
            var newCountry = new Country {Name = name, Code = code};

            // throws a database exception if invalid
            dataHelper.Create(newCountry);

            // update state
            state.SaveCountry(newCountry);
            state.IncrementNumberOfEvents();

            return newCountry;
        }

        /// <summary>
        /// Updates the country with the given ID to have the given code.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        ///<exception cref="DatabaseException">if</exception>
        /// <exception cref="InvalidObjectStateException"></exception>
        public Country Update(int id, string code)
        {
            // get the name of the country
            var country = state.GetCountry(id);
            if(country == null)
                throw new ArgumentException("No country with that ID was found: " + id, "id");
            
            // see if anything changed
            if (country.Code.Equals(code))
                throw new NoChangeException();

            // throws an exception if invalid
            var newCountry = new Country { ID=id, Name = country.Name, Code = code };

            // throws a database exception if invalid
            dataHelper.Update(newCountry);

            // save to state
            state.SaveCountry(newCountry);
            state.IncrementNumberOfEvents();

            // return the country
            return newCountry;
        }

        public override Country Get(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id cannot be less than or equal to 0");
            }

            return state.GetCountry(id);
        }

        public override IEnumerable<Country> GetAll()
        {
            return state.GetAllCountries();
        }

        /// <summary>
        /// Checks whether a country with the same name is currently active.
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public override bool Exists(Country country)
        {
            var countries = state.GetAllCountries().AsQueryable();

            return countries.Any(t => t.Equals(country));
        }

        public override void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id cannot be less than or equal to 0");
            }

            var country = state.GetCountry(id);

            // check the country isn't used in any routeNodes
            var routeNodes = state.GetAllRouteNodes();
            bool isUsed = routeNodes.AsQueryable().Any(t => t.Country.Equals(country));
            if (isUsed)
            {
                throw new IllegalActionException("Cannot remove country that is used in an active RouteNode.");
            }

            // remove from db
            dataHelper.Delete(id);

            // remove from state     
            state.RemoveCountry(id);
            state.IncrementNumberOfEvents();
        }
    }
}
