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
                //var companies = dataHelper.LoadAll();
                var companies = new Dictionary<int, Company>();
                state.InitialiseCompanies(companies);
            }
        }

        public override Company Get(int id)
        {
            if (id == 0)
            {
                throw new IllegalActionException("id cannot be 0");
            }

            return state.GetCompany(id);
        }

        public override IEnumerable<Company> GetAll()
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
            throw new NotImplementedException();
        }
    }
}
