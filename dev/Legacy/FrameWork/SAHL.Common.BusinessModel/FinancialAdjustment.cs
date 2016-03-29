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

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO
    /// </summary>
    public partial class FinancialAdjustment : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO>, IFinancialAdjustment
    {
        public FinancialAdjustment(SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO FinancialAdjustment)
            : base(FinancialAdjustment)
        {
            this._DAO = FinancialAdjustment;
        }
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FromDate
        /// </summary>
        public DateTime? FromDate
        {
            get { return _DAO.FromDate; }
            set { _DAO.FromDate = value; }
        }
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.EndDate
        /// </summary>
        public DateTime? EndDate
        {
            get { return _DAO.EndDate; }
            set { _DAO.EndDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.CancellationDate
        /// </summary>
        public DateTime? CancellationDate
        {
            get { return _DAO.CancellationDate; }
            set { _DAO.CancellationDate = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FinancialService
        /// </summary>
        public IFinancialService FinancialService
        {
            get
            {
                if (null == _DAO.FinancialService) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IFinancialService, FinancialService_DAO>(_DAO.FinancialService);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.FinancialService = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.FinancialService = (FinancialService_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FinancialAdjustmentSource
        /// </summary>
        public IFinancialAdjustmentSource FinancialAdjustmentSource
        {
            get
            {
                if (null == _DAO.FinancialAdjustmentSource) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IFinancialAdjustmentSource, FinancialAdjustmentSource_DAO>(_DAO.FinancialAdjustmentSource);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.FinancialAdjustmentSource = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.FinancialAdjustmentSource = (FinancialAdjustmentSource_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FinancialAdjustmentStatus
        /// </summary>
        public IFinancialAdjustmentStatus FinancialAdjustmentStatus
        {
            get
            {
                if (null == _DAO.FinancialAdjustmentStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IFinancialAdjustmentStatus, FinancialAdjustmentStatus_DAO>(_DAO.FinancialAdjustmentStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.FinancialAdjustmentStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.FinancialAdjustmentStatus = (FinancialAdjustmentStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FinancialAdjustmentType
        /// </summary>
        public IFinancialAdjustmentType FinancialAdjustmentType
        {
            get
            {
                if (null == _DAO.FinancialAdjustmentType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IFinancialAdjustmentType, FinancialAdjustmentType_DAO>(_DAO.FinancialAdjustmentType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.FinancialAdjustmentType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.FinancialAdjustmentType = (FinancialAdjustmentType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.ReversalProvisionAdjustment
        /// </summary>
        public IReversalProvisionAdjustment ReversalProvisionAdjustment
        {
            get
            {
                if (null == _DAO.ReversalProvisionAdjustment) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IReversalProvisionAdjustment, ReversalProvisionAdjustment_DAO>(_DAO.ReversalProvisionAdjustment);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ReversalProvisionAdjustment = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ReversalProvisionAdjustment = (ReversalProvisionAdjustment_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.DifferentialProvisionAdjustment
        /// </summary>
        public IDifferentialProvisionAdjustment DifferentialProvisionAdjustment
        {
            get
            {
                if (null == _DAO.DifferentialProvisionAdjustment) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IDifferentialProvisionAdjustment, DifferentialProvisionAdjustment_DAO>(_DAO.DifferentialProvisionAdjustment);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.DifferentialProvisionAdjustment = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.DifferentialProvisionAdjustment = (DifferentialProvisionAdjustment_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.PaymentAdjustment
        /// </summary>
        public IPaymentAdjustment PaymentAdjustment
        {
            get
            {
                if (null == _DAO.PaymentAdjustment) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IPaymentAdjustment, PaymentAdjustment_DAO>(_DAO.PaymentAdjustment);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.PaymentAdjustment = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.PaymentAdjustment = (PaymentAdjustment_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.InterestRateAdjustment
        /// </summary>
        public IInterestRateAdjustment InterestRateAdjustment
        {
            get
            {
                if (null == _DAO.InterestRateAdjustment) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IInterestRateAdjustment, InterestRateAdjustment_DAO>(_DAO.InterestRateAdjustment);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.InterestRateAdjustment = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.InterestRateAdjustment = (InterestRateAdjustment_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.InterestRateAdjustment
		/// </summary>
		public IStaticRateAdjustment StaticRateAdjustment
		{
			get
			{
				if (null == _DAO.StaticRateAdjustment) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IStaticRateAdjustment, StaticRateAdjustment_DAO>(_DAO.StaticRateAdjustment);
				}
			}

			set
			{
				if (value == null)
				{
					_DAO.InterestRateAdjustment = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.InterestRateAdjustment = (InterestRateAdjustment_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FixedRateAdjustment
        /// </summary>
        public IFixedRateAdjustment FixedRateAdjustment
        {
            get
            {
                if (null == _DAO.FixedRateAdjustment) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IFixedRateAdjustment, FixedRateAdjustment_DAO>(_DAO.FixedRateAdjustment);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.FixedRateAdjustment = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.FixedRateAdjustment = (FixedRateAdjustment_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CancellationReason_DAO.CancellationReason
        /// </summary>
        public ICancellationReason CancellationReason
        {
            get
            {
                if (null == _DAO.CancellationReason) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<ICancellationReason, CancellationReason_DAO>(_DAO.CancellationReason);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CancellationReason = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CancellationReason = (CancellationReason_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.FinancialAdjustment_DAO.FinancialServiceAttribute
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


