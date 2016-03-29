using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Origination.Interfaces;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationAttributesUpdate : SAHLCommonBasePresenter<IApplicationAttributes>
    {
        CBOMenuNode _node;
        int offerKey;
        IApplicationRepository appRepo;
        IApplicationMortgageLoan appMortgageLoan;
        ILookupRepository _lookUps;
        IApplicationMortgageLoan applicationMortgageLoan;

        public ApplicationAttributesUpdate(IApplicationAttributes view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            offerKey = _node.GenericKey;

            appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            appMortgageLoan = appRepo.GetApplicationByKey(offerKey) as IApplicationMortgageLoan;

            _lookUps = RepositoryFactory.GetRepository<ILookupRepository>();

            IList<IApplicationAttributeType> applicationAttributes = appRepo.GetApplicationAttributeTypeByIsGeneric(true);
            IEventList<IApplicationAttributeType> appAttributes = new EventList<IApplicationAttributeType>(applicationAttributes);

            _view.PopulateAttributes(appAttributes);

            IEventList<IApplicationSource> marketSource = new EventList<IApplicationSource>();
            IEnumerable<IApplicationSource> marketingSources = _lookUps.ApplicationSources.OrderBy(x => x.Description);
            foreach (IApplicationSource marketingSource in marketingSources)
            {
                if (marketingSource.GeneralStatus.Key == (int)GeneralStatuses.Active)
                    marketSource.Add(_view.Messages, marketingSource);
            }

            _view.PopulateMarketingSource(marketSource);

            _view.BindApplication(appMortgageLoan);
            _view.ShowButtons = true;
            _view.showControlsForUpdate();

            _view.onCancelButtonClicked += new EventHandler(_view_onCancelButtonClicked);
            _view.onUpdateButtonClicked += new EventHandler(_view_onUpdateButtonClicked);
        }

        protected void _view_onCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("ApplicationAttributes");
        }

        protected void _view_onUpdateButtonClicked(object sender, EventArgs e)
        {
            applicationMortgageLoan = _view.GetUpdatedApplicationMortgageLoan(appMortgageLoan);

            CreateApplicationAttributeList();

            TransactionScope txn = new TransactionScope();
            try
            {
                appRepo.SaveApplication(applicationMortgageLoan);
                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }

            finally
            {
                txn.Dispose();
            }

            _view.Navigator.Navigate("ApplicationAttributes");
        }

        private void CreateApplicationAttributeList()
        {
            ListItemCollection applicationAttributeOptions = _view.GetAttributeOptions;

            bool found;

            // Only need do this if application has attributes
            if (applicationMortgageLoan.ApplicationAttributes.Count > 0)
            {   // This list has to be created to retain the original applicationMortgageLoan.ApplicationAttributes
                IList<IApplicationAttribute> appAttributeLst = new List<IApplicationAttribute>();
                for (int i = 0; i < appMortgageLoan.ApplicationAttributes.Count; i++)
                {
                    appAttributeLst.Add(appMortgageLoan.ApplicationAttributes[i]);
                }
                // Handle possible Deletions
                foreach (IApplicationAttribute appAttribute in appAttributeLst)
                {
                    ListItem hasItem = applicationAttributeOptions.FindByValue(appAttribute.ApplicationAttributeType.Key.ToString());
                    if (hasItem != null && hasItem.Selected == false)
                        applicationMortgageLoan.ApplicationAttributes.Remove(_view.Messages, appAttribute);
                }
            }

            // Handle possible Additions
            foreach (ListItem applicationAttribute in applicationAttributeOptions)
            {
                found = false;

                if (applicationAttribute.Selected == true)
                {
                    // Find the option in the domain object; If selected then add it
                    foreach (IApplicationAttribute appAttribute in applicationMortgageLoan.ApplicationAttributes)
                    {
                        if (appAttribute.ApplicationAttributeType.Key == Convert.ToInt32(applicationAttribute.Value))
                        {
                            found = true;
                            break;
                        }
                    }

                    // If not found, add it.
                    if (!found)
                    {
                        IApplicationAttribute applicationAttributeNew = appRepo.GetEmptyApplicationAttribute();
                        applicationAttributeNew.ApplicationAttributeType = _lookUps.ApplicationAttributesTypes.ObjectDictionary[applicationAttribute.Value];
                        applicationAttributeNew.Application = applicationMortgageLoan;
                        applicationMortgageLoan.ApplicationAttributes.Add(_view.Messages, applicationAttributeNew);
                    }
                }
            }
        }
    }
}