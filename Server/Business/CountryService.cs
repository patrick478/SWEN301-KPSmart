﻿
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

        public Country Update(int id, string name, string code)
        {
            // throws an exception if invalid
            var newCountry = new Country { ID=id, Name = name, Code = code };

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
            if (id == 0)
            {
                throw new IllegalActionException("id cannot be 0");
            }

            return state.GetCountry(id);
        }

        public override IEnumerable<Country> GetAll()
        {
            return state.GetAllCountries();
        }

        /// <summary>
        /// Checks whether the country exists or not.  Returns true if a country with the same name is 
        /// active.
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public override bool Exists(Country country)
        {
            var countries = state.GetAllCountries().AsQueryable();

            return countries.Any(t => t.Name.ToLower() == country.Name.ToLower());
        }

        public override void Delete(int id)
        {
            if (id == 0)
            {
                throw new IllegalActionException("id cannot be 0");
            }

            // check the country isn't used in any routeNodes
            var routeNodes = state.GetAllRouteNodes();
            bool isUsed = routeNodes.AsQueryable().Any(t => t.Country.ID == id);
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
