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
	/// SAHL.Common.BusinessModel.DAO.XMLHistory_DAO
	/// </summary>
	public partial class XMLHistory : BusinessModelBase<SAHL.Common.BusinessModel.DAO.XMLHistory_DAO>, IXMLHistory
	{
				public XMLHistory(SAHL.Common.BusinessModel.DAO.XMLHistory_DAO XMLHistory) : base(XMLHistory)
		{
			this._DAO = XMLHistory;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.XMLHistory_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.XMLHistory_DAO.GenericKeyType
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
		/// SAHL.Common.BusinessModel.DAO.XMLHistory_DAO.GenericKey
		/// </summary>
		public Int32 GenericKey 
		{
			get { return _DAO.GenericKey; }
			set { _DAO.GenericKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.XMLHistory_DAO.XmlData
		/// </summary>
		public String XmlData 
		{
			get { return _DAO.XmlData; }
			set { _DAO.XmlData = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.XMLHistory_DAO.InsertDate
		/// </summary>
		public DateTime InsertDate 
		{
			get { return _DAO.InsertDate; }
			set { _DAO.InsertDate = value;}
		}
	}
}


