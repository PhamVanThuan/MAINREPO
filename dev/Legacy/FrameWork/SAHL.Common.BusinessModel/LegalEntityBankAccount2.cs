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
using SAHL.Common.DomainMessages;
using SAHL.Common.Globals;
using SAHL.Common.Security;
namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LegalEntityBankAccount : BusinessModelBase<SAHL.Common.BusinessModel.DAO.LegalEntityBankAccount_DAO>, ILegalEntityBankAccount
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("BankAccountDebitOrderDoNotDelete");
        }

        /// <summary>
        /// Business Rule: Cannot deactivate the link between the Legal Entity and the Bank Account
        /// if the Bank Account is used in one of the account debit order(s).
        /// </summary>
        public IGeneralStatus GeneralStatus
        {
            get
            {
                if (null == _DAO.GeneralStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
                }
            }

            set
            {
                // Is this bank account being used in one of the account debit order(s).
//                if (((IGeneralStatus)value).Key == (int)GeneralStatuses.Inactive)
//                {
//#warning Not sure if I should use the object or the key here (should test this)!
//                    FinancialServiceBankAccount_DAO[] financialServiceBankAccount_DAOs = FinancialServiceBankAccount_DAO.FindAllByProperty("BankAccountKey", this.BankAccount.Key);
//                    foreach (FinancialServiceBankAccount_DAO financialServiceBankAccount_DAO in financialServiceBankAccount_DAOs)
//                    {
//                        if (financialServiceBankAccount_DAO.FinancialService.AccountStatus.Key == (int)SAHL.Common.Globals.AccountStatuses.Open)
//                        {
//                            string msg = "Cannot deactivate the link between the Legal Entity and the Bank Account. The Bank Account is being used in one of the Mortgage Account debit order(s).";

//                            SAHLPrincipalCache SPC = SAHLPrincipal.GetSAHLPrincipalCache();
//                            spc.DomainMessages.Add(new Error(msg, msg));

//                            return;
//                        }
//                    }
//                }

                if (value == null)
                {
                    _DAO.GeneralStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}


