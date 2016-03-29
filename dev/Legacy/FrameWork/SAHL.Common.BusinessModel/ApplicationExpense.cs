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
    /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO
    /// </summary>
    public partial class ApplicationExpense : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO>, IApplicationExpense
    {
        public ApplicationExpense(SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO ApplicationExpense)
            : base(ApplicationExpense)
        {
            this._DAO = ApplicationExpense;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.ExpenseAccountNumber
        /// </summary>
        public String ExpenseAccountNumber
        {
            get { return _DAO.ExpenseAccountNumber; }
            set { _DAO.ExpenseAccountNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.ExpenseAccountName
        /// </summary>
        public String ExpenseAccountName
        {
            get { return _DAO.ExpenseAccountName; }
            set { _DAO.ExpenseAccountName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.ExpenseReference
        /// </summary>
        public String ExpenseReference
        {
            get { return _DAO.ExpenseReference; }
            set { _DAO.ExpenseReference = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.TotalOutstandingAmount
        /// </summary>
        public Double TotalOutstandingAmount
        {
            get { return _DAO.TotalOutstandingAmount; }
            set { _DAO.TotalOutstandingAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.MonthlyPayment
        /// </summary>
        public Double MonthlyPayment
        {
            get { return _DAO.MonthlyPayment; }
            set { _DAO.MonthlyPayment = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.ToBeSettled
        /// </summary>
        public Boolean ToBeSettled
        {
            get { return _DAO.ToBeSettled; }
            set { _DAO.ToBeSettled = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.OverRidden
        /// </summary>
        public Boolean OverRidden
        {
            get { return _DAO.OverRidden; }
            set { _DAO.OverRidden = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.ExpenseType
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
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.LegalEntity
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

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.Application
        /// </summary>
        public IApplication Application
        {
            get
            {
                if (null == _DAO.Application) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplication, Application_DAO>(_DAO.Application);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Application = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Application = (Application_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.ApplicationDebtSettlements
        /// </summary>
        private DAOEventList<ApplicationDebtSettlement_DAO, IApplicationDebtSettlement, ApplicationDebtSettlement> _ApplicationDebtSettlements;

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationExpense_DAO.ApplicationDebtSettlements
        /// </summary>
        public IEventList<IApplicationDebtSettlement> ApplicationDebtSettlements
        {
            get
            {
                if (null == _ApplicationDebtSettlements)
                {
                    if (null == _DAO.ApplicationDebtSettlements)
                        _DAO.ApplicationDebtSettlements = new List<ApplicationDebtSettlement_DAO>();
                    _ApplicationDebtSettlements = new DAOEventList<ApplicationDebtSettlement_DAO, IApplicationDebtSettlement, ApplicationDebtSettlement>(_DAO.ApplicationDebtSettlements);
                    _ApplicationDebtSettlements.BeforeAdd += new EventListHandler(OnApplicationDebtSettlements_BeforeAdd);
                    _ApplicationDebtSettlements.BeforeRemove += new EventListHandler(OnApplicationDebtSettlements_BeforeRemove);
                    _ApplicationDebtSettlements.AfterAdd += new EventListHandler(OnApplicationDebtSettlements_AfterAdd);
                    _ApplicationDebtSettlements.AfterRemove += new EventListHandler(OnApplicationDebtSettlements_AfterRemove);
                }
                return _ApplicationDebtSettlements;
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            _ApplicationDebtSettlements = null;
        }
    }
}