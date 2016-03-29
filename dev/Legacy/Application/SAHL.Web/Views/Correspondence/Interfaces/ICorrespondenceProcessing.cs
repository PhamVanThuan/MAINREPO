using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Reports.Interfaces;
using SAHL.Common.BusinessModel.CorrespondenceGeneration.CorrespondenceBuilders;
using SAHL.Common.BusinessModel.CorrespondenceGeneration;

namespace SAHL.Web.Views.Correspondence.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICorrespondenceProcessing : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSendButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnPreviewButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnAddressAddButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnRecipientsGridSelectedIndexChanged;

        /// <summary>
        /// Sets up the allowed Correspondence Mediums
        /// </summary>
        /// <param name="reportDataList"></param>
        void SetupAllowedCorrespondenceMediums(List<ReportData> reportDataList);

        /// <summary>
        /// Sets up any extra Correspondence Parameters
        /// </summary>
        /// <param name="reportDataList"></param>
        /// <param name="cachedReportDataList"></param>
        void SetupExtraCorrespondenceParameters(List<ReportData> reportDataList, List<ReportData> cachedReportDataList);

        /// <summary>
        /// Binds the Legal Entitity Grid data
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="genericKeyTypeKey"></param>
        /// <param name="lstLegalEntities"></param>
        void BindRecipientData(int genericKey, int genericKeyTypeKey, IReadOnlyEventList<ILegalEntity> lstLegalEntities);

        /// <summary>
        /// Binds the Legal Entitity Address Grid data
        /// </summary>
        /// <param name="lstLegalEntityAddress"></param>
        /// <param name="lstMailingAddress"></param>
        void BindAddressData(IEventList<ILegalEntityAddress> lstLegalEntityAddress, IEventList<IAddress> lstMailingAddress);

        /// <summary>
        /// Binds the Correspondence Controls
        /// </summary>
        /// <param name="legalEntity"></param>
        void BindCorrespondenceMediumData(ILegalEntity legalEntity);

        /// <summary>
        /// Sets whether the screen functions in Single or Multiple recipient mode
        /// </summary>
        bool MultipleRecipientMode { get;  set; }

        /// <summary>
        /// Gets/Sets the GenericKey
        /// </summary>
        int GenericKey { get; set; }

        /// <summary>
        /// Gets/Sets the GenericKeyTypeKey
        /// </summary>
        int GenericKeyTypeKey { get; set; }

        /// <summary>
        /// Gets/Sets the Correspondence Documents
        /// </summary>
        string CorrespondenceDocuments { get; set; }

        /// <summary>
        /// Gets the selected CorrespondenceMediums
        /// </summary>
        List<CorrespondenceMediumInfo> SelectedCorrespondenceMediumInfo { get; }

        /// <summary>
        /// Sets whether to show/hide the life workflow header
        /// </summary>
        bool ShowLifeWorkFlowHeader { set;}

        /// <summary>
        /// Sets whether to show/hide the cancel button
        /// </summary>
        bool ShowCancelButton { set;}

        bool ShowMailingAddress { set; }

        bool AllowPreview { set; }

        int DocumentLanguageKey { get; set; }

        string DocumentLanguageDesc { set; }

        List<CorrespondenceExtraParameter> ExtraCorrespondenceParameters { get; }

        bool DisplayAttorneyRole { get; set;}

        bool DisplayDebtCounsellors { get; set; }

        bool DisplayClientsAndNCR { get; set; }

        bool DisableCorrespondenceOptionEntry { set;}

        bool SupressConfirmationMessage { set;}

        bool SetEmailOptionChecked { set;}

        IAddress AccountMailingAddress { get; set; }

        bool AddressParameterRequired { get;  set; }

        /// <summary>
        /// Setting this to TRUE will print one copy of the report for legalentities that share a domicilium address
        /// This will only apply to Post option only
        /// </summary>
        bool PostSingleCopyForSharedDomiciliums { get; set; }
        /// <summary>
        /// Setting this to TRUE will print one copy of the report for legalentities that share a domicilium address
        /// This will only apply to Email option only
        /// </summary>
        bool EmailSingleCopyForSharedDomiciliums { get; set; }
        /// <summary>
        /// If this is true documents generated for the client will ALSO be cc'd to the deb counsellor if the debt counsellor is selected as a recipient
        /// </summary>
        bool CCDebtCounsellor { get; set; }
        
        bool LegalEntityCorrespondence { get; set; }
    }
}

