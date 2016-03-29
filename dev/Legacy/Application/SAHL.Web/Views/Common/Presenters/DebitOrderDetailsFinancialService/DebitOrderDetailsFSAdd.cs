using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.UI;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI.Events;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Web.Views.Common.Presenters.FutureDatedTransactions;

namespace SAHL.Web.Views.Common.Presenters.DebitOrderDetailsFinancialService
{
    /// <summary>
    ///
    /// </summary>
    public class DebitOrderDetailsFSAdd : DebitOrderDetailsFSBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DebitOrderDetailsFSAdd(IDebitOrderDetails view, SAHLCommonBaseController controller)
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
            if (!_view.ShouldRunPage) return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnAddButtonClicked += new EventHandler(_view_OnAddButtonClicked);
            //IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            //IFinancialService fs = FSR.GetFinancialServiceByKey(base.MenuNode.GenericKey);
            IFinancialService fs = FinancialService;
            _view.gridPostBackType = GridPostBackType.None;
            _view.BindDebitOrderDays();
            _view.ShowControls = true;
            _view.ForceShowBankAccountControl = true;

            int[] roleTypes = new int[2] { (int)SAHL.Common.Globals.RoleTypes.MainApplicant, (int)SAHL.Common.Globals.RoleTypes.Suretor };
            IReadOnlyEventList<ILegalEntity> legalEntities = fs.Account.GetLegalEntitiesByRoleType(_view.Messages, roleTypes);

            IEventList<IBankAccount> bankAccounts = new EventList<IBankAccount>();
            for (int i = 0; i < fs.FinancialServiceBankAccounts.Count; i++)
            {
                if (fs.FinancialServiceBankAccounts[i].GeneralStatus.Key == (int)GeneralStatuses.Active)
                    bankAccounts.Add(_view.Messages, fs.FinancialServiceBankAccounts[i].BankAccount);
            }

            for (int i = 0; i < legalEntities.Count; i++)
            {
                //todo check if legalEntity is active on this account

                for (int x = 0; x < legalEntities[i].LegalEntityBankAccounts.Count; x++)
                {
                    if (legalEntities[i].LegalEntityBankAccounts[x].BankAccount != null
                      && legalEntities[i].LegalEntityBankAccounts[x].GeneralStatus.Key == (int)GeneralStatuses.Active)
                    {
                        bankAccounts.Add(_view.Messages, legalEntities[i].LegalEntityBankAccounts[x].BankAccount);
                    }
                }
            }

            _view.BindBankAccountControl(bankAccounts);

            _view.BindGrid(fs);
            _view.BindPaymentTypes();
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

            _view.ShowButtons = true;

