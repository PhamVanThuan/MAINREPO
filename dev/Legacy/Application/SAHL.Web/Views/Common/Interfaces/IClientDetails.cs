using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Specialized;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IClientDetails: IViewBase
    {
        #region Declare View Events

        #endregion

        #region Declare Presenter Actions 
        /// <summary>
        /// Binds a single <see cref="ILegalEntityNaturalPerson"/> object.
        /// </summary>
        void BindLegalEntityNaturalPerson(ILegalEntityNaturalPerson LegalEntity);

        /// <summary>
        /// Binds a single <see cref="ILegalEntityCompany"/> object
        /// </summary>
        /// <param name="LegalEntity"></param>
        void BindLegalEntityCompany(ILegalEntity LegalEntity);

        /// <summary>
        /// Binds the Postal and Residential addresses.
        /// </summary>
        /// <param name="LegalEntityAddress"></param>
        void BindAddresses(StringCollection LegalEntityAddress);

        /// <summary>
        /// Provides the address caption.
        /// </summary>
        /// <param name="AddressDescription"></param>
        void BindAddressDescription(string AddressDescription);

        #endregion

        #region Properties
        /// <summary>
        /// Sets whether the Natural Person panel is visible
        /// </summary>
        bool NaturalPersonPanelVisible { set;}

        /// <summary>
        /// Sets whether the Company panel is visible
        /// </summary>
        bool CompanyPanelVisible { set;}

        #endregion

    }
}
