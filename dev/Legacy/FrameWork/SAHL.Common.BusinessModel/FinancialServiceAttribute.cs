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
	/// SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO
	/// </summary>
	public partial class FinancialServiceAttribute : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO>, IFinancialServiceAttribute
	{
				public FinancialServiceAttribute(SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO FinancialServiceAttribute) : base(FinancialServiceAttribute)
		{
			this._DAO = FinancialServiceAttribute;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO.GeneralStatus
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
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO.FinancialService
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
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO.FinancialServiceAttributeType
		/// </summary>
		public IFinancialServiceAttributeType FinancialServiceAttributeType 
		{
			get
			{
				if (null == _DAO.FinancialServiceAttributeType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IFinancialServiceAttributeType, FinancialServiceAttributeType_DAO>(_DAO.FinancialServiceAttributeType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.FinancialServiceAttributeType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.FinancialServiceAttributeType = (FinancialServiceAttributeType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO.FinancialAdjustments
		/// </summary>
		private DAOEventList<FinancialAdjustment_DAO, IFinancialAdjustment, FinancialAdjustment> _FinancialAdjustments;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.FinancialServiceAttribute_DAO.FinancialAdjustments
		/// </summary>
		public IEventList<IFinancialAdjustment> FinancialAdjustments
		{
			get
			{
				if (null == _FinancialAdjustments) 
				{
					if(null == _DAO.FinancialAdjustments)
						_DAO.FinancialAdjustments = new List<FinancialAdjustment_DAO>();
					_FinancialAdjustments = new DAOEventList<FinancialAdjustment_DAO, IFinancialAdjustment, FinancialAdjustment>(_DAO.FinancialAdjustments);
					_FinancialAdjustments.BeforeAdd += new EventListHandler(OnFinancialAdjustments_BeforeAdd);					
					_FinancialAdjustments.BeforeRemove += new EventListHandler(OnFinancialAdjustments_BeforeRemove);					
					_FinancialAdjustments.AfterAdd += new EventListHandler(OnFinancialAdjustments_AfterAdd);					
					_FinancialAdjustments.AfterRemove += new EventListHandler(OnFinancialAdjustments_AfterRemove);					
				}
				return _FinancialAdjustments;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_FinancialAdjustments = null;
			
		}
	}
}


