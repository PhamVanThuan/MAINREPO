using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Origination.Interfaces
{
    public interface IResendInstruction : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="attorneys"></param>
        void BindRegistrationAttorneys(IList<IAttorney> attorneys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deedsOffice"></param>
        void BindDeedsOffice(IEventList<IDeedsOffice> deedsOffice);

        /// <summary>
        /// 
        /// </summary>
        int GetAttorneySelected { get;}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regmail"></param>
        void BindInstructionGrid(IList<IRegMail> regmail);

        /// <summary>
        /// 
        /// </summary>
        void SetLookups();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regmail"></param>
        void SetUpdateableFields(IRegMail regmail);

        #region EventHandlers
        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnDeedsOfficeSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnUpdateButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSendInstructionClicked;

        /// <summary>
        /// 
        /// </summary>
        bool SetUpdateButtonEnabled { set;}

        #endregion
    }
}
