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
	/// SAHL.Common.BusinessModel.DAO.Operator_DAO
	/// </summary>
	public partial class Operator : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Operator_DAO>, IOperator
	{
				public Operator(SAHL.Common.BusinessModel.DAO.Operator_DAO Operator) : base(Operator)
		{
			this._DAO = Operator;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Operator_DAO.Description
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Operator_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Operator_DAO.OperatorGroup
		/// </summary>
		public IOperatorGroup OperatorGroup 
		{
			get
			{
				if (null == _DAO.OperatorGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IOperatorGroup, OperatorGroup_DAO>(_DAO.OperatorGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OperatorGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.OperatorGroup = (OperatorGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


