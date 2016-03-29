using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Configuration;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System;
using System.Configuration;
using System.Linq;

namespace SAHL.Web.Views.PersonalLoan.Presenters
{
    public class RedirectPersonalLoanLifePolicyType : SAHLCommonBasePresenter<IRedirect>
    {
        private int _genericKey, _genericKeyTypeKey;

        public RedirectPersonalLoanLifePolicyType(IRedirect view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            // get the application or account key from the cbo
            CBONode CurrentNode = CBOManager.GetCurrentCBONode(_view.CurrentPrincipal);
            _genericKey = CurrentNode.GenericKey;
            _genericKeyTypeKey = CurrentNode.GenericKeyTypeKey;

            if (_genericKeyTypeKey != -1)
            {
                ILookupRepository _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                var GenKey = _lookupRepository.GenericKeyType.ObjectDictionary[_genericKeyTypeKey.ToString()];

                if (GenKey.TableName != "[2AM].[dbo].[Offer]" && GenKey.TableName != "[2AM].[dbo].[Account]")
                {
                    throw new Exception("This presenter only handles personal loan application or account objects.");
                }
                else
                {
                    string discriminationName;
                    if (HasSAHLLifeApplied(_genericKey, _genericKeyTypeKey))
                    {
                        discriminationName = "SAHLLifePolicyAppliedToApplication";
                    }
                    else
                    {
                        discriminationName = "SAHLLifePolicyNotAppliedToApplication";
                    }

                    SAHLRedirectionSection RedirectionSection = (SAHLRedirectionSection)ConfigurationManager.GetSection("RedirectionConfiguration");
                    if (RedirectionSection != null)
                    {
                        RedirectionElement Redirect = RedirectionSection.GetRedirection(discriminationName, base._view.ViewName);
                        base._view.Navigator.Navigate(Redirect.NavigationView);
                    }
                }
            }
            else
            {
                throw new Exception("GenericKeyType is not specified.");
            }
        }

        private bool HasSAHLLifeApplied(int genericKey, int genericKeyType)
        {
            var result = true;
            try
            {
                switch (genericKeyType)
                {
                    case (int)SAHL.Common.Globals.GenericKeyTypes.Account:
                        result = CheckAccountHasSAHLLifeApplied(genericKey);
                        break;

                    case (int)SAHL.Common.Globals.GenericKeyTypes.Offer:
                        var applicationUnsecuredLendingRepository = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                        result = applicationUnsecuredLendingRepository.ApplicationHasSAHLLifeApplied(genericKey);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
                // Avoiding showing the exception thrown when one tries to determine applied life type for a lead
            }
            return result;
        }

        // DRY not straight forward as there is no corresponding accountUnsecuredLendingRepository to move this method   
        private bool CheckAccountHasSAHLLifeApplied(int accountKey)
        {
            bool hasSAHLLifeApplied = false;
            var accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
            var genericAccount = accountRepository.GetAccountByKey(Convert.ToInt32(accountKey));
            var creditProtectionAccount = genericAccount as IAccountCreditProtectionPlan;
            if (creditProtectionAccount != null && creditProtectionAccount.FinancialServices.Count > 0)
            {
                IFinancialService SAHLCreditProtectionPlan = creditProtectionAccount.FinancialServices.Single(x => x.FinancialServiceType.Key == (int)FinancialServiceTypes.SAHLCreditProtectionPlan);
                if (SAHLCreditProtectionPlan != null)
                {
                    hasSAHLLifeApplied = true;
                }
            }
            var accountPersonalLoan = genericAccount as IAccountPersonalLoan;
            if (accountPersonalLoan != null)
            {
                if (accountPersonalLoan.ExternalLifePolicy != null)
                {
                    hasSAHLLifeApplied = false;
                }
            }

            return hasSAHLLifeApplied;
        }
    }
}