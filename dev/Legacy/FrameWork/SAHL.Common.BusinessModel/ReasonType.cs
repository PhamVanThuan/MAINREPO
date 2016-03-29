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
	/// ReasonType_DAO is used to assign each reason a type. It is also specifies the Generic Key type which applies to the Reason.
	/// </summary>
	public partial class ReasonType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ReasonType_DAO>, IReasonType
	{
				public ReasonType(SAHL.Common.BusinessModel.DAO.ReasonType_DAO ReasonType) : base(ReasonType)
		{
			this._DAO = ReasonType;
		}
		/// <summary>
		/// The Description of the Reason Type.
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
		}
		/// <summary>
		/// Each Reason Type (and the Reasons associated to the Reason Type) belong to a specific GenericKeyType. For example
		/// a Reason Type of 'Credit Decline Loan' would be linked to an OfferInformationKey via this GenericKeyType link.
		/// </summary>
		public IGenericKeyType GenericKeyType 
		{
			get
			{
				if (null == _DAO.GenericKeyType) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGenericKeyType, GenericKeyType_DAO>(_DAO.GenericKeyType);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GenericKeyType = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GenericKeyType = (GenericKeyType_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
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
		/// Each of the ReasonTypes belong to a ReasonTypeGroup. This is the foreign key reference to the ReasonTypeGroup table.
		/// </summary>
		public IReasonTypeGroup ReasonTypeGroup 
		{
			get
			{
				if (null == _DAO.ReasonTypeGroup) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IReasonTypeGroup, ReasonTypeGroup_DAO>(_DAO.ReasonTypeGroup);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ReasonTypeGroup = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.ReasonTypeGroup = (ReasonTypeGroup_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


