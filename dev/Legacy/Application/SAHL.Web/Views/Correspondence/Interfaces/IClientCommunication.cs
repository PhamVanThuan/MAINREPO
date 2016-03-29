using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Correspondence.Interfaces
{
    public class BindableRecipient
    {
        public BindableRecipient(ILegalEntity legalEntity, string roleDescription)
        {
            Key = legalEntity.Key;
            RecipientName = legalEntity.GetLegalName(LegalNameFormat.Full);
            CellPhoneNumber = legalEntity.CellPhoneNumber;
            EmailAddress = legalEntity.EmailAddress;
            Role = roleDescription;
        }
        public BindableRecipient(int legalEntityKey,string recipientName, string cellphoneNumber, string emailAddress, string role)
        {
            Key = legalEntityKey;
            RecipientName = recipientName;
            CellPhoneNumber = cellphoneNumber;
            EmailAddress = emailAddress;
            Role = role;
        }
        public int Key { get; set; }
        public string RecipientName { get; set; }
        public string Role { get; set; }
        public string CellPhoneNumber { get; set; }
        public string EmailAddress { get; set; }
    }

    public class SelectedClientCommuncation
    {
        public SAHL.Common.Globals.CorrespondenceMediums CorrespondenceMedium { get; set; }
        public string SMSType { get; set; }
        public string Subject { get; set; } 
        public string Body { get; set; }

        public List<BindableRecipient> SelectedRecipients;
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IClientCommunication : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnSendButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Binds the Recipients grid
        /// </summary>
        /// <param name="lstRecipients"></param>
        void BindRecipients(IList<BindableRecipient> lstRecipients);

        /// <summary>
        /// Binds the SMS Type dropdown list
        /// </summary>
        /// <param name="lstSMSTypes"></param>
        void BindSMSTypes(IList<SAHL.Common.Globals.SMSTypes> lstSMSTypes);

        /// <summary>
        /// Gets/Sets the GenericKey
        /// </summary>
        int GenericKey { get; set; }

        /// <summary>
        /// Gets/Sets the GenericKeyTypeKey
        /// </summary>
        int GenericKeyTypeKey { get; set; }

        /// <summary>
        /// Gets/Sets the SAHL/RCS Banking Details
        /// </summary>
        string BankDetails { get;  set; }

        /// <summary>
        /// Gets/Sets the Email Body
        /// </summary>
        string EmailBody { get; set; }

        /// <summary>
        /// Gets/Sets the Email Subject
        /// </summary>
        string EmailSubject { get; set; }

        /// <summary>
        /// Gets/Sets the SMS Type
        /// </summary>
        string SMSType { get; set; }

        /// <summary>
        /// Gets/Sets the SMS Text
        /// </summary>
        string SMSText { get; set; }

        /// <summary>
        /// Gets/Sets the CorrespondenceMedium
        /// </summary>
        SAHL.Common.Globals.CorrespondenceMediums CorrespondenceMedium  { get; set; }

        bool SelectFirstItem { get; set; }
    }
}

