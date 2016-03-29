using BuildingBlocks.Assertions;
using BuildingBlocks.Services.Contracts;
using NUnit.Framework;
using ObjectMaps.Presenters.PersonalLoans;
using System;

namespace BuildingBlocks.Presenters.PersonalLoans
{
    public class PersonalLoanLead : PersonalLoanLeadControls
    {
        private static IWatiNService watinService;

        public PersonalLoanLead()
        {
            watinService = ServiceLocator.Instance.GetService<IWatiNService>();
        }
    }
}