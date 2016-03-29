using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Web.Views.Life.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Web.Views.Life.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class LifeHelper
    {
        private SAHL.Common.BusinessModel.Interfaces.IAccount _account;
        //private IApplication _application;
        //private ILifeRepository _lifeRepo;
        //private IApplicationRepository _applicationRepo;
        private IAccountRepository _accountRepo;

        /// <summary>
        /// 
        /// </summary>
        public LifeHelper()
        {
            //_lifeRepo = RepositoryFactory.GetRepository<ILifeRepository>();
            //_applicationRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            _accountRepo = RepositoryFactory.GetRepository<IAccountRepository>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericKeyType">SAHL.Common.Globals.GenericKeyTypes</param>
        /// <param name="genericKey">int</param>
        /// <returns></returns>
        public IAccountLifePolicy GetLifeAccount(SAHL.Common.Globals.GenericKeyTypes genericKeyType, int genericKey)
        {
            IAccountLifePolicy lifePolicyAccount;

            // get the account
            switch (genericKeyType)
            {
                case SAHL.Common.Globals.GenericKeyTypes.Account:
                    _account = _accountRepo.GetAccountByKey(genericKey);
                   break;
               case SAHL.Common.Globals.GenericKeyTypes.FinancialService:
                   {
                       IFinancialServiceRepository FSR = RepositoryFactory.GetRepository<IFinancialServiceRepository>();
                       IFinancialService fs = FSR.GetFinancialServiceByKey(genericKey);
                       _account = fs.Account;// _accountRepo.GetAccountByFinancialServiceKey(messages, genericKey);
                   }
                   break;
                case SAHL.Common.Globals.GenericKeyTypes.Offer:
                    _account = _accountRepo.GetAccountByApplicationKey(genericKey);
                    break;
                default:
                    break;
            }

            if (_account is IAccountLifePolicy)
                lifePolicyAccount = _account as IAccountLifePolicy;
            else
                lifePolicyAccount = null;

            return lifePolicyAccount;
        }


    }
}
