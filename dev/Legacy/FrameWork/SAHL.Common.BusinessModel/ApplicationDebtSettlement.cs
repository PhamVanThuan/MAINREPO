using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO
    /// </summary>
    public partial class ApplicationDebtSettlement : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO>, IApplicationDebtSettlement
    {
        public ApplicationDebtSettlement(SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO ApplicationDebtSettlement)
            : base(ApplicationDebtSettlement)
        {
            this._DAO = ApplicationDebtSettlement;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.SettlementAmount
        /// </summary>
        public Double SettlementAmount
        {
            get { return _DAO.SettlementAmount; }
            set { _DAO.SettlementAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.SettlementDate
        /// </summary>
        public DateTime? SettlementDate
        {
            get { return _DAO.SettlementDate; }
            set { _DAO.SettlementDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.RateApplied
        /// </summary>
        public Double RateApplied
        {
            get { return _DAO.RateApplied; }
            set { _DAO.RateApplied = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.InterestStartDate
        /// </summary>
        public DateTime? InterestStartDate
        {
            get { return _DAO.InterestStartDate; }
            set { _DAO.InterestStartDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.CapitalAmount
        /// </summary>
        public Double CapitalAmount
        {
            get { return _DAO.CapitalAmount; }
            set { _DAO.CapitalAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.GuaranteeAmount
        /// </summary>
        public Double GuaranteeAmount
        {
            get { return _DAO.GuaranteeAmount; }
            set { _DAO.GuaranteeAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.BankAccount
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
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.Disbursement
        /// </summary>
        public IDisbursement Disbursement
        {
            get
            {
                if (null == _DAO.Disbursement) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IDisbursement, Disbursement_DAO>(_DAO.Disbursement);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Disbursement = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Disbursement = (Disbursement_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.DisbursementInterestApplied
        /// </summary>
        public IDisbursementInterestApplied DisbursementInterestApplied
        {
            get
            {
                if (null == _DAO.DisbursementInterestApplied) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IDisbursementInterestApplied, DisbursementInterestApplied_DAO>(_DAO.DisbursementInterestApplied);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.DisbursementInterestApplied = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.DisbursementInterestApplied = (DisbursementInterestApplied_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.DisbursementType
        /// </summary>
        public IDisbursementType DisbursementType
        {
            get
            {
                if (null == _DAO.DisbursementType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IDisbursementType, DisbursementType_DAO>(_DAO.DisbursementType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.DisbursementType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.DisbursementType = (DisbursementType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationDebtSettlement_DAO.OfferExpense
        /// </summary>
        public IApplicationExpense OfferExpense
        {
            get
            {
                if (null == _DAO.OfferExpense) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationExpense, ApplicationExpense_DAO>(_DAO.OfferExpense);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.OfferExpense = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.OfferExpense = (ApplicationExpense_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}