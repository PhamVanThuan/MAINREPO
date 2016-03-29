using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.UI;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SAHL.Web.Views.Common.Interfaces;
using System.Linq;


namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductSummary : SAHLCommonBasePresenter<IProductSummary>
    {
        private ILegalEntityRepository _legalEntityRepository;
        private IList<IAccount> _accounts;
        private IList<IFinancialAdjustment> _financialAdjustments;
        private int _legalEntityKey;
        private CBOMenuNode _node;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public ProductSummary(IProductSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            // Get the CBO Node   
            _node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal) as CBOMenuNode;
            if (_node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            _legalEntityKey = _node.GetParentNodeByType(GenericKeyTypes.LegalEntity).GenericKey;

            _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
        }

        /// <summary>
        /// Hook the events fired by the view and call relevant methods to bind control data
        /// </summary>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {

            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage)
                return;

            // get the legalentity object
            ILegalEntity legalEntity = _legalEntityRepository.GetLegalEntityByKey(_legalEntityKey);

            // Get the list of Accounts & Rate Overrides
            _accounts = new List<IAccount>();
            _financialAdjustments = new List<IFinancialAdjustment>();
            foreach (IRole role in legalEntity.Roles)
            {
                if (role.Account.ParentAccount == null
                    && role.GeneralStatus.Key==(int)SAHL.Common.Globals.GeneralStatuses.Active 
                    && role.RoleType.Key==(int)SAHL.Common.Globals.RoleTypes.MainApplicant
                    && role.Account.AccountStatus.Key != (int)SAHL.Common.Globals.AccountStatuses.ApplicationpriortoInstructAttorney)
                {
                    _accounts.Add(role.Account);
                    foreach (IFinancialService fs in role.Account.FinancialServices)
            	    {
                        foreach (IFinancialAdjustment fa in fs.FinancialAdjustments)
                        {
                            _financialAdjustments.Add(fa);
                        }
		            }
                }
            }

            // Bind the Products
            _view.BindSummaryGrid(_accounts);

            // Sort the FADJ's    
            IList<IFinancialAdjustment> financialAdjustmentsSorted = _financialAdjustments.OrderBy(x => x.FinancialAdjustmentStatus.Key).ThenBy(y => y.FromDate).ToList<IFinancialAdjustment>();

            // Bind the Rate Overrides
            _view.BindFinancialAdjustmentGrid(financialAdjustmentsSorted);

        }

    }
}
