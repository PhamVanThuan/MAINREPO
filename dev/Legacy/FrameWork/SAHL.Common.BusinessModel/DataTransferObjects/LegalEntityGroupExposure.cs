using System.Collections.Generic;
using System.Linq;

namespace SAHL.Common.BusinessModel.DataTransferObjects
{
    public class LegalEntityGroupExposure 
    {
        public List<GroupExposureItem> GroupExposureItems { get; protected set; }

        public LegalEntityGroupExposure()
        {
            GroupExposureItems = new List<GroupExposureItem>();
        }

        public double TotalCurrentBalance
        {
            get { return GroupExposureItems.Sum(ge => ge.CurrentBalance); }
        }

        public double TotalArrearBalance
        {
            get { return GroupExposureItems.Sum(ge => ge.ArrearBalance); }
        }

        public double TotalLoanAgreementAmount
        {
            get { return GroupExposureItems.Sum(ge => ge.LoanAgreementAmount); }
        }

        public double TotalLatestValuation
        {
            get { return GroupExposureItems.GroupBy(item => item.PropertyKey).Select(grp => grp.First()).Sum(ge => ge.LatestValuationAmount); }
        }

        public double TotalLoanInstalment
        {
            get { return GroupExposureItems.Sum(ge => ge.InstalmentAmount); }
        }

        public double TotalHouseholdIncome
        {
            // Sum of the column will be meaningless as each application has a total household income
            get { return 0; }
        }
    }
}