using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.UI;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Administration.Presenters
{
    public class AttorneyInvoiceView : SAHLCommonBasePresenter<IAttorneyInvoice>
    {
        public AttorneyInvoiceView(IAttorneyInvoice view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            
            base.OnViewInitialised(sender, e);

            if (!_view.ShouldRunPage)
                return;

            _view.ReadOnly = true;

            CBOMenuNode _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            switch (_node.GenericKeyTypeKey)
            {

                //case (int)GenericKeyTypes.DebtCounselling2AM:
                //        IDebtCounselling dc = dcRepo.GetDebtCounsellingByKey(_node.GenericKey);
                //        _financialService = dc.Account.FinancialServices[0];
                //        break;

                case (int)GenericKeyTypes.Account:
                        _view.AccountKey = _node.GenericKey;
                        BindInvoiceGrid(_view.AccountKey);
                        break;

                default:
                    break;
            }
        }
        protected void BindInvoiceGrid(int accountKey)
        {
            IEventList<IAccountAttorneyInvoice> invList = AccRepo.GetAccountAttorneyInvoiceListByAccountKey(accountKey);

            _view.BindGrid(invList);
        }

        private IAccountRepository _accRepo;
        protected IAccountRepository AccRepo
        {
            get
            {
                if (_accRepo == null)
                    _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();

                return _accRepo;
            }
        }

        private IRegistrationRepository _regRepo;
        protected IRegistrationRepository RegRepo
        {
            get
            {
                if (_regRepo == null)
                    _regRepo = RepositoryFactory.GetRepository<IRegistrationRepository>();

                return _regRepo;
            }
        }
    }
}