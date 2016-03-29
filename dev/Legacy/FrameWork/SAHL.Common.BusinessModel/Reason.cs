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
	/// Reason is a link between a ReasonDefinition and an generic object in the system. It is used to supply a reason for an action that is performed on that object.
	/// </summary>
	public partial class Reason : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Reason_DAO>, IReason
	{
				public Reason(SAHL.Common.BusinessModel.DAO.Reason_DAO Reason) : base(Reason)
		{
			this._DAO = Reason;
		}
		/// <summary>
		/// The Primary Key value of the GenericKey which the Reason is attached to.
		/// </summary>
		public Int32 GenericKey 
		{
			get { return _DAO.GenericKey; }
			set { _DAO.GenericKey = value;}
		}
		/// <summary>
		/// A comment for the reason.
		/// </summary>
		public String Comment 
		{
			get { return _DAO.Comment; }
			set { _DAO.Comment = value;}
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
		/// This is the foreign key reference to the ReasonDefinition table. Each Reason has a Definition which is stored in 
		/// the ReasonDefinition table.
		/// </summary>
		public IReasonDefinition ReasonDefinition 
		{
			get
			{
				if (null == _DAO.ReasonDefinition) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IReasonDefinition, ReasonDefinition_DAO>(_DAO.ReasonDefinition);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ReasonDefinition = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ReasonDefinition = (ReasonDefinition_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.Reason_DAO.StageTransition
		/// </summary>
		public IStageTransition StageTransition 
		{
			get
			{
				if (null == _DAO.StageTransition) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IStageTransition, StageTransition_DAO>(_DAO.StageTransition);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.StageTransition = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.StageTransition = (StageTransition_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


