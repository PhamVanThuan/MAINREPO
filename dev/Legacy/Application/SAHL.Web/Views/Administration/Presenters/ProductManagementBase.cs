using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class ProductManagementBase : SAHLCommonBasePresenter<IProductManagement>
    {
        private ICreditMatrixRepository _creditMatrixRepo;
        private ILookupRepository _lookupRepo;

        /// <summary>
        /// 
        /// </summary>
        protected ICreditMatrixRepository CreditMatrixRepo
        {
            get
            {
                return _creditMatrixRepo;
            }
            set
            {
                _creditMatrixRepo = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected ILookupRepository LookupRepo
        {
            get
            {
                return _lookupRepo;
            }
            set
            {
                _lookupRepo = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ProductManagementBase(IProductManagement view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            _creditMatrixRepo = RepositoryFactory.GetRepository<ICreditMatrixRepository>();
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

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

            _view.onOriginationSource_SelectedIndexChanged += new EventHandler(_view_onOriginationSource_SelectedIndexChanged);
            _view.onProduct_SelectedIndexChanged += new EventHandler(_view_onProduct_SelectedIndexChanged);
            _view.onLoadButtonClicked += new EventHandler(_view_onLoadButtonClicked);
            _view.onSubmitButtonClicked += new EventHandler(_view_onSubmitButtonClicked);

            // bind Lookups
            BindLookups();

            _view.UpdateMode = false;
        }

        void _view_onSubmitButtonClicked(object sender, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void _view_onProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCreditCriteria(_view.SelectedOriginationSourceKey, _view.SelectedProductKey);
        }

        void _view_onOriginationSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCreditCriteria(_view.SelectedOriginationSourceKey, _view.SelectedProductKey);
        }

        void _view_onLoadButtonClicked(object sender, EventArgs e)
        {
            BindCreditCriteria(_view.SelectedOriginationSourceKey, _view.SelectedProductKey);
        }

        private void BindCreditCriteria(int originationSourceKey, int productKey)
        {
            DataSet dsCreditCriteria = _creditMatrixRepo.GetCreditCriteriaByOSP(originationSourceKey, productKey);
            _view.BindCreditCriteria(dsCreditCriteria); 
        }

        private void BindLookups()
        {
            _view.BindOriginationSources(_lookupRepo.OriginationSources.BindableDictionary, Convert.ToString((int)SAHL.Common.Globals.OriginationSources.SAHomeLoans), false);
            _view.BindProducts(_lookupRepo.Products.BindableDictionary, String.Empty, true);
        }
    }
}
