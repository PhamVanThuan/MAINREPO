using System;
using System.Collections.Generic;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.CacheData;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;

namespace SAHL.Web.Views.Common.Presenters
{
    /// <summary>
    /// 
    /// </summary>
    public class CallSummary : SAHLCommonBasePresenter<ICallSummary>
    {

		#region Private members

		protected Int64 _instanceID;
		protected int _legalEntityKey;
		protected ILookupRepository lookupRepo;
		protected IReadOnlyEventList<IHelpDeskQuery> _helpDeskQueryList;
		protected bool _update;
		protected int _gridSelectedIndex;
		protected string _cachedDataScreenModeAddingKey = "CachedDataScreenModeAdding";

		#endregion

        public CallSummary(ICallSummary view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
            {
            }
        }

        protected bool DoValidate(IHelpDeskQuery helpDeskQuery)
		{
			SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
			IDomainMessageCollection dmc = spc.DomainMessages;
			bool passed = true;

			if (String.IsNullOrEmpty(_view.ShortDescription))
			{
				string errorMessage = "Please enter a Description.";
				dmc.Add(new Error(errorMessage, errorMessage));
				passed = false;
			}

			if (String.IsNullOrEmpty(_view.SelectedCategory))
			{
				string errorMessage = "Please select a Category.";
				dmc.Add(new Error(errorMessage, errorMessage));
				passed = false;
			}

            if (String.IsNullOrEmpty(_view.SelectedStatus))
			{
				string errorMessage = "Please select a Status.";
				dmc.Add(new Error(errorMessage, errorMessage));
				passed = false;
			}

			if (String.IsNullOrEmpty(_view.SelectedMemoType) && !_update)
			{
				string errorMessage = "Please select a Query Type.";
				dmc.Add(new Error(errorMessage, errorMessage));
				passed = false;
			}

			if (!_update)
			{
				if ((!String.IsNullOrEmpty(_view.SelectedMemoType) && Convert.ToInt32(_view.SelectedMemoType) == (int)GenericKeyTypes.Account)
					&& (String.IsNullOrEmpty(_view.SelectedAccountNumber)))
				{
					string errorMessage = "Please select an Account Number.";
					dmc.Add(new Error(errorMessage, errorMessage));
					passed = false;
				}
			}
			else
			{
				if (helpDeskQuery.Memo.GenericKeyType.Key == (int)GenericKeyTypes.Account
					&& String.IsNullOrEmpty(_view.SelectedAccountNumber))
				{
					string errorMessage = "Please select an Account Number.";
					dmc.Add(new Error(errorMessage, errorMessage));
					passed = false;
				}
			}

			if (String.IsNullOrEmpty(_view.DetailDescription))
			{
				string errorMessage = "Please enter a Detailed Descrption.";
				dmc.Add(new Error(errorMessage, errorMessage));
				passed = false;
			}

			return passed;
		}

		/// <summary>
		/// 
		/// </summary>
		protected void BindStatusDropDown()
		{
			_view.BindStatusDropDown();
		}

		/// <summary>
		/// 
		/// </summary>
		protected void BindCategoryDropDown(int selectedHelpDeskCategoryKey)
		{
			_view.BindCategoryDropDown(lookupRepo.HelpDeskCategoriesActive(selectedHelpDeskCategoryKey));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="legalEntityKey"></param>
		protected void BindAccountDropDown(int legalEntityKey)
		{
			ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
			ILegalEntity legalEntity = leRepo.GetLegalEntityByKey(legalEntityKey);
			IList<IAccount> accounts = new List<IAccount>();
			for (int i = 0; i < legalEntity.Roles.Count; i++)
			{
				if (legalEntity.Roles[i].RoleType.Key == Convert.ToInt32(RoleTypes.MainApplicant) &&
					legalEntity.Roles[i].GeneralStatus.Key == Convert.ToInt32(GeneralStatuses.Active) &&
					legalEntity.Roles[i].Account.AccountStatus.Key != Convert.ToInt32(AccountStatuses.Dormant))
				{
                    // must only get parent accounts here ie: exclude accounts that have related parent accounts
                    if (legalEntity.Roles[i].Account.ParentAccount == null)//if (legalEntity.Roles[i].Account.RelatedParentAccounts.Count == 0)
                    {
                        accounts.Add(legalEntity.Roles[i].Account);
                    }
				}
			}
			_view.BindAccountDropDown(accounts);
		}

		/// <summary>
		/// 
		/// </summary>
		protected void BindQueryTypeDropDown()
		{
			IList<IGenericKeyType> queryTypeList = new List<IGenericKeyType>();
			for (int i = 0; i < lookupRepo.GenericKeyType.Count; i++)
			{
				if (lookupRepo.GenericKeyType[i].Key == Convert.ToInt32(GenericKeyTypes.Account) ||
					lookupRepo.GenericKeyType[i].Key == Convert.ToInt32(GenericKeyTypes.LegalEntity))
					queryTypeList.Add(lookupRepo.GenericKeyType[i]);
			}
			_view.BindQueryTypeDropDown(queryTypeList);
		}
    }
}
