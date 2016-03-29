using System;
using System.Collections.Generic;
using System.Linq;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Common.Models.Affordability;

namespace SAHL.Web.Views.Common.Presenters.AffordabilityDetailsPresenters
{
    /// <summary>
    ///
    /// </summary>
    public class AffordabilityDetailsBase : SAHLCommonBasePresenter<IAffordabilityDetails>
    {
        /// <summary>
        ///
        /// </summary>
        private CBOMenuNode _node;

        private ILegalEntity _legalEntity;

        private IApplication _application;

        private ILegalEntityRepository _legalEntityRepository;

        private IApplicationRepository _applicationRepositoy;

        /// <summary>
        ///
        /// </summary>
        protected CBOMenuNode Node
        {
            get
            {
                return _node;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public ILegalEntity LegalEntity
        {
            get
            {
                return _legalEntity;
            }
            set
            {
                _legalEntity = value;
            }
        }

        public IApplication Application
        {
            get
            {
                return _application;
            }
            set
            {
                _application = value;
            }
        }

        public ILegalEntityRepository LegalEntityRepository
        {
            get 
            {
                return _legalEntityRepository;
            }
            set 
            {
                _legalEntityRepository = value;
            }
        }

        public IApplicationRepository ApplicationRepository
        {
            get
            {
                return _applicationRepositoy;
            }
            set
            {
                _applicationRepositoy = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public AffordabilityDetailsBase(IAffordabilityDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
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

            LegalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            ApplicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();

            _view.OnCancelButtonClicked += _view_OnCancelButtonClicked;
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;

            if (_node != null)
                switch (_node.GenericKeyTypeKey)
                {
                    case (int)SAHL.Common.Globals.GenericKeyTypes.LegalEntity:

                        // get the legalentity
                        _legalEntity = LegalEntityRepository.GetLegalEntityByKey(_node.GenericKey);

                        // get the application
                        _application = ApplicationRepository.GetApplicationByKey(_node.ParentNode.GenericKey);

                        InitialiseAffordabilityModel();
                        break;

                    case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:

                        // get the application
                        _application = ApplicationRepository.GetApplicationByKey(_node.GenericKey);
                        break;

                    default:
                        break;
                }

           

            IApplicationMortgageLoan ml = _application as IApplicationMortgageLoan;
            if (ml != null)
            {
                _view.NumberOfDependantsInHouseHold = ml.DependentsPerHousehold.HasValue ? ml.DependentsPerHousehold.Value.ToString() : "0";
                _view.ContributingDependants = ml.ContributingDependents.HasValue ? ml.ContributingDependents.Value.ToString() : "0";
            }
        }

        protected void InitialiseAffordabilityModel()
        {
            IList<AffordabilityModel> affordabilityModel = new List<AffordabilityModel>();
            foreach (var item in LegalEntity.LegalEntityAffordabilities.Where(x => x.Application.Key == Application.Key))
            {
                affordabilityModel.Add(new AffordabilityModel(item.AffordabilityType.Key, item.Description, item.AffordabilityType.DescriptionRequired, item.Amount, (AffordabilityTypeGroups)item.AffordabilityType.AffordabilityTypeGroup.Key, item.AffordabilityType.Description, item.AffordabilityType.Sequence));
            }

            _view.Affordability = affordabilityModel.OrderBy(x => x.AffordabilityTypeGroups);
        }

        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("AffordabilityDetails");
        }
    }
}