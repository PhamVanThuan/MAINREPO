using System;
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
    public class DebitOrderDetailsFSUpdate : DebitOrderDetailsFSBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public DebitOrderDetailsFSUpdate(IDebitOrderDetails view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            if (!_view.ShouldRunPage) return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnUpdateButtonClicked += new KeyChangedEventHandler(_view_OnUpdateButtonClicked);
            //_view.OnGridSelectedIndexChanged += new KeyChangedEventHandler(_view_OnGridSelectedIndexChanged);

            _view.SetControlsToGrid = true;
            _view.BindBankAccountControl(base.BankAccounts);
            _view.BindGrid(base.FinancialService);
            _view.BindPaymentTypes();
            _view.BindDebitOrderDays();
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

            if (_view.Messages.Count > 0)
                _view.BindControlsToGrid();

            _view.ShowButtons = true;
            _view.ShowControls = true;
            _view.ShowLabels = false;
            _view.gridPostBackType = GridPostBackType.SingleClick;
            _view.ButtonUpdateVisible = true;
            _view.Ignore = true;
        }

        private void _view_OnUpdateButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                BuildMappingTable();
                IFutureDatedChangeRepository FDCR = RepositoryFactory.GetRepository<IFutureDatedChangeRepository>();

                IFutureDatedChangeDetail futureDatedChangeDetail = null;

                //is this a change to a future dated change
                if (_view.DetailKey != -1 && _view.FutureDatedChangeKey != -1)
                    futureDatedChangeDetail = FDCR.GetFutureDatedChangeDetailByKey(_view.DetailKey);

                //create the update records one at a time if required
                IFutureDatedChange fdc = null;
                IFutureDatedChange fdcFixed = null;

                if (futureDatedChangeDetail != null)
                {
                    fdc = futureDatedChangeDetail.FutureDatedChange;
                    if (FDCMap.ContainsKey(fdc.Key))
                        fdcFixed = FDCR.GetFutureDatedChangeByKey(Convert.ToInt32(FDCMap[fdc.Key]));

                    foreach (IFutureDatedChangeDetail det in fdc.FutureDatedChangeDetails)
                    {
                        IFutureDatedChangeDetail detFixed = null;
                        if (FDCDMap.ContainsKey(det.Key))
                            detFixed = FDCR.GetFutureDatedChangeDetailByKey(Convert.ToInt32(FDCDMap[det.Key]));

                        switch (det.ColumnName)
                        {
                            case "DebitOrderDay":
                                {
                                    if (det.Value != _view.DODay)
                                    {
                                        det.ChangeDate = DateTime.Today;
                                        det.Value = _view.DODay;
                                        det.UserID = _view.CurrentPrincipal.Identity.Name;

                                        UpdateFinancialServiceBankAccount(det);

                                        if (fdcFixed != null && detFixed != null)
                                        {
                                            detFixed.ChangeDate = DateTime.Today;
                                            detFixed.Value = _view.DODay;
                                            detFixed.UserID = _view.CurrentPrincipal.Identity.Name;

                                            UpdateFinancialServiceBankAccount(detFixed);
                                        }
                                    }
                                    break;
                                }
                            case "FinancialServicePaymentTypeKey":
                                {
                                    if (det.Value != _view.PaymentTypeKey)
                                    {
                                        det.ChangeDate = DateTime.Today;
                                        det.Value = _view.PaymentTypeKey;
                                        det.UserID = _view.CurrentPrincipal.Identity.Name;
                                        if (_view.PaymentTypeKey == Convert.ToString((int)SAHL.Common.Globals.FinancialServicePaymentTypes.DebitOrderPayment))
                                        {
                                            int key = -1;
                                            if (int.TryParse(_view.BankAccountKey, out key) == false)
                                            {
                                                _view.Messages.Add(new Error("Please select a bank account.", "Please select a bank account."));
                                                return;
                                            }

                                            IFutureDatedChangeDetail futureDatedChangeDetail2 = FDCR.CreateEmptyFutureDatedChangeDetail();
                                            futureDatedChangeDetail2.FutureDatedChange = fdc;
                                            futureDatedChangeDetail2.Action = 'U';
                                            futureDatedChangeDetail2.ChangeDate = DateTime.Today;
                                            futureDatedChangeDetail2.ColumnName = "BankAccountKey";
                                            futureDatedChangeDetail2.TableName = "FinancialServiceBankAccount";
                                            futureDatedChangeDetail2.Value = _view.BankAccountKey;
                                            futureDatedChangeDetail2.UserID = _view.CurrentPrincipal.Identity.Name;
                                            IFinancialServiceBankAccount fsbAccount = null;
                                            foreach (IFinancialServiceBankAccount fsba in base.FinancialService.FinancialServiceBankAccounts)
                                            {
                                                if (fsba.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Inactive)
                                                {
                                                    fsbAccount = fsba;
                                                }
                                            }
                                            futureDatedChangeDetail2.ReferenceKey = fsbAccount.Key;
                                            fdc.FutureDatedChangeDetails.Add(_view.Messages, futureDatedChangeDetail2);
                                        }

                                        if (fdcFixed != null && detFixed != null)
                                        {
                                            detFixed.ChangeDate = DateTime.Today;
                                            detFixed.Value = _view.PaymentTypeKey;
                                            detFixed.UserID = _view.CurrentPrincipal.Identity.Name;
                                            if (_view.PaymentTypeKey == Convert.ToString((int)SAHL.Common.Globals.FinancialServicePaymentTypes.DebitOrderPayment))
                                            {
                                                int Fkey = -1;
                                                if (int.TryParse(_view.BankAccountKey, out Fkey) == false)
                                                {
                                                    _view.Messages.Add(new Error("Please select a bank account.", "Please select a bank account."));
                                                    return;
                                                }

                                                IFutureDatedChangeDetail futureDatedChangeDetailF2 = FDCR.CreateEmptyFutureDatedChangeDetail();
                                                futureDatedChangeDetailF2.FutureDatedChange = fdcFixed;
                                                futureDatedChangeDetailF2.Action = 'U';
                                                futureDatedChangeDetailF2.ChangeDate = DateTime.Today;
                                                futureDatedChangeDetailF2.ColumnName = "BankAccountKey";
                                                futureDatedChangeDetailF2.TableName = "FinancialServiceBankAccount";
                                                futureDatedChangeDetailF2.Value = _view.BankAccountKey;
                                                futureDatedChangeDetailF2.UserID = _view.CurrentPrincipal.Identity.Name;

                                                fdcFixed.FutureDatedChangeDetails.Add(_view.Messages, futureDatedChangeDetailF2);
                                            }
                                        }
                                    }
                                    break;
                                }
                        }

                        if (fdcFixed != null && detFixed != null)
                            fdcFixed = UpdateFixedFDC(fdcFixed, detFixed);

                        if (_view.EffectDate.HasValue && fdc.EffectiveDate != _view.EffectDate)
                        {
                            if (fdcFixed != null && detFixed != null)
                                fdcFixed.EffectiveDate = _view.EffectDate.Value;

                            fdc.EffectiveDate = _view.EffectDate.Value;
                        }

                        if (fdcFixed != null && detFixed != null)
                            FDCR.SaveFutureDatedChange(fdcFixed);

                        FDCR.SaveFutureDatedChange(fdc);
                    }
                }
                else
                {
                    CreateNewFinancialServiceBankAccount();
                }

                ts.VoteCommit();
                _view.Navigator.Navigate("DebitOrderDetails");
            }
            catch (Exception)
            {
                ts.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                ts.Dispose();
            }
        }

        private void UpdateFinancialServiceBankAccount(IFutureDatedChangeDetail det)
        {
            // get the finservicebankaccount record
            IFinancialServiceRepository fsRepo = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            IFinancialServiceBankAccount fsbAccount = fsRepo.GetFinancialServiceBankAccountByKey(det.ReferenceKey);
            if (fsbAccount != null)
            {
                // update fsb with new details
                fsbAccount.ChangeDate = det.ChangeDate;
                fsbAccount.DebitOrderDay = Convert.ToInt32(det.Value);
                if (_view.PaymentTypeKey != SAHLDropDownList.PleaseSelectValue)
                    fsbAccount.FinancialServicePaymentType = lookupRepo.FinancialServicePaymentTypes.ObjectDictionary[_view.PaymentTypeKey];
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "New rule added to FxCop, avoiding refactor")]
        private void CreateNewFinancialServiceBankAccount()
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

                    SetFinancialServiceBankAccountIsNaedoCompliant(_finServ, fsbAccount);

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

                    foreach (IFinancialServiceBankAccount fsba in _finServ.FinancialServiceBankAccounts)
                    {
                        if (fsba.GeneralStatus.Key == (int)SAHL.Common.Globals.GeneralStatuses.Active)
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
                            break;
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
                    //
                    FDCR.SaveFutureDatedChange(futureDatedChange);
                }
            }
        }

        private void SetFinancialServiceBankAccountIsNaedoCompliant(IFinancialService _finServ, IFinancialServiceBankAccount fsbAccount)
        {
            switch (int.Parse(_view.PaymentTypeKey))
            {
                // If the user changes the Payment Type to 'Debit Order Payment' 
                // or Bank Account has changed and the Payment Type is 'Debit Order Payment', set IsNaedoCompliant to 1
                // otherwise the IsNaedoCompliant indicator remains the same as it was on the original
                case (int)SAHL.Common.Globals.FinancialServicePaymentTypes.DebitOrderPayment:
                    if (_finServ.CurrentBankAccount.FinancialServicePaymentType.Key.ToString() != _view.PaymentTypeKey
                     || _finServ.CurrentBankAccount.BankAccount.Key.ToString() != _view.BankAccountKey)
                        fsbAccount.IsNaedoCompliant = true;
                    else
                        fsbAccount.IsNaedoCompliant = _finServ.CurrentBankAccount.IsNaedoCompliant;
                    break;
                // If the user selects Payment Type of 'Direct Payment' or 'Subsidy Payment', set IsNaedoCompliant to 0
                case (int)SAHL.Common.Globals.FinancialServicePaymentTypes.DirectPayment:
                case (int)SAHL.Common.Globals.FinancialServicePaymentTypes.SubsidyPayment:
                    fsbAccount.IsNaedoCompliant = false;
                    break;
                default:
                    break;
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