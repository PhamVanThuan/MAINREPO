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
	/// SAHL.Common.BusinessModel.DAO.ApplicationInformationRateOverride_DAO
	/// </summary>
	public partial class ApplicationInformationRateOverride : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInformationRateOverride_DAO>, IApplicationInformationRateOverride
	{
				public ApplicationInformationRateOverride(SAHL.Common.BusinessModel.DAO.ApplicationInformationRateOverride_DAO ApplicationInformationRateOverride) : base(ApplicationInformationRateOverride)
		{
			this._DAO = ApplicationInformationRateOverride;
		}
		/// <summary>
		/// The Term applicable to the Rate Override e.g. A CAP Rate Override has a term of 24 months.
		/// </summary>
		public Int32? Term
		{
			get { return _DAO.Term; }
			set { _DAO.Term = value;}
		}
		/// <summary>
		/// The rate at which the client has decided to CAP their Mortgage Loan. The CAP is invoked when the client's rate exceeds 
		/// this rate.
		/// </summary>
		public Double? CapRate
		{
			get { return _DAO.CapRate; }
			set { _DAO.CapRate = value;}
		}
		/// <summary>
		/// The Outstanding Loan Balance when the client accepted the CAP Application. The CAP Rate applies to this balance.
		/// </summary>
		public Double? CapBalance
		{
			get { return _DAO.CapBalance; }
			set { _DAO.CapBalance = value;}
		}
		/// <summary>
		/// No rate override currently uses this property.
		/// </summary>
		public Double? FloorRate
		{
			get { return _DAO.FloorRate; }
			set { _DAO.FloorRate = value;}
		}
		/// <summary>
		/// No rate override currently uses this property.
		/// </summary>
		public Double? FixedRate
		{
			get { return _DAO.FixedRate; }
			set { _DAO.FixedRate = value;}
		}
		/// <summary>
		/// Certain Rate Overrides, such as Super Lo, require a discount to be given to the client. This is the value of the
		/// discount.
		/// </summary>
		public Double? Discount
		{
			get { return _DAO.Discount; }
			set { _DAO.Discount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationRateOverride_DAO.FromDate
		/// </summary>
		public DateTime? FromDate
		{
			get { return _DAO.FromDate; }
			set { _DAO.FromDate = value;}
		}
		/// <summary>
		/// Primary Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// The ApplicationInformationRateOverride record belongs to an ApplicationInformation record.
		/// </summary>
		public IApplicationInformation ApplicationInformation 
		{
			get
			{
				if (null == _DAO.ApplicationInformation) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IApplicationInformation, ApplicationInformation_DAO>(_DAO.ApplicationInformation);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ApplicationInformation = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ApplicationInformation = (ApplicationInformation_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationRateOverride_DAO.FinancialAdjustmentTypeSource
		/// </summary>
		public IFinancialAdjustmentTypeSource FinancialAdjustmentTypeSource 
		{
			get
			{
				if (null == _DAO.FinancialAdjustmentTypeSource) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialAdjustmentTypeSource, FinancialAdjustmentTypeSource_DAO>(_DAO.FinancialAdjustmentTypeSource);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialAdjustmentTypeSource = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialAdjustmentTypeSource = (FinancialAdjustmentTypeSource_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationRateOverride_DAO.ApplicationInformationAppliedRateAdjustments
		/// </summary>
		private DAOEventList<ApplicationInformationAppliedRateAdjustment_DAO, IApplicationInformationAppliedRateAdjustment, ApplicationInformationAppliedRateAdjustment> _ApplicationInformationAppliedRateAdjustments;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationInformationRateOverride_DAO.ApplicationInformationAppliedRateAdjustments
		/// </summary>
		public IEventList<IApplicationInformationAppliedRateAdjustment> ApplicationInformationAppliedRateAdjustments
		{
			get
			{
				if (null == _ApplicationInformationAppliedRateAdjustments) 
				{
					if(null == _DAO.ApplicationInformationAppliedRateAdjustments)
						_DAO.ApplicationInformationAppliedRateAdjustments = new List<ApplicationInformationAppliedRateAdjustment_DAO>();
					_ApplicationInformationAppliedRateAdjustments = new DAOEventList<ApplicationInformationAppliedRateAdjustment_DAO, IApplicationInformationAppliedRateAdjustment, ApplicationInformationAppliedRateAdjustment>(_DAO.ApplicationInformationAppliedRateAdjustments);
					_ApplicationInformationAppliedRateAdjustments.BeforeAdd += new EventListHandler(OnApplicationInformationAppliedRateAdjustments_BeforeAdd);					
					_ApplicationInformationAppliedRateAdjustments.BeforeRemove += new EventListHandler(OnApplicationInformationAppliedRateAdjustments_BeforeRemove);					
					_ApplicationInformationAppliedRateAdjustments.AfterAdd += new EventListHandler(OnApplicationInformationAppliedRateAdjustments_AfterAdd);					
					_ApplicationInformationAppliedRateAdjustments.AfterRemove += new EventListHandler(OnApplicationInformationAppliedRateAdjustments_AfterRemove);					
				}
				return _ApplicationInformationAppliedRateAdjustments;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_ApplicationInformationAppliedRateAdjustments = null;
			
		}
	}
}


