namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IFurtherAdvanceApplicationInformation : IApplicationInformation, ISupportsQuickCashApplicationInformation
    {
        /// <summary>
        ///
        /// </summary>
        //IApplicationInformationQuickCash ApplicationInformationQuickCash
        //{
        //    get;
        //    set;
        //}

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