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
	/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureRoundRobinStatus_DAO
	/// </summary>
	public partial class UserOrganisationStructureRoundRobinStatus : BusinessModelBase<SAHL.Common.BusinessModel.DAO.UserOrganisationStructureRoundRobinStatus_DAO>, IUserOrganisationStructureRoundRobinStatus
	{
				public UserOrganisationStructureRoundRobinStatus(SAHL.Common.BusinessModel.DAO.UserOrganisationStructureRoundRobinStatus_DAO UserOrganisationStructureRoundRobinStatus) : base(UserOrganisationStructureRoundRobinStatus)
		{
			this._DAO = UserOrganisationStructureRoundRobinStatus;
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureRoundRobinStatus_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureRoundRobinStatus_DAO.GeneralStatus
		/// </summary>
		public IGeneralStatus GeneralStatus 
		{
			get
			{
				if (null == _DAO.GeneralStatus) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.GeneralStatus);
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
					_DAO.GeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureRoundRobinStatus_DAO.CapitecGeneralStatus
        /// </summary>
        public IGeneralStatus CapitecGeneralStatus
        {
            get
            {
                if (null == _DAO.CapitecGeneralStatus) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IGeneralStatus, GeneralStatus_DAO>(_DAO.CapitecGeneralStatus);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.CapitecGeneralStatus = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.CapitecGeneralStatus = (GeneralStatus_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
		/// <summary>
		/// SAHL.Common.BusinessModel.DAO.UserOrganisationStructureRoundRobinStatus_DAO.UserOrganisationStructure
		/// </summary>
		public IUserOrganisationStructure UserOrganisationStructure 
		{
			get
			{
				if (null == _DAO.UserOrganisationStructure) return null;
				else
				{
					IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
					return BMTM.GetMappedType<IUserOrganisationStructure, UserOrganisationStructure_DAO>(_DAO.UserOrganisationStructure);
					}
			}

			set
			{
				if(value == null)
				{
					_DAO.UserOrganisationStructure = null;
					return;
				}
				IDAOObject obj = value as IDAOObject;

				if (obj != null)
					_DAO.UserOrganisationStructure = (UserOrganisationStructure_DAO)obj.GetDAOObject();
				else
					throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
			}
		}
	}
}


