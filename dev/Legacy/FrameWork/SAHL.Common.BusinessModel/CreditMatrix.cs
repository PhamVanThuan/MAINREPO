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
	/// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO
	/// </summary>
	public partial class CreditMatrix : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO>, ICreditMatrix
	{
				public CreditMatrix(SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO CreditMatrix) : base(CreditMatrix)
		{
			this._DAO = CreditMatrix;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO.NewBusinessIndicator
		/// </summary>
		public Char NewBusinessIndicator 
		{
			get { return _DAO.NewBusinessIndicator; }
			set { _DAO.NewBusinessIndicator = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO.ImplementationDate
		/// </summary>
		public DateTime? ImplementationDate
		{
			get { return _DAO.ImplementationDate; }
			set { _DAO.ImplementationDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO.CreditCriterias
		/// </summary>
		private DAOEventList<CreditCriteria_DAO, ICreditCriteria, CreditCriteria> _CreditCriterias;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO.CreditCriterias
		/// </summary>
		public IEventList<ICreditCriteria> CreditCriterias
		{
			get
			{
				if (null == _CreditCriterias) 
				{
					if(null == _DAO.CreditCriterias)
						_DAO.CreditCriterias = new List<CreditCriteria_DAO>();
					_CreditCriterias = new DAOEventList<CreditCriteria_DAO, ICreditCriteria, CreditCriteria>(_DAO.CreditCriterias);
					_CreditCriterias.BeforeAdd += new EventListHandler(OnCreditCriterias_BeforeAdd);					
					_CreditCriterias.BeforeRemove += new EventListHandler(OnCreditCriterias_BeforeRemove);					
					_CreditCriterias.AfterAdd += new EventListHandler(OnCreditCriterias_AfterAdd);					
					_CreditCriterias.AfterRemove += new EventListHandler(OnCreditCriterias_AfterRemove);					
				}
				return _CreditCriterias;
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO.OriginationSourceProducts
		/// </summary>
		private DAOEventList<OriginationSourceProduct_DAO, IOriginationSourceProduct, OriginationSourceProduct> _OriginationSourceProducts;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrix_DAO.OriginationSourceProducts
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
		public override void Refresh()
		{
			base.Refresh();
			_CreditCriterias = null;
			_OriginationSourceProducts = null;
			
		}
	}
}


