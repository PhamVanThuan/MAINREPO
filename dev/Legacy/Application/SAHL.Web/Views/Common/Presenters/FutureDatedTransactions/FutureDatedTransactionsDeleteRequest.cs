using System;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.UI;
using SAHL.Common;
using SAHL.Common.Globals;
using System.Collections.Generic;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Web.Views.Common.Presenters.FutureDatedTransactions
{
    /// <summary>
    /// 
    /// </summary>
    public class FutureDatedTransactionsDeleteRequest : FutureDatedTransactionsBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public FutureDatedTransactionsDeleteRequest(IFutureDatedTransactions view, SAHLCommonBaseController controller)
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
			if (!_view.ShouldRunPage) return;

            InstanceNode node = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal, CBONodeSetType.X2) as InstanceNode;
            if (node == null)
                throw new NullReferenceException(StaticMessages.NullCurrentCBONode);

            IX2Service x2svc = ServiceFactory.GetService<IX2Service>();
            IDictionary<string, object> deleteDebitOrderRequest = x2svc.GetX2DataRow((long)node.InstanceID);

			if (deleteDebitOrderRequest.Count > 0)
			{
				if (!Convert.ToBoolean(deleteDebitOrderRequest["RequestApproved"]))
				{
					int manualDebitOrderKey = int.Parse(string.IsNullOrEmpty(deleteDebitOrderRequest["DebitOrderKey"].ToString()) ? "0" : deleteDebitOrderRequest["DebitOrderKey"].ToString());

                    if (manualDebitOrderKey != 0)
					{
                        IManualDebitOrder recurringTran = ManualDebitOrderRepository.GetManualDebitOrderByKey(manualDebitOrderKey);

                        IEventList<IManualDebitOrder> lstManualDebitOrder = new EventList<IManualDebitOrder>();
                        lstManualDebitOrder.Add(_view.Messages, recurringTran);

                        if (lstManualDebitOrder.Count == 1)
						{
                            _view.BindOrdersToGrid(lstManualDebitOrder);

							IFinancialService fs = FinancialServiceRepo.GetFinancialServiceByKey(recurringTran.FinancialService.Key);
							int[] roleTypes = new int[3] { (int)SAHL.Common.Globals.RoleTypes.AssuredLife, (int)SAHL.Common.Globals.RoleTypes.MainApplicant, (int)SAHL.Common.Globals.RoleTypes.Suretor };
							IReadOnlyEventList<ILegalEntity> lstLegalEntities = fs.Account.GetLegalEntitiesByRoleType(_view.Messages, roleTypes);
							foreach (ILegalEntity le in lstLegalEntities)
							{
								foreach (ILegalEntityBankAccount ba in le.LegalEntityBankAccounts)
								{
									_view.LegalEntityBankAccounts.Add(ba);
								}
							}
						}
					}
				}
				_view.lblRequestedByText = string.IsNullOrEmpty(deleteDebitOrderRequest["RequestUser"].ToString()) ? "" : deleteDebitOrderRequest["RequestUser"].ToString();
				_view.lblProcessedByText = string.IsNullOrEmpty(deleteDebitOrderRequest["ProcessUser"].ToString()) ? "" : deleteDebitOrderRequest["ProcessUser"].ToString();
				_view.ShowLabels = true;
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
			if (!_view.ShouldRunPage) return;

			_view.ShowButtons = false;
			_view.ArrearBalanceRowVisible = false;
            _view.ControlsVisible = true;
			_view.tdRequestedByVisible = true;
			_view.lblRequestedByVisible = true;
			_view.tdProcessedByVisible = true;
			_view.lblProcessedByVisible = true;
		}
	}
}
