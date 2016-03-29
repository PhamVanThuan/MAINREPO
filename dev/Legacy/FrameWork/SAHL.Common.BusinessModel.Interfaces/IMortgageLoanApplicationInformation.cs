namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IMortgageLoanApplicationInformation : IApplicationInformation
    {
        /// <summary>
        ///
        /// </summary>
        IApplicationInformationQuickCash ApplicationInformationQuickCash
        {
            get;
            set;
        }

        /// <summary>
        /// 'Interest Only' information on an application
        /// </summary>
        IApplicationInformationInterestOnly ApplicationInformationInterestOnly
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IApplicationInformationVariableLoan ApplicationInformationVariableLoan
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IApplicationInformationVarifixLoan ApplicationInformationVarifixLoan
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IApplicationInformationSuperLoLoan ApplicationInformationSuperLoLoan
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        IApplicationInformationEdge ApplicationInformationEdge
        {
            get;
            set;
        }
    }
}