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
	/// ReasonDescription_DAO contains the text versions of the Reasons. It also links the Reason to a Translatable Item
		/// from which a translated version of the reason can be taken.
	/// </summary>
	public partial class ReasonDescription : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ReasonDescription_DAO>, IReasonDescription
	{
				public ReasonDescription(SAHL.Common.BusinessModel.DAO.ReasonDescription_DAO ReasonDescription) : base(ReasonDescription)
		{
			this._DAO = ReasonDescription;
		}
		/// <summary>
		/// The text description of the Reason
		/// </summary>
		public String Description 
		{
			get { return _DAO.Description; }
			set { _DAO.Description = value;}
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
		/// Each Reason belongs to a Translatable Item in the TranslatableItem table. This is the foreign key reference to the
		/// TranslatableItem table.
		/// </summary>
		public ITranslatableItem TranslatableItem 
		{
			get
			{
				if (null == _DAO.TranslatableItem) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<ITranslatableItem, TranslatableItem_DAO>(_DAO.TranslatableItem);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.TranslatableItem = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.TranslatableItem = (TranslatableItem_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


