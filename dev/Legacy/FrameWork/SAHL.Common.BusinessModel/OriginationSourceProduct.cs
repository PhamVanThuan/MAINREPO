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
	/// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO
	/// </summary>
	public partial class OriginationSourceProduct : BusinessModelBase<SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO>, IOriginationSourceProduct
	{
				public OriginationSourceProduct(SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO OriginationSourceProduct) : base(OriginationSourceProduct)
		{
			this._DAO = OriginationSourceProduct;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO.OriginationSource
		/// </summary>
		public IOriginationSource OriginationSource 
		{
			get
			{
				if (null == _DAO.OriginationSource) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOriginationSource, OriginationSource_DAO>(_DAO.OriginationSource);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OriginationSource = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OriginationSource = (OriginationSource_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO.Product
		/// </summary>
		public IProduct Product 
		{
			get
			{
				if (null == _DAO.Product) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IProduct, Product_DAO>(_DAO.Product);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Product = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Product = (Product_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO.CreditMatrices
		/// </summary>
		private DAOEventList<CreditMatrix_DAO, ICreditMatrix, CreditMatrix> _CreditMatrices;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO.CreditMatrices
		/// </summary>
		public IEventList<ICreditMatrix> CreditMatrices
		{
			get
			{
				if (null == _CreditMatrices) 
				{
					if(null == _DAO.CreditMatrices)
						_DAO.CreditMatrices = new List<CreditMatrix_DAO>();
					_CreditMatrices = new DAOEventList<CreditMatrix_DAO, ICreditMatrix, CreditMatrix>(_DAO.CreditMatrices);
					_CreditMatrices.BeforeAdd += new EventListHandler(OnCreditMatrices_BeforeAdd);					
					_CreditMatrices.BeforeRemove += new EventListHandler(OnCreditMatrices_BeforeRemove);					
					_CreditMatrices.AfterAdd += new EventListHandler(OnCreditMatrices_AfterAdd);					
					_CreditMatrices.AfterRemove += new EventListHandler(OnCreditMatrices_AfterRemove);					
				}
				return _CreditMatrices;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO.Priorities
		/// </summary>
		private DAOEventList<Priority_DAO, IPriority, Priority> _Priorities;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.OriginationSourceProduct_DAO.Priorities
		/// </summary>
		public IEventList<IPriority> Priorities
		{
			get
			{
				if (null == _Priorities) 
				{
					if(null == _DAO.Priorities)
						_DAO.Priorities = new List<Priority_DAO>();
					_Priorities = new DAOEventList<Priority_DAO, IPriority, Priority>(_DAO.Priorities);
					_Priorities.BeforeAdd += new EventListHandler(OnPriorities_BeforeAdd);					
					_Priorities.BeforeRemove += new EventListHandler(OnPriorities_BeforeRemove);					
					_Priorities.AfterAdd += new EventListHandler(OnPriorities_AfterAdd);					
					_Priorities.AfterRemove += new EventListHandler(OnPriorities_AfterRemove);					
				}
				return _Priorities;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_CreditMatrices = null;
			_Priorities = null;
			
		}
	}
}


