using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Server.Data;

namespace Server.Business
{
    public class CompanyService
    {
        private CurrentState state;
        private DataHelper<Company> dataHelper = new CompanyDataHelper();


        public CompanyService(CurrentState state)
        {
            this.state = state;

            // initialise the companies of the state     
            //var companies = dataHelper.LoadAll();
            var companies = new Dictionary<int, Company>(); 
            state.InitialiseCompanies(companies);
        }

    }
}
