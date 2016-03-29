using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO
    /// </summary>
    public partial class ApplicationDebitOrder : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO>, IApplicationDebitOrder
    {
        public ApplicationDebitOrder(SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO ApplicationDebitOrder)
            : base(ApplicationDebitOrder)
        {
            this._DAO = ApplicationDebitOrder;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO.Percentage
        /// </summary>
        public Double Percentage
        {
            get { return _DAO.Percentage; }
            set { _DAO.Percentage = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO.DebitOrderDay
        /// </summary>
        public Int32 DebitOrderDay
        {
            get { return _DAO.DebitOrderDay; }
            set { _DAO.DebitOrderDay = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO.BankAccount
        /// </summary>
        public IBankAccount BankAccount
        {
            get
            {
                if (null == _DAO.BankAccount) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBankAccount, BankAccount_DAO>(_DAO.BankAccount);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.BankAccount = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.BankAccount = (BankAccount_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO.Application
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
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebitOrder_DAO.FinancialServicePaymentType
        /// </summary>
        public IFinancialServicePaymentType FinancialServicePaymentType
        {
            get
            {
                if (null == _DAO.FinancialServicePaymentType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IFinancialServicePaymentType, FinancialServicePaymentType_DAO>(_DAO.FinancialServicePaymentType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.FinancialServicePaymentType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.FinancialServicePaymentType = (FinancialServicePaymentType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}