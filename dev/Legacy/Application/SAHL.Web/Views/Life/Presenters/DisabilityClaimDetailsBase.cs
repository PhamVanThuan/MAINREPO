using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Web.Services;
using SAHL.Services.Interfaces.LifeDomain;
using SAHL.Services.Interfaces.LifeDomain.Models;
using SAHL.Services.Interfaces.LifeDomain.Queries;
using SAHL.V3.Framework;
using SAHL.V3.Framework.DomainServices;
using SAHL.Web.Views.Life.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Web.Views.Life.Presenters
{
    public class DisabilityClaimDetailsBase : SAHLCommonBasePresenter<IDisabilityClaimDetails>
    {
        private CBOMenuNode cboNode;
        internal DisabilityClaimDetailModel disabilityClaim;
        public IV3ServiceManager v3ServiceManager;
        public IMortgageLoanDomainService mortgageLoanDomainService;
        public ILifeDomainService lifeDomainService;
        private IStageDefinitionRepository sdRepo;

        public DisabilityClaimDetailsBase(IDisabilityClaimDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            sdRepo = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            cboNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (cboNode == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            v3ServiceManager = V3ServiceManager.Instance;
            mortgageLoanDomainService = v3ServiceManager.Get<IMortgageLoanDomainService>();
            lifeDomainService = v3ServiceManager.Get<ILifeDomainService>();

            GetDisabilityClaimByKeyQuery getDisabilityClaimByKeyQuery = new GetDisabilityClaimByKeyQuery(cboNode.GenericKey);
            ISystemMessageCollection systemMessageCollection = lifeDomainService.PerformQuery(getDisabilityClaimByKeyQuery);

            if (!systemMessageCollection.HasErrors)
            {
                disabilityClaim = getDisabilityClaimByKeyQuery.Result.Results.FirstOrDefault();

                _view.DateOfDiagnosisEditable = false;
                _view.DisabilityTypeEditable = false;
                _view.AdditionalDisabilityDetailsEditable = false;
                _view.OccupationEditable = false;
                _view.LastDateWorkedEditable = false;
                _view.ExpectedReturnToWorkDateEditable = false;
                _view.UpdateButtonsVisible = false;

                ILookupRepository lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                _view.BindDisabilityTypes(lookupRepository.DisabilityTypes);

                _view.BindDisabilityClaim(disabilityClaim);

                if(disabilityClaim.DisabilityClaimStatusKey == (int)DisabilityClaimStatuses.Approved)
                {
                    var approvedTransition = sdRepo.GetStageTransitionsByGenericKey(disabilityClaim.DisabilityClaimKey, (int)StageDefinitionGroups.DisabilityClaimWorkflow)
                                             .FirstOrDefault(x => x.StageDefinitionStageDefinitionGroup.Key == (int)StageDefinitionStageDefinitionGroups.DisabilityClaimApproved);

                    _view.BindPaymentDetails(approvedTransition.ADUser.ADUserName, approvedTransition.TransitionDate, disabilityClaim.NumberOfInstalmentsAuthorised.Value, disabilityClaim.PaymentStartDate.Value, disabilityClaim.PaymentEndDate.Value);
                }

            }
            else
            {
                v3ServiceManager.HandleSystemMessages(systemMessageCollection);
            }
        }
    }
}