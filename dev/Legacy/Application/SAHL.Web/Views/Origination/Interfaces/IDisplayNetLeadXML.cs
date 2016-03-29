namespace SAHL.Web.Views.Origination.Interfaces
{
    using System;
    using SAHL.Common.BusinessModel.Interfaces;
    using SAHL.Common.Web.UI;

    public interface IDisplayNetLeadXML : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnRetryCreateClicked;

        /// <summary>
        /// 
        /// </summary>
        void BindRetryButtonText(string stateName);

        /// <summary>
        /// 
        /// </summary>
        void BindRawNetLeadXML(ILeadInputInformation leadInputInformation);
    }
}
