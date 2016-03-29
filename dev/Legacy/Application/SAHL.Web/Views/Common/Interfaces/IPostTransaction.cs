using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPostTransaction : IViewBase
    {

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnPostButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler onSelectedTransctionTypeChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TransactionTypes"></param>
        void BindTransactionTypes(IReadOnlyEventList<ITransactionType> TransactionTypes);

        void BindFinancialServiceTypes(Dictionary<string, string> dictFinancialServices);


        #region Properties

        /// <summary>
        /// The selected type of the transaction
        /// </summary>
        int TransactionType
        {
            get;
        }

        /// <summary>
        /// The date the transaction is valid for
        /// </summary>
        DateTime EffectiveDate
        {
            get;
        }

        /// <summary>
        /// The amount of the transaction
        /// </summary>
        double Amount
        {
            get;
        }

        /// <summary>
        /// Text reference for the transaction
        /// </summary>
        string Reference
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        int SelectedFinancialServiceKey { get; }

        /// <summary>
        /// 
        /// </summary>
        bool FinancialServicesVisible{set;}

        #endregion

    }
}
