using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    ///
    /// </summary>
    public class ApplicantsOfferBase: ApplicantsBase
    {
        private IApplicationRepository _applicationRepository;
        private IApplication _application;
        private IList<IApplicationRole> _applicationRoles;

        private IDictionary<string, string> _applicationRoleTypes;

        public IDictionary<string, string> ApplicationRoleTypes
        {
            get { return _applicationRoleTypes; }
            set { _applicationRoleTypes = value; }
        }

        protected IApplicationRepository ApplicationRepository
        {
            get { return _applicationRepository; }
        }

        protected IApplication Application
        {
            get { return _application; }
            set { _application = value; }
        }

        protected IList<IApplicationRole> ApplicationRoles
        {
            get { return _applicationRoles; }
            set { _applicationRoles = value; }
        }
        
        /// <summary>
        /// Consructor. Gets the View and controller pairs.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ApplicantsOfferBase(IApplicants view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
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

            _application = _applicationRepository.GetApplicationByKey(_node.GenericKey);

            // get the applicants
            _applicationRoles = new List<IApplicationRole>();
            foreach (IApplicationRole role in _application.ApplicationRoles)
            {
                foreach (KeyValuePair<string, string> roleType in _applicationRoleTypes)
                {
                    if (role.ApplicationRoleType.Key.ToString() == roleType.Key)
                    {
                        _applicationRoles.Add(role);
                        break;
                    }
                }
            }

            _view.BindOfferApplicantsGrid(_applicationRoles);

            if (_applicationRoles.Count > 0)
            {
                _legalEntity = _applicationRoles[0].LegalEntity;
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

            if (_applicationRoles.Count > 0)
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