            _view.ShowLabels = false;
            _view.SetEffectiveDateToCurrentDate = true;
            _view.ButtonAddVisible = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "New rule added to FxCop, avoiding refactor")]
        private void _view_OnAddButtonClicked(object sender, EventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
                IRuleService svc = ServiceFactory.GetService<IRuleService>();
                svc.ExecuteRule(spc.DomainMessages, "DebitOrderFinancialServiceCheck", base.FinancialService.Account);

                if (_view.Messages.Count > 0)
                    throw new DomainValidationException();

                foreach (IFinancialService _finServ in base.FinancialService.Account.FinancialServices)
                {
                    if ((_finServ.AccountStatus.Key == (int)AccountStatuses.Open || _finServ.AccountStatus.Key == (int)AccountStatuses.Dormant)
                        && (_finServ.FinancialServiceType.Key == (int)FinancialServiceTypes.VariableLoan
                             || _finServ.FinancialServiceType.Key == (int)FinancialServiceTypes.FixedLoan
                             || _finServ.FinancialServiceType.Key == (int)FinancialServiceTypes.PersonalLoan))
                    {
                        IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
                        ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                        IFutureDatedChangeRepository FDCR = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();

                        IFinancialServiceBankAccount fsbAccount = null;

                        //Create a new FinancialServiceBankAccount and set it's status to Inactive
                        fsbAccount = FSR.GetEmptyFinancialServiceBankAccount();
                        //fsbAccount.FinancialService = base.FinancialService;
                        fsbAccount.FinancialService = _finServ;
                        if (_view.PaymentTypeKey != SAHLDropDownList.PleaseSelectValue)
                            fsbAccount.FinancialServicePaymentType = LR.FinancialServicePaymentTypes.ObjectDictionary[_view.PaymentTypeKey];

                        IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();

                        int doDay = -1;
                        Int32.TryParse(_view.DODay, out doDay);

                        int bankKey = -1;
                        IBankAccount ba = null;
                        if (int.TryParse(_view.BankAccountKey, out bankKey))
                        {
                            ba = BAR.GetBankAccountByKey(int.Parse(_view.BankAccountKey));
                        }

                        IGeneralStatus statusInactive = LR.GeneralStatuses[GeneralStatuses.Inactive];

                        fsbAccount.BankAccount = ba;
                        fsbAccount.ChangeDate = DateTime.Today;
                        if (_view.EffectDate.HasValue)
                            fsbAccount.SetDebitOrderDay(_view.EffectDate.Value, doDay);
                        else
                            fsbAccount.SetDebitOrderDay(DateTime.MinValue, doDay);

                        fsbAccount.GeneralStatus = statusInactive;
                        fsbAccount.UserID = _view.CurrentPrincipal.Identity.Name;

                        // Only set IsNaedoCompliant to 'True' if the PaymentType is Debit Order

                        int intResult;
                        int.TryParse(_view.PaymentTypeKey, out intResult);
                        if (intResult == (int)SAHL.Common.Globals.FinancialServicePaymentTypes.DebitOrderPayment)
                            fsbAccount.IsNaedoCompliant = true;
                        else
                            fsbAccount.IsNaedoCompliant = false;

                        ICommonRepository commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();
                        fsbAccount.ProviderKey = commonRepo.GetDefaultDebitOrderProviderKey();

                        _finServ.FinancialServiceBankAccounts.Add(_view.Messages, fsbAccount);

                        FSR.SaveFinancialService(_finServ);

                        IFutureDatedChange futureDatedChange = CreateNewFutureDatedChange(_finServ);

                        IFutureDatedChangeDetail futureDatedChangeDetail = FDCR.CreateEmptyFutureDatedChangeDetail();
                        futureDatedChangeDetail.FutureDatedChange = futureDatedChange;
                        futureDatedChangeDetail.Action = 'U';
                        futureDatedChangeDetail.ChangeDate = DateTime.Today;
                        futureDatedChangeDetail.ColumnName = "DebitOrderDay";
                        futureDatedChangeDetail.TableName = "FinancialServiceBankAccount";
                        futureDatedChangeDetail.Value = doDay.ToString();
                        futureDatedChangeDetail.UserID = _view.CurrentPrincipal.Identity.Name;
                        futureDatedChangeDetail.ReferenceKey = fsbAccount.Key;
                        futureDatedChange.FutureDatedChangeDetails.Add(_view.Messages, futureDatedChangeDetail);

                        // get the key of the current active FSBA so we have a starting point when deactivating previous
                        // records
                        int fsbaCurrentKey = 0;
                        foreach (IFinancialServiceBankAccount fsba in _finServ.FinancialServiceBankAccounts)
                        {
                            if (fsba.GeneralStatus.Key == (int)GeneralStatuses.Active)
                            {
                                fsbaCurrentKey = fsba.Key;
                                break;
                            }
                        }

                        // now loop through all the FSBAs and if they're the current one OR anything subsequent to that,
                        // make sure they get deactivated - you must do ALL of them in case an intermediate debit order is
                        // deleted before it becomes active
                        foreach (IFinancialServiceBankAccount fsba in _finServ.FinancialServiceBankAccounts)
                        {
                            // make sure ALL financial service bank account records created before this one are deactivated
                            if (fsba.Key < fsbAccount.Key && fsba.Key >= fsbaCurrentKey)
                            {
                                //set the existing FinancialServiceBankAccount to inactive on the effective date
                                IFutureDatedChangeDetail futureDatedChangeDetail2 = FDCR.CreateEmptyFutureDatedChangeDetail();
                                futureDatedChangeDetail2.FutureDatedChange = futureDatedChange;
                                futureDatedChangeDetail2.Action = 'U';
                                futureDatedChangeDetail2.ChangeDate = DateTime.Today;
                                futureDatedChangeDetail2.ColumnName = "GeneralStatusKey";
                                futureDatedChangeDetail2.TableName = "FinancialServiceBankAccount";

                                futureDatedChangeDetail2.Value = statusInactive.Key.ToString();
                                futureDatedChangeDetail2.ReferenceKey = fsba.Key;
                                futureDatedChangeDetail2.UserID = _view.CurrentPrincipal.Identity.Name;
                                futureDatedChange.FutureDatedChangeDetails.Add(_view.Messages, futureDatedChangeDetail2);
                            }
                        }
                        IGeneralStatus statusActive = LR.GeneralStatuses[GeneralStatuses.Active];

                        //Set the newly created FinancialServiceBankAccount to Active on the effectiveDate
                        IFutureDatedChangeDetail futureDatedChangeDetail3 = FDCR.CreateEmptyFutureDatedChangeDetail();
                        futureDatedChangeDetail3.FutureDatedChange = futureDatedChange;
                        futureDatedChangeDetail3.Action = 'U';
                        futureDatedChangeDetail3.ChangeDate = DateTime.Today;
                        futureDatedChangeDetail3.ColumnName = "GeneralStatusKey";
                        futureDatedChangeDetail3.TableName = "FinancialServiceBankAccount";
                        futureDatedChangeDetail3.Value = statusActive.Key.ToString();
                        futureDatedChangeDetail3.ReferenceKey = fsbAccount.Key;
                        futureDatedChangeDetail3.UserID = _view.CurrentPrincipal.Identity.Name;
                        futureDatedChange.FutureDatedChangeDetails.Add(_view.Messages, futureDatedChangeDetail3);

                        IFutureDatedChangeDetail futureDatedChangeDetail4 = FDCR.CreateEmptyFutureDatedChangeDetail();
                        futureDatedChangeDetail4.FutureDatedChange = futureDatedChange;
                        futureDatedChangeDetail4.Action = 'U';
                        futureDatedChangeDetail4.ChangeDate = DateTime.Today;
                        futureDatedChangeDetail4.ColumnName = "FinancialServicePaymentTypeKey";
                        futureDatedChangeDetail4.TableName = "FinancialServiceBankAccount";
                        futureDatedChangeDetail4.Value = _view.PaymentTypeKey;
                        futureDatedChangeDetail4.ReferenceKey = fsbAccount.Key;
                        futureDatedChangeDetail4.UserID = _view.CurrentPrincipal.Identity.Name;
                        futureDatedChange.FutureDatedChangeDetails.Add(_view.Messages, futureDatedChangeDetail4);
                        FDCR.SaveFutureDatedChange(futureDatedChange);
                    }
                }
                ts.VoteCommit();
                _view.Navigator.Navigate("DebitOrderDetails");
            }
            catch (Exception)
            {
                ts.VoteRollBack();
                if (_view.IsValid)
                {
                    throw;
                }
            }
            finally
            {
                ts.Dispose();
            }
        }

        private IFutureDatedChange CreateNewFutureDatedChange(IFinancialService fs)
        {
            ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
            IFutureDatedChangeRepository FDCR = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();

            IFutureDatedChange futureDatedChange = FDCR.CreateEmptyFutureDatedChange();
            if (_view.EffectDate.HasValue)
                futureDatedChange.EffectiveDate = _view.EffectDate.Value;
            futureDatedChange.ChangeDate = DateTime.Today;

            futureDatedChange.IdentifierReferenceKey = fs.Key;

            futureDatedChange.FutureDatedChangeType = LR.FutureDatedChangeTypes.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.FutureDatedChangeTypes.NormalDebitOrder)];

            futureDatedChange.InsertDate = DateTime.Today;

            futureDatedChange.UserID = _view.CurrentPrincipal.Identity.Name;

            return futureDatedChange;
        }

        private void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("DebitOrderDetails");
        }
    }
}