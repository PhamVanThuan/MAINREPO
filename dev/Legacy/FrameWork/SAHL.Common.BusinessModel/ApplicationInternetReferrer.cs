using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO
    /// </summary>
    public partial class ApplicationInternetReferrer : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO>, IApplicationInternetReferrer
    {
        public ApplicationInternetReferrer(SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO ApplicationInternetReferrer)
            : base(ApplicationInternetReferrer)
        {
            this._DAO = ApplicationInternetReferrer;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO.Application
        /// </summary>
        public IApplication Application
        {
            get
            {
                if (null == _DAO.Application) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplication, Application_DAO>(_DAO.Application);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.Application = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.Application = (Application_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO.UserURL
        /// </summary>
        public String UserURL
        {
            get { return _DAO.UserURL; }
            set { _DAO.UserURL = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO.ReferringServerURL
        /// </summary>
        public String ReferringServerURL
        {
            get { return _DAO.ReferringServerURL; }
            set { _DAO.ReferringServerURL = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicationInternetReferrer_DAO.Parameters
        /// </summary>
        public String Parameters
        {
            get { return _DAO.Parameters; }
            set { _DAO.Parameters = value; }
        }
    }
}