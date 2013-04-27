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
            // initialise the companies of the state     
            //var companies = dataHelper.LoadAll();
            var companies = new Dictionary<int, Company>(); 
            state.InitialiseCompanies(companies);
        }

        public override Company Get(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Company> GetAll()
        {
            throw new NotImplementedException();
        }

        public override bool Exists(Company obj)
        {
            throw new NotImplementedException();
        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
