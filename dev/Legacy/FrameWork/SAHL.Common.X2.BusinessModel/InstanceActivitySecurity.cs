using System;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class InstanceActivitySecurity : IEntityValidation, IInstanceActivitySecurity, IDAOObject
    {
        protected SAHL.Common.X2.BusinessModel.DAO.InstanceActivitySecurity_DAO _InstanceActivitySecurity;

        public InstanceActivitySecurity(SAHL.Common.X2.BusinessModel.DAO.InstanceActivitySecurity_DAO InstanceActivitySecurity)
        {
            this._InstanceActivitySecurity = InstanceActivitySecurity;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns><see cref="InstanceActivitySecurity_DAO"/></returns>
        public object GetDAOObject()
        {
            return _InstanceActivitySecurity;
        }

        /// <summary>
        ///
        /// </summary>
        public Int32 ID
        {
            get { return _InstanceActivitySecurity.ID; }
            set { _InstanceActivitySecurity.ID = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String ADUserName
        {
            get { return _InstanceActivitySecurity.ADUserName; }
            set { _InstanceActivitySecurity.ADUserName = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public IActivity Activity
        {
            get
            {
                if (null == _InstanceActivitySecurity.Activity) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IActivity, Activity_DAO>(_InstanceActivitySecurity.Activity);
                }
            }

            set
            {
                if (value == null)
                {
                    _InstanceActivitySecurity.Activity = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _InstanceActivitySecurity.Activity = (Activity_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IInstance Instance
        {
            get
            {
                if (null == _InstanceActivitySecurity.Instance) return null;
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IInstance, Instance_DAO>(_InstanceActivitySecurity.Instance);
                }
            }

            set
            {
                if (value == null)
                {
                    _InstanceActivitySecurity.Instance = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _InstanceActivitySecurity.Instance = (Instance_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}