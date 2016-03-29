using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Service.Interfaces;
using System.Security.Principal;
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using System.Collections.Specialized;
using SAHL.Common.BusinessModel;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Globals;


namespace SAHL.Web.Views.Common.Presenters
{
    public class ClientDetailsDisplay : SAHLCommonBasePresenter<IClientDetails>
    {
        private CBOMenuNode _node;
        private ILegalEntity _legalEntity;
        private int _legalEntityKey;
        private ILegalEntityRepository _legalEntityRepository;

        public ClientDetailsDisplay(IClientDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            // Get the seleceted LE Key and bind it.
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBOManager.GetCurrentNodeSetName(_view.CurrentPrincipal)) as CBOMenuNode;
            if (!(_node == null))
                _legalEntityKey = Convert.ToInt32(_node.GenericKey);
            else
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _legalEntity = _legalEntityRepository.GetLegalEntityByKey(_legalEntityKey);

            if (_legalEntity is ILegalEntityNaturalPerson)
                _view.BindLegalEntityNaturalPerson((ILegalEntityNaturalPerson)_legalEntity);
            else
                _view.BindLegalEntityCompany(_legalEntity);


            // Bind the LE Address
            string formmattedAddress, addressDescription;

            formmattedAddress = GetAddressFormattedDescription(AddressTypes.Postal);
            if (String.IsNullOrEmpty(formmattedAddress))
            {
                formmattedAddress = GetAddressFormattedDescription(AddressTypes.Residential);
                addressDescription = "Legal Entity Address - Residential";
            }
            else
                addressDescription = "Legal Entity Address - Postal";


            StringCollection legalEntityAddresses = new StringCollection();
            string[] separator = { "<BR/>", "<Br />" };
            legalEntityAddresses.AddRange(formmattedAddress.Split(separator, StringSplitOptions.None));

            _view.BindAddresses(legalEntityAddresses);
            _view.BindAddressDescription(addressDescription);

        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage) return; 

            bool isLegalEntityNaturalPerson = (_legalEntity is ILegalEntityNaturalPerson);

            _view.NaturalPersonPanelVisible = isLegalEntityNaturalPerson;
            _view.CompanyPanelVisible = !isLegalEntityNaturalPerson;
        }

        /// <summary>
        /// Returns a formatted address for the first address of a specified type (Ordered by ChangeDate).
        /// </summary>
        /// <param name="TypeOfAddress">Type of address (Residential or Postal).</param>
        private string GetAddressFormattedDescription(AddressTypes TypeOfAddress)
        {
            string formmattedAddress = String.Empty;

            // A sorted list container to hold data 
            SortedList<int, int> SortedLegalEntityAddressKeys = new SortedList<int, int>();

            // Find the latest address
            for (int index = 0; index < _legalEntity.LegalEntityAddresses.Count; index++ )
            {
                if (_legalEntity.LegalEntityAddresses[index].AddressType.Key == (int)TypeOfAddress 
                    && _legalEntity.LegalEntityAddresses[index].GeneralStatus.Key == (int)GeneralStatuses.Active)
                    SortedLegalEntityAddressKeys.Add(_legalEntity.LegalEntityAddresses[index].Key, index);
            }

            if (SortedLegalEntityAddressKeys.Count > 0)
                formmattedAddress = _legalEntity.LegalEntityAddresses[SortedLegalEntityAddressKeys.Values[0]].Address.GetFormattedDescription(AddressDelimiters.HtmlLineBreak);

            return formmattedAddress;
        }
    }
}
