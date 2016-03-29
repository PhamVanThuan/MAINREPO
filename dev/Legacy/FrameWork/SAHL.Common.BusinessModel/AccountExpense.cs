using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO
    /// </summary>
    public partial class AccountExpense : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AccountExpense_DAO>, IAccountExpense
    {
        public AccountExpense(SAHL.Common.BusinessModel.DAO.AccountExpense_DAO AccountExpense)
            : base(AccountExpense)
        {
            this._DAO = AccountExpense;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.ExpenseAccountNumber
        /// </summary>
        public String ExpenseAccountNumber
        {
            get { return _DAO.ExpenseAccountNumber; }
            set { _DAO.ExpenseAccountNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.ExpenseAccountName
        /// </summary>
        public String ExpenseAccountName
        {
            get { return _DAO.ExpenseAccountName; }
            set { _DAO.ExpenseAccountName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.ExpenseReference
        /// </summary>
        public String ExpenseReference
        {
            get { return _DAO.ExpenseReference; }
            set { _DAO.ExpenseReference = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.TotalOutstandingAmount
        /// </summary>
        public Double TotalOutstandingAmount
        {
            get { return _DAO.TotalOutstandingAmount; }
            set { _DAO.TotalOutstandingAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.MonthlyPayment
        /// </summary>
        public Double MonthlyPayment
        {
            get { return _DAO.MonthlyPayment; }
            set { _DAO.MonthlyPayment = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.ToBeSettled
        /// </summary>
        public Boolean ToBeSettled
        {
            get { return _DAO.ToBeSettled; }
            set { _DAO.ToBeSettled = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.OverRidden
        /// </summary>
        public Boolean OverRidden
        {
            get { return _DAO.OverRidden; }
            set { _DAO.OverRidden = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.AccountDebtSettlements
        /// </summary>
        private DAOEventList<AccountDebtSettlement_DAO, IAccountDebtSettlement, AccountDebtSettlement> _AccountDebtSettlements;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.AccountDebtSettlements
        /// </summary>
        public IEventList<IAccountDebtSettlement> AccountDebtSettlements
        {
            get
            {
                if (null == _AccountDebtSettlements)
                {
                    if (null == _DAO.AccountDebtSettlements)
                        _DAO.AccountDebtSettlements = new List<AccountDebtSettlement_DAO>();
                    _AccountDebtSettlements = new DAOEventList<AccountDebtSettlement_DAO, IAccountDebtSettlement, AccountDebtSettlement>(_DAO.AccountDebtSettlements);
                    _AccountDebtSettlements.BeforeAdd += new EventListHandler(OnAccountDebtSettlements_BeforeAdd);
                    _AccountDebtSettlements.BeforeRemove += new EventListHandler(OnAccountDebtSettlements_BeforeRemove);
                    _AccountDebtSettlements.AfterAdd += new EventListHandler(OnAccountDebtSettlements_AfterAdd);
                    _AccountDebtSettlements.AfterRemove += new EventListHandler(OnAccountDebtSettlements_AfterRemove);
                }
                return _AccountDebtSettlements;
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.Account
        /// </summary>
        public IAccount Account
        {
            get
            {
                if (null == _DAO.Account) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IAccount, Account_DAO>(_DAO.Account);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Account = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Account = (Account_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.ExpenseType
        /// </summary>
        public IExpenseType ExpenseType
        {
            get
            {
                if (null == _DAO.ExpenseType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IExpenseType, ExpenseType_DAO>(_DAO.ExpenseType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ExpenseType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ExpenseType = (ExpenseType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountExpense_DAO.LegalEntity
        /// </summary>
        public ILegalEntity LegalEntity
        {
            get
            {
                if (null == _DAO.LegalEntity) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ILegalEntity, LegalEntity_DAO>(_DAO.LegalEntity);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.LegalEntity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.LegalEntity = (LegalEntity_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _AccountDebtSettlements = null;
        }
    }
}