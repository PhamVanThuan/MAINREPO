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
	/// SAHL.Common.BusinessModel.DAO.MortgageLoanPurpose_DAO
	/// </summary>
	public partial class MortgageLoanPurpose : BusinessModelBase<SAHL.Common.BusinessModel.DAO.MortgageLoanPurpose_DAO>, IMortgageLoanPurpose
	{
				public MortgageLoanPurpose(SAHL.Common.BusinessModel.DAO.MortgageLoanPurpose_DAO MortgageLoanPurpose) : base(MortgageLoanPurpose)
		{
			this._DAO = MortgageLoanPurpose;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanPurpose_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanPurpose_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.MortgageLoanPurpose_DAO.MortgageLoanPurposeGroup
		/// </summary>
		public IMortgageLoanPurposeGroup MortgageLoanPurposeGroup 
		{
			get
			{
				if (null == _DAO.MortgageLoanPurposeGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IMortgageLoanPurposeGroup, MortgageLoanPurposeGroup_DAO>(_DAO.MortgageLoanPurposeGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.MortgageLoanPurposeGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.MortgageLoanPurposeGroup = (MortgageLoanPurposeGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


