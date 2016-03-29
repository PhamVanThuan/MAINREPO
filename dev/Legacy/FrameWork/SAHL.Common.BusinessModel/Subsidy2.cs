using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Security;

using System.Security.Principal;
using SAHL.Common.DomainMessages;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class Subsidy : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Subsidy_DAO>, ISubsidy
	{
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("SubsidyDetailsValidateStopOrderAmount");
            Rules.Add("SubsidyDetailsMandatoryAccountOrApplication");
            Rules.Add("SubsidyDetailsMandatorySalaryNumber");
            Rules.Add("SubsidyDetailsUpdateSubsidyAmount");
        }

        ///// <summary>
        ///// 
        ///// </summary>
        public IAccount Account
        {
            get
            {
                if (null == _DAO.Accounts || _DAO.Accounts.Count == 0)
                    return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Accounts[0]);

                }
            }
            set
            {
                if (_DAO.Accounts == null)
                    _DAO.Accounts = new List<Account_DAO>();

                IList<Account_DAO> list = _DAO.Accounts as IList<Account_DAO>;
                if (list.Count > 0)
                    list.Clear();

                if (value != null)
                {
                    IDAOObject obj = value as IDAOObject;
                    if (obj != null)
                        list.Add((Account_DAO)obj.GetDAOObject());
                    else
                        throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
                }
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        public IApplication Application
        {
            get
            {
                if (null == _DAO.Applications || _DAO.Applications.Count == 0)
                    return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplication, Application_DAO>(_DAO.Applications[0]);

                }
            }
            set
            {
                if (_DAO.Applications == null)
                    _DAO.Applications = new List<Application_DAO>();

                IList<Application_DAO> list = _DAO.Applications as IList<Application_DAO>;
                if (list.Count > 0)
                    list.Clear();

                if (value != null)
                {
                    IDAOObject obj = value as IDAOObject;
                    if (obj != null)
                        list.Add((Application_DAO)obj.GetDAOObject());
                    else
                        throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
                }
            }
        }

        ///// <summary>
        ///// Adds new account to subsidy.account collection, which must always be a 1 item collection.
        ///// If the account to be set is different from the current account, all accounts records
        ///// are removed and the new account is added. This is because Subsidy.Account can only have one
        ///// account associated with it. 
        ///// </summary>
        ///// <param name="account"></param>
        //public void SetSubsidyAccount(IAccount account)
        //{
        //    //SAHLPrincipal currentPrincipal = new SAHLPrincipal(WindowsIdentity.GetCurrent());
        //    SAHLPrincipalCache spc = SAHLPrincipal.GetSAHLPrincipalCache(); //SAHLPrincipalCache.GetPrincipalCache(currentPrincipal);
        //    if (account.Key != this.Accounts[0].Key)
        //    {
        //        spc.DomainMessages.Add(new Warning("Are you sure you want to move the subsidy from account " + this._DAO.Accounts[0].Key + " to " + account.Key + " ?", ""));
        //        for (int i=0 ; i < this.Accounts.Count ; i++)
        //        {
        //            this.Accounts.Remove(spc.DomainMessages,this.Accounts[i]);
        //        }
        //        this.Accounts.Add(spc.DomainMessages, account);
        //    }
        //}

        protected void OnApplications_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnApplications_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnApplications_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnApplications_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnAccounts_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnAccounts_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }
        protected void OnAccounts_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnAccounts_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

	}
}


