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
	/// SAHL.Common.BusinessModel.DAO.InterestOnly_DAO
	/// </summary>
	public partial class InterestOnly : BusinessModelBase<SAHL.Common.BusinessModel.DAO.InterestOnly_DAO>, IInterestOnly
	{
		public InterestOnly(SAHL.Common.BusinessModel.DAO.InterestOnly_DAO InterestOnly)
			: base(InterestOnly)
		{
			this._DAO = InterestOnly;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO.Key
		/// </summary>
		public Int32 Key
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value; }
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InterestOnly_DAO.EntryDate
		/// </summary>
		public DateTime EntryDate 
		{
			get { return _DAO.EntryDate; }
			set { _DAO.EntryDate = value; }
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InterestOnly_DAO.MaturityDate
		/// </summary>
		public DateTime MaturityDate 
		{
			get { return _DAO.MaturityDate; }
			set { _DAO.MaturityDate = value; }
		}
	
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.InterestOnly_DAO.FinancialServiceAttribute
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
				if(value == null)
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


