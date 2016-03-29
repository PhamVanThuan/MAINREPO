using System;
using System.Collections.Generic;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Capitec.Managers.Application.Models;

namespace SAHL.Services.Capitec.Managers.Application
{
    public interface IApplicationDataManager
    {
        void AddApplication(Guid applicationID, DateTime dateTime, int applicationNumber, Guid applicationPurposeEnumId, Guid userId, DateTime captureStartTime, Guid branchId);

        int GetNextApplicationNumber();

        void AddApplicationLoanDetail(Guid applicationID, Guid applicationLoanDetailID, Guid employmentTypeID, Guid occupancyTypeEnumID, decimal householdIncome, decimal instalment, decimal interestRate, decimal loanAmount, decimal ltv, decimal pti, decimal fees, int termInMonths, bool capitaliseFees);

        void AddNewPurchaseApplicationLoanDetail(Guid applicationLoanDetailID, decimal purchasePrice, decimal deposit);

        void AddSwitchApplicationLoanDetail(Guid applicationLoanDetailID, decimal cashRequired, decimal currentBalance, decimal estimatedMarketValueOfTheHome, decimal interimInterest);

        List<ApplicantModel> GetApplicantsForApplication(Guid applicationID);

        ApplicationDataModel GetApplicationByID(Guid guid);

        void SetApplicationStatus(Guid applicationID, Guid applicationStatusID, DateTime statusChangeDate);

        bool DoesApplicationExist(Guid applicationID);

        void UpdateCaptureEndTime(Guid applicationID, DateTime captureEndTime);
    }
}