using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces.Repositories
{
    public interface IApplicationUnsecuredLendingRepository
    {
        IResult CalculateUnsecuredLending(double amount, List<int> terms, bool creditLifePolicySelected);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationUnsecuredLending GetEmptyApplicationUnsecuredLending();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IApplicationInformationPersonalLoan GetEmptyApplicationInformationPersonalLoan();

        /// <summary>
        ///
        /// </summary>
        /// <param name="applicationInformationPersonalLoan"></param>
        void CreateApplicationInformationPersonalLoan(IApplicationInformationPersonalLoan applicationInformationPersonalLoan);

        /// <summary>
        /// Gets summary from ApplicationInformationPersonalLoan_DAO by offer key.
        /// </summary>
        /// <param name="genericKey">The Offer Key.</param>
        /// <returns>ApplicationInformationPersonalLoan List.</returns>
        IReadOnlyEventList<IApplicationInformationPersonalLoan> GetApplicationInformationPersonalLoanSummaryByKey(int genericKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        IApplicationUnsecuredLending GetApplicationByKey(int Key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="application"></param>
        /// <param name="creditLifePolicy"></param>
        /// <param name="result"></param>
        /// <param name="selectedPersonalLoanOption"></param>
        /// <returns></returns>
        IApplicationUnsecuredLending SetupPersonalLoanApplication(IDomainMessageCollection messages, IApplication application, bool creditLifePolicy, IResult result, ICalculatedItem selectedPersonalLoanOption);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userID"></param>
        void CreateAndOpenPersonalLoan(int accountKey, string userID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="userID"></param>
        void DisbursePersonalLoan(int accountKey, string userID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountKey"></param>
        void ReturnDisbursedPersonalLoanToApplication(int accountKey);
        /// <summary>
      /// 
      /// </summary>
      /// <param name="legalEntityIDNumber"></param>
      /// <returns>LeadCreated</returns>
        bool CreatePersonalLoanLead(string legalEntityIDNumber);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IExternalLifePolicy GetEmptyExternalLifePolicy();
        /// <summary>
        /// Checks Life Premium to determine if SAHL Life is applied
        /// </summary>
        /// <param name="applicationKey"></param>
        /// <returns></returns>
        bool ApplicationHasSAHLLifeApplied(int applicationKey);
    }
}