using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CAP_DAO
    /// </summary>
    public partial class CAP : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CAP_DAO>, ICAP
    {
        public CAP(SAHL.Common.BusinessModel.DAO.CAP_DAO CAP)
            : base(CAP)
        {
            this._DAO = CAP;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.TradeKey
        /// </summary>
        public Int32 TradeKey
        {
            get { return _DAO.TradeKey; }
            set { _DAO.TradeKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.CancellationDate
        /// </summary>
        public DateTime? CancellationDate
        {
            get { return _DAO.CancellationDate; }
            set { _DAO.CancellationDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.CancellationReasonKey
        /// </summary>
        public Int32? CancellationReasonKey
        {
            get { return _DAO.CancellationReasonKey; }
            set { _DAO.CancellationReasonKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.CAPBalance
        /// </summary>
        public Double CAPBalance
        {
            get { return _DAO.CAPBalance; }
            set { _DAO.CAPBalance = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.CAPPrePaymentAmount
        /// </summary>
        public Double CAPPrePaymentAmount
        {
            get { return _DAO.CAPPrePaymentAmount; }
            set { _DAO.CAPPrePaymentAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.MTDCAPPrePaymentAmount
        /// </summary>
        public Double MTDCAPPrePaymentAmount
        {
            get { return _DAO.MTDCAPPrePaymentAmount; }
            set { _DAO.MTDCAPPrePaymentAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.Invoked
        /// </summary>
        public Boolean Invoked
        {
            get { return _DAO.Invoked; }
            set { _DAO.Invoked = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.CapOfferDetail
        /// </summary>
        public ICapApplicationDetail CapOfferDetail
        {
            get
            {
                if (null == _DAO.CapOfferDetail) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICapApplicationDetail, CapApplicationDetail_DAO>(_DAO.CapOfferDetail);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CapOfferDetail = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CapOfferDetail = (CapApplicationDetail_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.CAPPaymentOption
        /// </summary>
        public ICapPaymentOption CAPPaymentOption
        {
            get
            {
                if (null == _DAO.CAPPaymentOption) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICapPaymentOption, CapPaymentOption_DAO>(_DAO.CAPPaymentOption);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CAPPaymentOption = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CAPPaymentOption = (CapPaymentOption_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CAP_DAO.FinancialServiceAttribute
        /// </summary>
        public IFinancialServiceAttribute FinancialServiceAttribute
        {
            get
            {
                if (null == _DAO.FinancialServiceAttribute) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IFinancialServiceAttribute, FinancialServiceAttribute_DAO>(_DAO.FinancialServiceAttribute);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.FinancialServiceAttribute = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.FinancialServiceAttribute = (FinancialServiceAttribute_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}