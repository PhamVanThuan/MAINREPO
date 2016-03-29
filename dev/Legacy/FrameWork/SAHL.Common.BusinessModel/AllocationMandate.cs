using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AllocationMandate_DAO
    /// </summary>
    public partial class AllocationMandate : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AllocationMandate_DAO>, IAllocationMandate
    {
        public AllocationMandate(SAHL.Common.BusinessModel.DAO.AllocationMandate_DAO AllocationMandate)
            : base(AllocationMandate)
        {
            this._DAO = AllocationMandate;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandate_DAO.Name
        /// </summary>
        public String Name
        {
            get { return _DAO.Name; }
            set { _DAO.Name = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandate_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandate_DAO.TypeName
        /// </summary>
        public String TypeName
        {
            get { return _DAO.TypeName; }
            set { _DAO.TypeName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandate_DAO.ParameterValue
        /// </summary>
        public String ParameterValue
        {
            get { return _DAO.ParameterValue; }
            set { _DAO.ParameterValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandate_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AllocationMandate_DAO.ParameterType
        /// </summary>
        public IParameterType ParameterType
        {
            get
            {
                if (null == _DAO.ParameterType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IParameterType, ParameterType_DAO>(_DAO.ParameterType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ParameterType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ParameterType = (ParameterType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}