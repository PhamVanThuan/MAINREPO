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
	/// SAHL.Common.BusinessModel.DAO.Provider_DAO
	/// </summary>
	public partial class Provider : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Provider_DAO>, IProvider
	{
        public Provider(SAHL.Common.BusinessModel.DAO.Provider_DAO Provider)
            : base(Provider)
		{
			this._DAO = Provider;
		}
		/// <summary>
        /// SAHL.Common.BusinessModel.DAO.Provider_DAO.Key
		/// </summary>
		public Int32 Key 
		{
			get { return _DAO.Key; }
			set { _DAO.Key = value;}
		}
		/// <summary>
        /// SAHL.Common.BusinessModel.DAO.Provider_DAO.Description
		/// </summary>
        public String Description 
		{
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
		}
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Provider_DAO.URL
        /// </summary>
        public String URL
        {
            get { return _DAO.URL; }
            set { _DAO.URL = value; }
        }

		/// <summary>
        /// SAHL.Common.BusinessModel.DAO.Provider_DAO.GeneralStatus
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
        /// SAHL.Common.BusinessModel.DAO.Provider_DAO.IsDefault
		/// </summary>
        public bool IsDefault 
		{
            get { return _DAO.IsDefault; }
            set { _DAO.IsDefault = value; }
		}

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Provider_DAO.MaxCollectionAmount
        /// </summary>
        public decimal MaxCollectionAmount
        {
            get { return _DAO.MaxCollectionAmount; }
            set { _DAO.MaxCollectionAmount = value; }
        }
	}
}


