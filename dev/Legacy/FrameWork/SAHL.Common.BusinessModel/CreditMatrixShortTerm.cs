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
	/// SAHL.Common.BusinessModel.DAO.CreditMatrixShortTerm_DAO
	/// </summary>
	public partial class CreditMatrixShortTerm : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CreditMatrixShortTerm_DAO>, ICreditMatrixShortTerm
	{
				public CreditMatrixShortTerm(SAHL.Common.BusinessModel.DAO.CreditMatrixShortTerm_DAO CreditMatrixShortTerm) : base(CreditMatrixShortTerm)
		{
			this._DAO = CreditMatrixShortTerm;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixShortTerm_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixShortTerm_DAO.NewBusinessIndicator
		/// </summary>
		public char NewBusinessIndicator 
		{
			get { return _DAO.NewBusinessIndicator; }
			set { _DAO.NewBusinessIndicator = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixShortTerm_DAO.ImplementationDate
		/// </summary>
		public DateTime? ImplementationDate 
		{
			get { return _DAO.ImplementationDate; }
			set { _DAO.ImplementationDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixShortTerm_DAO.CreditCriteriaShortTerms
		/// </summary>
		private DAOEventList<CreditCriteriaShortTerm_DAO, ICreditCriteriaShortTerm, CreditCriteriaShortTerm> _CreditCriteriaShortTerms;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixShortTerm_DAO.CreditCriteriaShortTerms
		/// </summary>
		public IEventList<ICreditCriteriaShortTerm> CreditCriteriaShortTerms
		{
			get
			{
				if (null == _CreditCriteriaShortTerms) 
				{
					if(null == _DAO.CreditCriteriaShortTerms)
						_DAO.CreditCriteriaShortTerms = new List<CreditCriteriaShortTerm_DAO>();
					_CreditCriteriaShortTerms = new DAOEventList<CreditCriteriaShortTerm_DAO, ICreditCriteriaShortTerm, CreditCriteriaShortTerm>(_DAO.CreditCriteriaShortTerms);
					_CreditCriteriaShortTerms.BeforeAdd += new EventListHandler(OnCreditCriteriaShortTerms_BeforeAdd);					
					_CreditCriteriaShortTerms.BeforeRemove += new EventListHandler(OnCreditCriteriaShortTerms_BeforeRemove);					
					_CreditCriteriaShortTerms.AfterAdd += new EventListHandler(OnCreditCriteriaShortTerms_AfterAdd);					
					_CreditCriteriaShortTerms.AfterRemove += new EventListHandler(OnCreditCriteriaShortTerms_AfterRemove);					
				}
				return _CreditCriteriaShortTerms;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_CreditCriteriaShortTerms = null;
			
		}
	}
}


