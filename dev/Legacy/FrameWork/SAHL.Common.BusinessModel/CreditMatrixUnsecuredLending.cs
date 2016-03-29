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
	/// SAHL.Common.BusinessModel.DAO.CreditMatrixUnsecuredLending_DAO
	/// </summary>
	public partial class CreditMatrixUnsecuredLending : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CreditMatrixUnsecuredLending_DAO>, ICreditMatrixUnsecuredLending
	{
				public CreditMatrixUnsecuredLending(SAHL.Common.BusinessModel.DAO.CreditMatrixUnsecuredLending_DAO CreditMatrixUnsecuredLending) : base(CreditMatrixUnsecuredLending)
		{
			this._DAO = CreditMatrixUnsecuredLending;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixUnsecuredLending_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixUnsecuredLending_DAO.NewBusinessIndicator
		/// </summary>
		public Char NewBusinessIndicator 
		{
			get { return _DAO.NewBusinessIndicator; }
			set { _DAO.NewBusinessIndicator = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixUnsecuredLending_DAO.ImplementationDate
		/// </summary>
		public DateTime? ImplementationDate
		{
			get { return _DAO.ImplementationDate; }
			set { _DAO.ImplementationDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixUnsecuredLending_DAO.CreditCriteriaUnsecuredLendings
		/// </summary>
		private DAOEventList<CreditCriteriaUnsecuredLending_DAO, ICreditCriteriaUnsecuredLending, CreditCriteriaUnsecuredLending> _CreditCriteriaUnsecuredLendings;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.CreditMatrixUnsecuredLending_DAO.CreditCriteriaUnsecuredLendings
		/// </summary>
		public IEventList<ICreditCriteriaUnsecuredLending> CreditCriteriaUnsecuredLendings
		{
			get
			{
				if (null == _CreditCriteriaUnsecuredLendings) 
				{
					if(null == _DAO.CreditCriteriaUnsecuredLendings)
						_DAO.CreditCriteriaUnsecuredLendings = new List<CreditCriteriaUnsecuredLending_DAO>();
					_CreditCriteriaUnsecuredLendings = new DAOEventList<CreditCriteriaUnsecuredLending_DAO, ICreditCriteriaUnsecuredLending, CreditCriteriaUnsecuredLending>(_DAO.CreditCriteriaUnsecuredLendings);
					_CreditCriteriaUnsecuredLendings.BeforeAdd += new EventListHandler(OnCreditCriteriaUnsecuredLendings_BeforeAdd);					
					_CreditCriteriaUnsecuredLendings.BeforeRemove += new EventListHandler(OnCreditCriteriaUnsecuredLendings_BeforeRemove);					
					_CreditCriteriaUnsecuredLendings.AfterAdd += new EventListHandler(OnCreditCriteriaUnsecuredLendings_AfterAdd);					
					_CreditCriteriaUnsecuredLendings.AfterRemove += new EventListHandler(OnCreditCriteriaUnsecuredLendings_AfterRemove);					
				}
				return _CreditCriteriaUnsecuredLendings;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_CreditCriteriaUnsecuredLendings = null;
			
		}
	}
}


