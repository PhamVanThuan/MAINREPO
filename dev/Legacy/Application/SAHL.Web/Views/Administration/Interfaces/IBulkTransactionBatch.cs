using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;

namespace SAHL.Web.Views.Administration.Interfaces
{
    public interface IBulkTransactionBatch : IViewBase
    {
        #region eventhandlers

        /// <summary>
        /// SelectedIndexChange event on BatchStatus DropDown
        /// </summary>
        event KeyChangedEventHandler OnBatchStatusSelectedIndexChange;
        /// <summary>
        /// SelectedIndexChange event on BatchNumber DropDown
        /// </summary>
        event KeyChangedEventHandler OnBatchNumberSelectedIndexChange;
        /// <summary>
        /// SelectedIndexChange event on SubsidyType DropDown
        /// </summary>
        event KeyChangedEventHandler OnSubsidyTypeSelectedIndexChange;
        /// <summary>
        /// Selected Index Change of Transaction Type Drop Down
        /// </summary>
        event KeyChangedEventHandler OnTransactionStatusSelectedIndexChange;
        
        /// <summary>
        /// Selected Index Change of Parent SusbidyProvider Drop Down
        /// </summary>
        event KeyChangedEventHandler OnParentSubsidySelectedIndexChange;
        /// <summary>
        /// Click event of Next Button - used for Paging on Grid
        /// </summary>
        event EventHandler onNextButtonClicked;
        /// <summary>
        /// Click event of Previous Button - used for Paging on Grid
        /// </summary>
        event EventHandler onPrevButtonClicked;
        /// <summary>
        /// EventHandler for Delete Option
        /// </summary>
        event KeyChangedEventHandler onDeleteButtonClicked;
        /// <summary>
        /// EventHandler for Find Option - used to find records on Grid
        /// </summary>
        event KeyChangedEventHandler onFindButtonClicked;
        /// <summary>
        /// Selected Index Change event on Grid
        /// </summary>
        event KeyChangedEventHandler OnBulkGridSelectedIndexChange;
        /// <summary>
        /// EventHandler for Update Presenter
        /// </summary>
        event EventHandler OnUpdateButtonClicked;

        event KeyChangedEventHandler OnTransactionTypeSelectedIndexChange;

        event EventHandler OnSaveButtonClicked;

        event EventHandler OnPostButtonClicked;

        event EventHandler OnCancelButtonClicked;

        event EventHandler OnRemoveButtonClicked;
        
        #endregion
        void BindTransactionTypes(IReadOnlyEventList<ITransactionType> transactionTypes);
        /// <summary>
        /// Bind BatchStatus Drop Down
        /// </summary>
        /// <param name="batchStatus"></param>
        void BindBatchStatus(IEventList<IBulkBatchStatus> batchStatus);
        /// <summary>
        /// Bind BatchNumber Drop Down
        /// </summary>
        /// <param name="bulkBatch"></param>
        void BindBatchNumber(IList<IBulkBatch> bulkBatch);
        /// <summary>
        /// Bind the Status Drop Down for Add Option
        /// </summary>
        void BindStatusDropDownforAdd();
        /// <summary>
        /// Sets the controls for Display Mode
        /// </summary>
        void SetControlsForDisplay();
        /// <summary>
        /// Sets the controls for Display Mode
        /// </summary>
        void SetControlsForAdd();
        /// <summary>
        /// Sets the controls for Update Mode
        /// </summary>
        void SetControlsForUpdate();
        /// <summary>
        /// Sets the controls for Delete Mode
        /// </summary>
        void SetControlsForDelete();
        /// <summary>
        /// Enables or Disables Next and Previous buttons , depending on whether grid is populated
        /// </summary>
        bool SetButtonsNextAndPrevious { set; }
        /// <summary>
        /// Sets buttons for Update Mode
        /// </summary>
        bool SetUpdateButtons { set; }
        /// <summary>
        /// Set Buttons for Add Mode
        /// </summary>
        bool SetAddUpdateControls { set;}

        bool DisableAllDropDownsForAdd { set; } 
        /// <summary>
        /// Bind Transaction Status
        /// </summary>
        /// <param name="batchTransactionStatus"></param>
        void BindTransactionStatus(IEventList<IBatchTransactionStatus> batchTransactionStatus);
        /// <summary>
        /// Bind Grid for display and Delete Modes
        /// </summary>
        /// <param name="action"></param>
        /// <param name="pageNumber"></param>
        /// <param name="totalPages"></param>
        /// <param name="totalNumber"></param>
        /// <param name="batchTransaction"></param>
        void BindBulkGridDisplay(string action, int pageNumber, int totalPages, int totalNumber, DataView batchTransaction);
        /// <summary>
        /// Bind Grid for Add Mode
        /// </summary>
        /// <param name="action"></param>
        /// <param name="selectedGridIndex"></param>
        /// <param name="pageNumber"></param>
        /// <param name="totalPages"></param>
        /// <param name="totalNumber"></param>
        /// <param name="batchTransaction"></param>
        void BindBulkGridAdd(string action,int selectedGridIndex , int pageNumber, int totalPages, int totalNumber, DataView batchTransaction);
        /// <summary>
        /// Binds BatchFields depending on Batch Number selected - called in Display and Delete Modes
        /// </summary>
        /// <param name="bulkBatch"></param>
        /// <param name="subsidyProvider"></param>
        /// <param name="transactionType"></param>
        void BindBatchFields(IBulkBatch bulkBatch, string subsidyProvider, string transactionType);
        /// <summary>
        /// Bind SubsidyTypes drop down
        /// </summary>
        /// <param name="subsidyProviderType"></param>
        void BindSubsidyTypes(IEventList<ISubsidyProviderType> subsidyProviderType);
        /// <summary>
        /// Bind Parent Susbidy Drop Down
        /// </summary>
        /// <param name="subsidyProvider"></param>
        void BindParentSubsidyProvider(IList<SAHL.Common.BusinessModel.Interfaces.ISubsidyProvider> subsidyProvider);
        /// <summary>
        /// Binds Status Drop Down for Update Mode
        /// </summary>
        /// <param name="batchStatus"></param>
        void BindBatchStatusForUpdate(IEventList<IBulkBatchStatus> batchStatus);
        /// <summary>
        /// Set PostBack Type on Grid - called on Update and Add Mode
        /// </summary>
        void SetPostBackType();
        /// <summary>
        /// Gets the selected Index on Grid after Binding - called on Update and Add modes
        /// </summary>
        /// <param name="selectedGridIndex"></param>
        void SetSelectedIndexOnGrid(int selectedGridIndex);
        /// <summary>
        /// Sets the No Match Found error message when using find button grid
        /// </summary>
        /// <param name="errorMessage"></param>
        void SetErrorMessage(string errorMessage);

        /// <summary>
        /// Gets/sets the effective date.
        /// </summary>
        DateTime? EffectiveDate { get; set; }

        void SetMesssageBoxForAdd(string batchNumber);

        void RestoreDropDownValues(int subsidyProviderKeySelected, int transactionTypeSelected, int subsidyProviderTypeKey, DateTime effectiveDate);

        void SetBatchPostErrorMessage(string errorMessage);

        /// <summary>
        /// Gets/sets the captured transaction amount.
        /// </summary>
        double? TransactionAmount { get; set; }
    }
}
