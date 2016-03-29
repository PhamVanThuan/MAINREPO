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
	/// SAHL.Common.BusinessModel.DAO.HOCRates_DAO
	/// </summary>
	public partial class HOCRates : BusinessModelBase<SAHL.Common.BusinessModel.DAO.HOCRates_DAO>, IHOCRates
	{
				public HOCRates(SAHL.Common.BusinessModel.DAO.HOCRates_DAO HOCRates) : base(HOCRates)
		{
			this._DAO = HOCRates;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCRates_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCRates_DAO.HOCInsurer
		/// </summary>
		public IHOCInsurer HOCInsurer 
		{
			get
			{
				if (null == _DAO.HOCInsurer) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHOCInsurer, HOCInsurer_DAO>(_DAO.HOCInsurer);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HOCInsurer = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HOCInsurer = (HOCInsurer_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCRates_DAO.HOCSubsidence
		/// </summary>
		public IHOCSubsidence HOCSubsidence 
		{
			get
			{
				if (null == _DAO.HOCSubsidence) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IHOCSubsidence, HOCSubsidence_DAO>(_DAO.HOCSubsidence);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.HOCSubsidence = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.HOCSubsidence = (HOCSubsidence_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCRates_DAO.ThatchPremium
		/// </summary>
		public Double ThatchPremium 
		{
			get { return _DAO.ThatchPremium; }
			set { _DAO.ThatchPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCRates_DAO.ConventionalPremium
		/// </summary>
		public Double ConventionalPremium 
		{
			get { return _DAO.ConventionalPremium; }
			set { _DAO.ConventionalPremium = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.HOCRates_DAO.ShinglePremium
		/// </summary>
		public Double ShinglePremium 
		{
			get { return _DAO.ShinglePremium; }
			set { _DAO.ShinglePremium = value;}
		}
	}
}


