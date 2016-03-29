using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IApplicationMortgageLoan : IEntityValidation, IApplication
    {
        /// <summary>
        ///
        /// </summary>
        System.Double MinBondRequired
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IApplicantType ApplicantType
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Int32? NumApplicants
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        System.String TransferringAttorney
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IResetConfiguration ResetConfiguration
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Double? ClientEstimatePropertyValuation
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IProperty Property { get; set; }

        ///// <summary>
        ///// The greatest amount of income (for all legal entities playing a role in an account/application)
        ///// </summary>
        ////EmploymentTypes GetEmploymentType(bool UseAllCurrentEmployment);

        /// <summary>
        /// Gets the list of Margins (Link Rates) that an application is allowed to have.
        /// </summary>
        /// <returns></returns>
        IEventList<IMargin> GetMargins();

        /// <summary>
        /// Returns the Total Loan required depending on the Type of the Application.
        /// </summary>
        double? LoanAgreementAmount { get; }

        /// <summary>
        ///
        /// </summary>
        Int32? DependentsPerHousehold
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        Int32? ContributingDependents
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        ILanguage Language
        {
            get;
            set;
        }

        /// <summary>
        /// returns the Active Estate Agent roles LegalEntity and related
        /// organisation structures at the time the role was assigned
        /// </summary>
        /// <param name="EstateAgent">The Active Estate Agent Role's LE</param>
        /// <param name="Company">The Active Estate Agents Company when the role was created</param>
        /// <param name="Branch">The Active Estate Agents Branch when the role was created</param>
        /// <param name="Principal">The Active Estate Agents Principal when the role was created</param>
        void GetEsateAgentDetails(out ILegalEntity EstateAgent, out ILegalEntity Company, out ILegalEntity Branch, out ILegalEntity Principal);

        /*
         *
        /// <summary>
        ///
        /// </summary>
        IEventList<IMortgageLoanApplicationInformation> MortgageLoanApplicationInformations
        {
            get;
        }

        /// <summary>
        ///
        /// </summary>
        IMortgageLoanApplicationInformation CurrentMortgageLoanApplicationInformation
        {
            get;
        }

        /// <summary>
        /// This method creates a new, empty class, adds it to the MortgageLoanApplicationInformations Collection and returns the new object to the caller.
        /// </summary>
        /// <param name="Messages"></param>
        /// <returns></returns>
        IMortgageLoanApplicationInformation CreateEmptyApplicationInformation(IDomainMessageCollection Messages);

        */
    }
}