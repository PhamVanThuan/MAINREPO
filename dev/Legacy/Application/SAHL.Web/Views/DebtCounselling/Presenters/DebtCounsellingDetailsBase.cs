using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.DebtCounselling.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.DebtCounselling.Presenters
{
    public class DebtCounsellingDetailsBase : SAHLCommonBasePresenter<IDebtCounsellingDetails>
    {
        public CBOMenuNode _node;
        public InstanceNode _instanceNode;
        protected IDebtCounselling _debtCounselling;

        private int _debtCounsellingKey;
        private int _genericKey;
        public int GenericKey
        {
            get { return _genericKey; }
            set { _genericKey = value; }
        }

        private int _genericKeyTypeKey;
        public int GenericKeyTypeKey
        {
            get { return _genericKeyTypeKey; }
            set { _genericKeyTypeKey = value; }
        }


        private IDebtCounsellingRepository _debtCounsellingRepo;
        public IDebtCounsellingRepository DebtCounsellingRepo
        {
            get
            {
                if (_debtCounsellingRepo == null)
                    _debtCounsellingRepo = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();

                return _debtCounsellingRepo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DebtCounsellingDetailsBase(IDebtCounsellingDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // Get the Instance Node   
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            if (_node is InstanceNode)
            {
                _instanceNode = _node as InstanceNode;
                _genericKey = _instanceNode.GenericKey; // this will be the debtcounsellingkey
                _genericKeyTypeKey = _instanceNode.GenericKeyTypeKey;
            }
            else
            {
                _genericKey = _node.GenericKey;
                _genericKeyTypeKey = _node.GenericKeyTypeKey;
            }

            switch (GenericKeyTypeKey)
            {
                case (int)SAHL.Common.Globals.GenericKeyTypes.DebtCounselling2AM:
                    //Generic Key is Debt Counselling Key
                    _debtCounsellingKey = GenericKey;
                    break;
                default:
                    break;
            }

            if (_debtCounsellingKey > 0)
                _debtCounselling = DebtCounsellingRepo.GetDebtCounsellingByKey(_debtCounsellingKey);

            //Bind the data
            BindDebtCounsellingDetails();

        }

        private void BindDebtCounsellingDetails()
        {
            if (_debtCounselling == null)
                return;

            if (!String.IsNullOrEmpty(_debtCounselling.ReferenceNumber))
            {
                _view.ReferenceNumber = _debtCounselling.ReferenceNumber;
            }
        }
    }
}