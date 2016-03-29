using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Common.Interfaces;
using SAHL.Common.Web.UI;
using Castle.ActiveRecord;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.CacheData;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Common.Presenters.Banking.BankingDetailsSearch
{    
    public class BankingDetailsSearchLegalEntity : BankingDetailsSearchBase
    {        

        public BankingDetailsSearchLegalEntity(IBankingDetailsSearch view, SAHLCommonBaseController controller)
            : base(view, controller)
        {

        }

        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);
            _view.OnUseButtonClicked += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnUseButtonClicked);

        }

        void _view_OnUseButtonClicked(object sender, SAHL.Common.Web.UI.Events.KeyChangedEventArgs e)
        {
            TransactionScope ts = new TransactionScope();
            try
            {
                ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();               
                ILegalEntity le = null;

                if (GlobalCacheData.ContainsKey(ViewConstants.LegalEntity))
                {
                    le = LER.GetLegalEntityByKey(int.Parse(GlobalCacheData[ViewConstants.LegalEntity].ToString()));
                }

                KeyChangedEventArgs arg = e as KeyChangedEventArgs;
                //SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(_view.CurrentPrincipal);
 
                    ILegalEntityBankAccount leBA = LER.GetEmptyLegalEntityBankAccount();
                    int bankAccountKey = int.Parse(arg.Key.ToString());
                    IBankAccountRepository BAR = RepositoryFactory.GetRepository<IBankAccountRepository>();
                    IBankAccount ba = BAR.GetBankAccountByKey(bankAccountKey);
                    ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                    leBA.GeneralStatus = LR.GeneralStatuses[GeneralStatuses.Active];
                    leBA.BankAccount = ba;
                    leBA.LegalEntity = le;
                    le.LegalEntityBankAccounts.Add(_view.Messages, leBA);
                    ExclusionSets.Add(RuleExclusionSets.BankingDetailsViewLegalEntity);
                    LER.SaveLegalEntity(le, false);
                    ExclusionSets.Remove(RuleExclusionSets.BankingDetailsViewLegalEntity);

                    ts.VoteCommit();
                    _view.Navigator.Navigate("Use");
                
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
    }
}
