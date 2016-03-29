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
	/// ReasonDefinition_DAO links the Reason Description and the Reason Type.
	/// </summary>
	public partial class ReasonDefinition : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ReasonDefinition_DAO>, IReasonDefinition
	{
				public ReasonDefinition(SAHL.Common.BusinessModel.DAO.ReasonDefinition_DAO ReasonDefinition) : base(ReasonDefinition)
		{
			this._DAO = ReasonDefinition;
		}
		/// <summary>
		/// An indicator as to whether a comment can be stored against the reason.
		/// </summary>
		public Boolean AllowComment 
		{
			get { return _DAO.AllowComment; }
			set { _DAO.AllowComment = value;}
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
		/// SAHL.Common.BusinessModel.DAO.ReasonDefinition_DAO.EnforceComment
		/// </summary>
		public Boolean EnforceComment 
		{
			get { return _DAO.EnforceComment; }
			set { _DAO.EnforceComment = value;}
		}
		/// <summary>
		/// a list of OriginationSourceProducts this reasondefinition is applicable to.
		/// </summary>
		private DAOEventList<OriginationSourceProduct_DAO, IOriginationSourceProduct, OriginationSourceProduct> _OriginationSourceProducts;
		/// <summary>
		/// a list of OriginationSourceProducts this reasondefinition is applicable to.
		/// </summary>
		public IEventList<IOriginationSourceProduct> OriginationSourceProducts
		{
			get
			{
				if (null == _OriginationSourceProducts) 
				{
					if(null == _DAO.OriginationSourceProducts)
						_DAO.OriginationSourceProducts = new List<OriginationSourceProduct_DAO>();
					_OriginationSourceProducts = new DAOEventList<OriginationSourceProduct_DAO, IOriginationSourceProduct, OriginationSourceProduct>(_DAO.OriginationSourceProducts);
					_OriginationSourceProducts.BeforeAdd += new EventListHandler(OnOriginationSourceProducts_BeforeAdd);					
					_OriginationSourceProducts.BeforeRemove += new EventListHandler(OnOriginationSourceProducts_BeforeRemove);					
					_OriginationSourceProducts.AfterAdd += new EventListHandler(OnOriginationSourceProducts_AfterAdd);					
					_OriginationSourceProducts.AfterRemove += new EventListHandler(OnOriginationSourceProducts_AfterRemove);					
				}
				return _OriginationSourceProducts;
			}
		}
		/// <summary>
		/// Each Reason Definition belongs to a Reason Description, this is the foreign key reference to the ReasonDescription table.
		/// </summary>
		public IReasonDescription ReasonDescription 
		{
			get
			{
				if (null == _DAO.ReasonDescription) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IReasonDescription, ReasonDescription_DAO>(_DAO.ReasonDescription);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ReasonDescription = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ReasonDescription = (ReasonDescription_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// Each Reason Definition belongs to a Reason Type, this is the foreign key reference to the ReasonType table.
		/// </summary>
		public IReasonType ReasonType 
		{
			get
			{
				if (null == _DAO.ReasonType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IReasonType, ReasonType_DAO>(_DAO.ReasonType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ReasonType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ReasonType = (ReasonType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// The foreign key reference to the GeneralStatus table.
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
		public override void Refresh()
		{
			base.Refresh();
			_OriginationSourceProducts = null;
			
		}
	}
}


