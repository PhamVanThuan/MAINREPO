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
	/// SAHL.Common.BusinessModel.DAO.GenericSet_DAO
	/// </summary>
	public partial class GenericSet : BusinessModelBase<SAHL.Common.BusinessModel.DAO.GenericSet_DAO>, IGenericSet
	{
				public GenericSet(SAHL.Common.BusinessModel.DAO.GenericSet_DAO GenericSet) : base(GenericSet)
		{
			this._DAO = GenericSet;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericSet_DAO.GenericKey
		/// </summary>
		public Int32 GenericKey 
		{
			get { return _DAO.GenericKey; }
			set { _DAO.GenericKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericSet_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.GenericSet_DAO.GenericSetDefinition
		/// </summary>
		public IGenericSetDefinition GenericSetDefinition 
		{
			get
			{
				if (null == _DAO.GenericSetDefinition) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGenericSetDefinition, GenericSetDefinition_DAO>(_DAO.GenericSetDefinition);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GenericSetDefinition = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.GenericSetDefinition = (GenericSetDefinition_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


