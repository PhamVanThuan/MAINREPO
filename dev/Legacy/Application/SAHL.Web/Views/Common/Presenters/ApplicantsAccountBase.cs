using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class ApplicantsAccountBase: ApplicantsBase
    {
        private IAccountRepository _accountRepository;
        private IAccount _account;
        private IList<IRole> _accRoles;

        private IDictionary<string, string> _accRoleTypes;

        public IDictionary<string, string> AccRoleTypes
        {
            get { return _accRoleTypes; }
            set { _accRoleTypes = value; }
        }

        protected IAccountRepository AccountRepository
        {
            get { return _accountRepository; }
        }

        protected IAccount Account
        {
            get { return _account; }
            set { _account = value; }
        }

        protected IList<IRole> AccountRoles
        {
            get { return _accRoles; }
            set { _accRoles = value; }
        }
        
        /// <summary>
        /// Consructor. Gets the View and controller pairs.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicantsAccountBase(IApplicants view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            //default the presenter to assume that if no account has been set, 
            //then the generic key is the account key
            if (_account == null)
                _account = _accountRepository.GetAccountByKey(_node.GenericKey);


            //// set the applicationroletypes to display
            AccRoleTypes = new Dictionary<string, string>();
            string roleTypeKey = Convert.ToString((int)RoleTypes.AssuredLife);
            AccRoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.RoleTypes[((int)RoleTypes.AssuredLife)-1].Description));
            roleTypeKey = Convert.ToString((int)RoleTypes.MainApplicant);
            AccRoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.RoleTypes[((int)RoleTypes.MainApplicant)-1].Description));
            roleTypeKey = Convert.ToString((int)RoleTypes.Suretor);
            AccRoleTypes.Add(new KeyValuePair<string, string>(roleTypeKey, base.LookupRepository.RoleTypes[((int)RoleTypes.Suretor)-1].Description));

            // get the applicants
            _accRoles = new List<IRole>();
            foreach (IRole role in _account.Roles)
            {
                foreach (KeyValuePair<string, string> roleType in AccRoleTypes)
                {
                    if (role.RoleType.Key.ToString() == roleType.Key)
                    {
                        _accRoles.Add(role);
                        break;
                    }
                }
            }

            _view.BindAccountApplicantsGrid(_accRoles);

            if (_accRoles.Count > 0)
            {
                _legalEntity = _accRoles[0].LegalEntity;
                _view.BindLegalEntityDetails(_legalEntity);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);

            if (!_view.ShouldRunPage)
                return;

            if (_accRoles.Count > 0)
            {
                _view.ApplicantDetailsVisible = true;
                _view.CancelButtonEnabled = true;
                _view.RemoveButtonEnabled = true;
                _view.AddButtonEnabled = false;

                if (_legalEntity is ILegalEntityNaturalPerson)
                {
                    _view.ApplicantDetailsNaturalPersonVisible = true;
                    _view.ApplicantDetailsCompanyVisible = false;
                }
                else
                {
                    _view.ApplicantDetailsNaturalPersonVisible = false;
                    _view.ApplicantDetailsCompanyVisible = true;
                }
            }
            else
            {
                _view.ApplicantDetailsVisible = false;
                _view.ApplicantDetailsCompanyVisible = false;
                _view.ApplicantDetailsNaturalPersonVisible = false;
                _view.CancelButtonEnabled = false;
                _view.RemoveButtonEnabled = false;
                _view.AddButtonEnabled = false;
            }
        }
    }
}
