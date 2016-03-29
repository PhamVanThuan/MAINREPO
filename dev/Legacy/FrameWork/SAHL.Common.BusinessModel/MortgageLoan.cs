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
	/// SAHL.Common.BusinessModel.DAO.MortgageLoan_DAO
	/// </summary>
	public partial class MortgageLoan : FinancialService, IMortgageLoan
	{
		protected new SAHL.Common.BusinessModel.DAO.MortgageLoan_DAO _DAO;
		public MortgageLoan(SAHL.Common.BusinessModel.DAO.MortgageLoan_DAO MortgageLoan) : base(MortgageLoan)
		{
			this._DAO = MortgageLoan;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoan_DAO.CreditMatrix
		/// </summary>
		public ICreditMatrix CreditMatrix 
		{
			get
			{
				if (null == _DAO.CreditMatrix) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ICreditMatrix, CreditMatrix_DAO>(_DAO.CreditMatrix);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.CreditMatrix = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.CreditMatrix = (CreditMatrix_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoan_DAO.Property
		/// </summary>
		public IProperty Property 
		{
			get
			{
				if (null == _DAO.Property) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IProperty, Property_DAO>(_DAO.Property);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.Property = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.Property = (Property_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoan_DAO.MortgageLoanPurpose
		/// </summary>
		public IMortgageLoanPurpose MortgageLoanPurpose 
		{
			get
			{
				if (null == _DAO.MortgageLoanPurpose) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMortgageLoanPurpose, MortgageLoanPurpose_DAO>(_DAO.MortgageLoanPurpose);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.MortgageLoanPurpose = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.MortgageLoanPurpose = (MortgageLoanPurpose_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoan_DAO.Bonds
		/// </summary>
		private DAOEventList<Bond_DAO, IBond, Bond> _Bonds;
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoan_DAO.Bonds
		/// </summary>
		public IEventList<IBond> Bonds
		{
			get
			{
				if (null == _Bonds) 
				{
					if(null == _DAO.Bonds)
						_DAO.Bonds = new List<Bond_DAO>();
					_Bonds = new DAOEventList<Bond_DAO, IBond, Bond>(_DAO.Bonds);
					_Bonds.BeforeAdd += new EventListHandler(OnBonds_BeforeAdd);					
					_Bonds.BeforeRemove += new EventListHandler(OnBonds_BeforeRemove);					
					_Bonds.AfterAdd += new EventListHandler(OnBonds_AfterAdd);					
					_Bonds.AfterRemove += new EventListHandler(OnBonds_AfterRemove);					
				}
				return _Bonds;
			}
		}
		public override void Refresh()
		{
			base.Refresh();
			_Bonds = null;
			
		}
	}
}


