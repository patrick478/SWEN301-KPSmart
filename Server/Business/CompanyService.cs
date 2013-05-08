using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Data;

namespace Server.Business
{
    public class CompanyService: Service<Company>
    {

        public CompanyService(CurrentState state) : base(state, new CompanyDataHelper())
        {
            if (!state.CompaniesInitialised)
            {
                // initialise the companies of the state     
                var companies = dataHelper.LoadAll();
                state.InitialiseCompanies(companies);
            }
        }

        /// <summary>
        /// Creates a new company with the given name.
        /// </summary>
        /// <param name="name">Company name</param>
        /// <returns>the created object, with ID field, and LastEdited initialised</returns>
        /// <exception cref="DatabaseException">if a company with the same name already exists</exception>
        /// <exception cref="InvalidObjectStateException">if the name is illegal</exception>
        public Company Create(string name)
        {
            // throws an exception if invalid
            var newCompany = new Company{Name = name};

            // throws a database exception if invalid
            dataHelper.Create(newCompany);

            // update state
            state.SaveCompany(newCompany);
            state.IncrementNumberOfEvents();

            return newCompany;
        }

        public override Company Get(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id cannot be less than or equal to 0");
            }

            return state.GetCompany(id);
        }

        public override IList<Company> GetAll()
        {
            return state.GetAllCompanies();
        }

        public override bool Exists(Company company)
        {
            var companies = state.GetAllCompanies().AsQueryable();

            return companies.Any(t => t.Name.ToLower() == company.Name.ToLower());
        }

        public override void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id cannot be less than or equal to 0");
            }

            var company = state.GetCompany(id);

            // check the company doesn't offer any routes.
            var routes = state.GetAllRoutes();
            bool isUsed = routes.AsQueryable().Any(t => t.Company.Equals(company));
            if (isUsed)
            {
                throw new IllegalActionException("Cannot remove a company that currently offers routes.");
            }

            // remove from db
            dataHelper.Delete(id);

            // remove from state     
            state.RemoveCompany(id);
            state.IncrementNumberOfEvents();
        }
    }
}
