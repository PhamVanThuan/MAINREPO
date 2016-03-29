
using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO
	/// </summary>
    public partial class UserOrganisationStructure_WTF : BusinessModelBase<UserOrganisationStructure_WTF_DAO>, IUserOrganisationStructure
	{
        public UserOrganisationStructure_WTF(UserOrganisationStructure_WTF_DAO UserOrganisationStructure_WTF) : base(UserOrganisationStructure_WTF)
		{
            this._DAO = UserOrganisationStructure_WTF;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.ADUser
		/// </summary>
		public IADUser ADUser 
		{
			get
			{
				if (null == _DAO.ADUser) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IADUser, ADUser_WTF_DAO>(_DAO.ADUser);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.ADUser = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
                    _DAO.ADUser = (ADUser_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructure_DAO.OrganisationStructure
		/// </summary>
		public IOrganisationStructure OrganisationStructure 
		{
			get
			{
				if (null == _DAO.OrganisationStructure) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IOrganisationStructure, OrganisationStructure_WTF_DAO>(_DAO.OrganisationStructure);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.OrganisationStructure = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
                    _DAO.OrganisationStructure = (OrganisationStructure_WTF_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}


        public IEventList<IUserOrganisationStructureRoundRobinStatus> UserOrganisationStructureRoundRobinStatus
        {
            get { throw new NotImplementedException(); }
        }
    }
}



