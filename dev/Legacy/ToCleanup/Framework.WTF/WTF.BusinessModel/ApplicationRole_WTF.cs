
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;

using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO
	/// </summary>
    public partial class ApplicationRole_WTF : BusinessModelBase<ApplicationRole_WTF_DAO>, IApplicationRole_WTF
	{
        public ApplicationRole_WTF(ApplicationRole_WTF_DAO ApplicationRole_WTF) : base(ApplicationRole_WTF)
		{
            this._DAO = ApplicationRole_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.ApplicationRoleTypeKey
		/// </summary>
		public Int32 ApplicationRoleTypeKey 
		{
			get { return _DAO.ApplicationRoleTypeKey; }
			set { _DAO.ApplicationRoleTypeKey = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.StatusChangeDate
		/// </summary>
		public DateTime? StatusChangeDate 
		{
			get { return _DAO.StatusChangeDate; }
			set { _DAO.StatusChangeDate = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.GeneralStatus
		/// </summary>
        public IGeneralStatus_WTF GeneralStatus 
		{
			get
			{
				if (null == _DAO.GeneralStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGeneralStatus_WTF, GeneralStatus_WTF_DAO>(_DAO.GeneralStatus);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.GeneralStatus = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
                    _DAO.GeneralStatus = (GeneralStatus_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.LegalEntity
		/// </summary>
        public Int32 LegalEntityKey
        {
            get { return _DAO.LegalEntityKey; }
            set { _DAO.LegalEntityKey = value; }
        }
        /// <summary>
		/// SAHL.Common.BusinessModel.DAO.ApplicationRole_DAO.Application
		/// </summary>
        public Int32 ApplicationKey
        {
            get { return _DAO.ApplicationKey; }
            set { _DAO.ApplicationKey = value; }
        }
    }
}



