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
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel;
using SAHL.Common.Service.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.Collections;
using System.Threading;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common;
using SAHL.Common.CacheData;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Common.Presenters.Reassign
{
    public class ReassignUserBase : SAHLCommonBasePresenter<IReassignUser>
    {
        protected ILookupRepository _lookups;
        protected IOrganisationStructureRepository _oSR;
        protected IApplicationRepository _appRepo;
        protected IApplicationRole _appRole;
        protected int _applicationKey;
        protected CBOMenuNode _node;
        protected InstanceNode _instanceNode;
        protected long _instanceID;
        protected IMemoRepository _memoRepo;
        protected IADUser _aduser;
        protected IApplicationMortgageLoan _appML;

        public ReassignUserBase(IReassignUser view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node != null)
                _applicationKey = Convert.ToInt32(_node.GenericKey);
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!View.ShouldRunPage) return;

            SetUp();
        }


        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
            if (!View.ShouldRunPage) return;

            if (_view.Messages.Count > 0)
                SetUp();
        }

        protected void BindAssignToUsers()
        {
            IOrganisationStructure organisationStructure = _oSR.GetOrganisationStructForADUser(_appRole);
            if (organisationStructure == null)
            {
                IADUser ReassignADUSER = _oSR.GetAdUserByLegalEntityKey(_appRole.LegalEntity.Key);
                string errorMessage = string.Format("Unable to determine an OrganisationStructure for ({0}), from whom the case is to be reassigned.", ReassignADUSER.ADUserName);
                _view.Messages.Add(new Error(errorMessage, errorMessage));
                DisableAllControls();
            }
            else
            {
                IEventList<IADUser> _toADUserLst = _oSR.GetADUserPerOrgStructAndAppRole(organisationStructure, _appRole);
                if (_toADUserLst != null && _toADUserLst.Count > 0)
                {
                    _view.ConsultantsRowVisible = true;
                    _view.BindUsers(_toADUserLst);
                }
            }
        }

        protected void GenerateMemo()
        {
            SAHL.Common.BusinessModel.Interfaces.IMemo memo = _memoRepo.CreateMemo();
            memo.GenericKey = _applicationKey;
            memo.GenericKeyType = _lookups.GenericKeyType.ObjectDictionary[((int)GenericKeyTypes.Offer).ToString()];
            // create as resolved when
            memo.GeneralStatus  = _lookups.GeneralStatuses[GeneralStatuses.Inactive]; 
            memo.ADUser = _aduser;
            memo.Description = _view.MemoDescription;
            memo.InsertedDate = DateTime.Now;
            _memoRepo.SaveMemo(memo);
        }

        protected bool ValidateUser()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
            IDomainMessageCollection dmc = spc.DomainMessages;
            IRuleService svc = ServiceFactory.GetService<IRuleService>();
            svc.ExecuteRule(dmc, "ReassignUserValidateLoggedInUser", _appRole,_aduser);
            if (_view.Messages.Count > 0)
            {
                DisableAllControls();
                return false;
            }
            else
                return true;
        }

        protected void DisableAllControls()
        {
            _view.SubmitButtonVisible = false;
            _view.ConsultantsRowVisible = false;
            _view.RoleVisible = false;
            _view.ShowCommentRow = false;
            _view.ShowCheckBoxRow = false;
            _view.ShowGrid = false;
            _view.CancelButtonVisible = false;
        }

        private void SetUp()
        {
            _lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            _oSR = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _memoRepo = RepositoryFactory.GetRepository<IMemoRepository>();
            _aduser = _oSR.GetAdUserForAdUserName(Thread.CurrentPrincipal.Identity.Name);
            _instanceNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            _instanceID = _instanceNode.InstanceID;
        }
    }
}

