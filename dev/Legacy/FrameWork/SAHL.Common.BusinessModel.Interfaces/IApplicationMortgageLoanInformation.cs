using System;
using System.Collections.Generic;
using System.Text;

namespace SAHL.Common.BusinessModel.Interfaces
{
    public partial interface IApplicationInformationMortgageLoan : IApplicationInformation
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

    }
}
