using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQuarterlyLoanStatement : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        int SelectedOriginationSource {get;}

        /// <summary>
        /// 
        /// </summary>
        int SelectedResetConfiguration { get;}

        /// <summary>
        /// 
        /// </summary>
        int SelectedStatementMonths { get;}

        /// <summary>
        /// 
        /// </summary>
        string SelectedSamples { get;}

        /// <summary>
        /// 
        /// </summary>
        string SelectedMailingAddresses { get;}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="osList"></param>
        void BindOriginationSource(IList<IOriginationSource> osList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetList"></param>
        void BindResetConfiguration(IList<IResetConfiguration> resetList);



    }
}
