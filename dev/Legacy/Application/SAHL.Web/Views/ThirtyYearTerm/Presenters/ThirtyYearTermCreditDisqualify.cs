using Castle.ActiveRecord;
using SAHL.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Common.Presenters.CommonReason;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.ThirtyYearTerm.Presenters
{
    public class ThirtyYearTermCreditDisqualify : CommonReasonBase
    {

        public ThirtyYearTermCreditDisqualify(ICommonReason view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            // get the instance node
            InstanceNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;

            base.GenericKey = node.GenericKey;
        }

        protected override void _view_OnSubmitButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            var selectedReasons = (List<SelectedReason>)e.Key;

            foreach (var reason in selectedReasons)
            {
                if (string.IsNullOrEmpty(reason.Comment))
                {
                    _view.Messages.Add(new Error("You must enter a comment for the selected reason.", "You must enter a comment for the selected reason."));
                }
            }

            if (_view.Messages.Count > 0)
                return;

            AddReasonsAndAttributes(sender, e);

            CompleteActivityAndNavigate();
        }

        private void AddReasonsAndAttributes(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                base._view_OnSubmitButtonClicked(sender, e);
                AddApplicationAttribute();
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
        }

        private void AddApplicationAttribute()
        {
            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IApplication application = appRepo.GetApplicationByKey(_genericKey);

            if (!application.HasAttribute(OfferAttributeTypes.CreditDisqualified30YearTerm))
            {
                IApplicationAttribute applicationAttributeNew = _appRepo.GetEmptyApplicationAttribute();
                applicationAttributeNew.ApplicationAttributeType = lookupRepo.ApplicationAttributesTypes.ObjectDictionary[((int)OfferAttributeTypes.CreditDisqualified30YearTerm).ToString()];
                applicationAttributeNew.Application = application;
                application.ApplicationAttributes.Add(_view.Messages, applicationAttributeNew);
                appRepo.SaveApplication(application);
            }

        }

        public override void CancelActivity()
        {
            base.X2Service.CancelActivity(_view.CurrentPrincipal);
            base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
        }

        public override void CompleteActivityAndNavigate()
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                X2ServiceResponse rsp = base.X2Service.CompleteActivity(_view.CurrentPrincipal, null, false);
                if (base.sdsdgKeys.Count > 0)
                {
                    UpdateReasonsWithStageTransitionKey();
                }
                base.X2Service.WorkflowNavigate(_view.CurrentPrincipal, _view.Navigator);
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
        }
    }
}