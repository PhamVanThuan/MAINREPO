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
	/// SAHL.Common.BusinessModel.DAO.Fee_DAO
	/// </summary>
	public partial class Fee : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Fee_DAO>, IFee
	{
				public Fee(SAHL.Common.BusinessModel.DAO.Fee_DAO Fee) : base(Fee)
		{
			this._DAO = Fee;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fee_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fee_DAO.Amount
		/// </summary>
		public Double Amount 
		{
			get { return _DAO.Amount; }
			set { _DAO.Amount = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fee_DAO.LastPostedDate
		/// </summary>
		public DateTime? LastPostedDate
		{
			get { return _DAO.LastPostedDate; }
			set { _DAO.LastPostedDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fee_DAO.FinancialService
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
				if(value == null)
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
		/// SAHL.Common.BusinessModel.DAO.Fee_DAO.GeneralStatus
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
				if(value == null)
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
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Fee_DAO.FeeType
		/// </summary>
		public IFeeType FeeType 
		{
			get
			{
				if (null == _DAO.FeeType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFeeType, FeeType_DAO>(_DAO.FeeType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FeeType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FeeType = (FeeType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


