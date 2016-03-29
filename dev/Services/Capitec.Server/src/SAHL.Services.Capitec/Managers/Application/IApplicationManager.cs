using System;
using System.Collections.Generic;
using SAHL.Core.SystemMessages;
using SAHL.Services.Capitec.Managers.Application.Models;

namespace SAHL.Services.Capitec.Managers.Application
{
    public interface IApplicationManager
    {
        //ISystemMessageCollection CreateApplicantForApplication(Guid applicantID, SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant applicant);

        void AddLoanApplication(int applicationNumber, Guid applicationID, LoanApplication application);

        bool PerformITCs(Dictionary<Guid, SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant> applicants);

        bool RecalculateSwitchApplication(LoanApplication switchLoanApplication, Dictionary<Guid, SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant> addedApplicants, bool eligibleBorrower, Guid applicationID);

        bool RecalculateNewPurchaseApplication(LoanApplication application, Dictionary<Guid, SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant> applicants, bool eligibleBorrower, Guid applicationID);

        List<ApplicantModel> GetApplicantsForApplication(Guid applicationID);

        int GetApplicationNumberForApplication(Guid guid);

        void SetApplicationToDeclined(Guid applicationID);

        ApplicationLoanDetails GetRecalculatedLoanDetailsForApplication(LoanApplication application);

        CalculatedLoanDetailsModel CalculateLoanDetailForApplication(ApplicationLoanDetails loanDetails, List<SAHL.Services.Interfaces.Capitec.ViewModels.Apply.Applicant> applicants, bool eligibleBorrower, Guid applicationID);

        bool DoesApplicationExist(Guid applicationID);

        void UpdateCaptureEndTime(Guid applicationID, DateTime captureEndTime);
    }
}