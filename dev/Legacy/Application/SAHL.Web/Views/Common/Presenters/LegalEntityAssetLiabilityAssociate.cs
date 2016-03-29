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
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.BusinessModel.Repositories;

namespace SAHL.Web.Views.Common.Presenters
{
    public class LegalEntityAssetLiabilityAssociate : LegalEntityAssetLiabilityBase
    {
        IEventList<ILegalEntityAssetLiability> appAssets;

        public LegalEntityAssetLiabilityAssociate(ILegalEntityAssetLiabilityDetails view, SAHLCommonBaseController controller)
        : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            gridSelectedIndex = 0;

            IEventList<ILegalEntityAssetLiability> leAssetLiabilityList = new EventList<ILegalEntityAssetLiability>();
            if (legalEntity.LegalEntityAssetLiabilities != null)
            {
                foreach (ILegalEntityAssetLiability _leal in legalEntity.LegalEntityAssetLiabilities)
                {
                    if (_leal.GeneralStatus.Key == (int)GeneralStatuses.Active)
                        leAssetLiabilityList.Add(null, _leal);
                }
            }

            if (leAssetLiabilityList.Count > 0)
                _view.BindAssetLiabilityGrid(_view.ViewName, leAssetLiabilityList);
           
            appAssets = new EventList<ILegalEntityAssetLiability>();

            GetAllAssetsBelongingToSpouse();

            // if we are on the FLOBO and are dealing with an application, then the parent node genrickey should be an application key
            // we must get all the Fixed Assets for the other applicants on the Application
            if (_node.ParentNode != null && _node.ParentNode.GenericKeyTypeKey == (int)GenericKeyTypes.Offer)
            {
                IApplication app = RepositoryFactory.GetRepository<IApplicationRepository>().GetApplicationByKey(Convert.ToInt32(_node.ParentNode.GenericKey));
                GetAllFixedAssetsOfOtherApplicants(app);
            }

            RemoveAssetsAlreadyAssociatedWithApplicant();

            _view.ShowUpdate = false;
            _view.SetUpdateButtonText("Add");

            if (appAssets.Count > 0)
            {
                _view.ShowpnlAssociate(false);
                _view.BindAssociatedAssets(appAssets);
                _view.ShowCancelButton = true;
                _view.ShowUpdateButton = true;
            }
            _view.OndddlAssociateSelectedIndexChanged+=new KeyChangedEventHandler(_view_OndddlAssociateSelectedIndexChanged);
            _view.OnAddButtonClicked+=new EventHandler(_view_OnAddButtonClicked);
        }

        protected override void OnViewPreRender(object sender, EventArgs e)
        {

            base.OnViewPreRender(sender, e);
            if (!_view.ShouldRunPage)
                return;

            if (appAssets.Count > 0)
                _view.BindDisplayPanel(appAssets[gridSelectedIndex]);
            else
                _view.ShowpnlAssociate(true);
        }

        protected void _view_OndddlAssociateSelectedIndexChanged(object sender, KeyChangedEventArgs e)
        {
            gridSelectedIndex = Convert.ToInt32(e.Key);
        }

        private void RemoveAssetsAlreadyAssociatedWithApplicant()
        {
            if (appAssets != null && appAssets.Count > 0)
            {
                for (int x = 0; x < legalEntity.LegalEntityAssetLiabilities.Count; x++)
                {
                    for (int y = 0; y < appAssets.Count; y++)
                        if (legalEntity.LegalEntityAssetLiabilities[x].AssetLiability.Key == appAssets[y].AssetLiability.Key)
                            appAssets.Remove(_view.Messages, appAssets[y]);
                }
            }
        }

        private void GetAllFixedAssetsOfOtherApplicants(IApplication app)
        {
            foreach (IApplicationRole role in app.ApplicationRoles)
            {
                if (role.LegalEntity.Key != legalEntity.Key) // Ignore Applicant you're working on
                {
                    // Added Lead Main Applicant and Lead Main Suretor
                    if (role.ApplicationRoleType.Key == (int)OfferRoleTypes.MainApplicant || role.ApplicationRoleType.Key == (int)OfferRoleTypes.Suretor
                        || role.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadMainApplicant || role.ApplicationRoleType.Key == (int)OfferRoleTypes.LeadSuretor)
                    {
                        foreach (ILegalEntityAssetLiability leAssetLiability in role.LegalEntity.LegalEntityAssetLiabilities)
                            if (leAssetLiability.AssetLiability is IAssetLiabilityFixedProperty 
                                && leAssetLiability.GeneralStatus.Key == (int)GeneralStatuses.Active)
                                appAssets.Add(_view.Messages, leAssetLiability);
                    }
                }
            }
        }

        private void GetAllAssetsBelongingToSpouse()
        {
            ILegalEntityNaturalPerson leNaturalPerson = legalEntity as ILegalEntityNaturalPerson;
            // If married COP - Fetch all assets belonging to spouse
            if (leNaturalPerson != null)
            {
                if (leNaturalPerson.MaritalStatus != null && leNaturalPerson.MaritalStatus.Key == (int)MaritalStatuses.MarriedCommunityofProperty)
                for (int i = 0; i < leNaturalPerson.LegalEntityRelationships.Count; i++)
                {   
                    if (leNaturalPerson.LegalEntityRelationships[i].LegalEntityRelationshipType.Key == (int)LegalEntityRelationshipTypes.Spouse)
                    {
                        foreach (ILegalEntityAssetLiability le in leNaturalPerson.LegalEntityRelationships[i].RelatedLegalEntity.LegalEntityAssetLiabilities)
                        {
                            if (le.GeneralStatus.Key == (int)GeneralStatuses.Active)
                                appAssets.Add(_view.Messages, le); 
                        }
                    }
                }
            }
        }

        protected void _view_OnAddButtonClicked(object sender, EventArgs e)
        {
            ILegalEntityAssetLiability le = leRepo.GetEmptyLegalEntityAssetLiability();

            le.LegalEntity = legalEntity;
            le.AssetLiability = appAssets[gridSelectedIndex].AssetLiability;
            le.GeneralStatus = lookups.GeneralStatuses[GeneralStatuses.Active];

            TransactionScope txn = new TransactionScope();

            try
            {
                leRepo.SaveLegalEntityAssetLiability(le);
                txn.VoteCommit();

                _view.Navigator.Navigate(_view.ViewName);
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
        }
    }
}
