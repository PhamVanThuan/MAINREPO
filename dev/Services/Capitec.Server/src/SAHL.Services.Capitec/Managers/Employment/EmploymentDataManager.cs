using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Identity;
using System;

namespace SAHL.Services.Capitec.Managers.Employment
{
    public class EmploymentDataManager : IEmploymentDataManager
    {
        private IDbFactory dbFactory;
        public EmploymentDataManager(IDbFactory dbFactory) 
        {
            this.dbFactory = dbFactory;
        }

        public void AddApplicantEmployment(Guid applicantID, Guid employmentTypeEnumId, decimal basicMonthlyIncome, decimal threeMonthAverageCommission, decimal housingAllowance)
        {
            Guid applicantEmploymentId = CombGuid.Instance.Generate();
            ApplicantEmploymentDataModel applicantEmployment = new ApplicantEmploymentDataModel(applicantEmploymentId, applicantID, employmentTypeEnumId, basicMonthlyIncome, threeMonthAverageCommission, housingAllowance);

            using (var db = this.dbFactory.NewDb().InAppContext())
            {
                db.Insert(applicantEmployment);
                db.Complete();
            }
        }
    }
}