using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Managers.Applicant
{
    public interface IApplicantManager
    {
        void AddIncomeContributorOfferRoleAttribute(int offerRoleKey);

        bool IsApplicantAnIncomeContributor(int applicationRoleKey);
    }
}
