using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.BulkBatchParameter_DAO
    /// </summary>
    public partial class BulkBatchParameter : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BulkBatchParameter_DAO>, IBulkBatchParameter
    {
        public BulkBatchParameter(SAHL.Common.BusinessModel.DAO.BulkBatchParameter_DAO BulkBatchParameter)
            : base(BulkBatchParameter)
        {
            this._DAO = BulkBatchParameter;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchParameter_DAO.ParameterName
        /// </summary>
        public String ParameterName
        {
            get { return _DAO.ParameterName; }
            set { _DAO.ParameterName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchParameter_DAO.ParameterValue
        /// </summary>
        public String ParameterValue
        {
            get { return _DAO.ParameterValue; }
            set { _DAO.ParameterValue = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchParameter_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.BulkBatchParameter_DAO.BulkBatch
        /// </summary>
        public IBulkBatch BulkBatch
        {
            get
            {
                if (null == _DAO.BulkBatch) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBulkBatch, BulkBatch_DAO>(_DAO.BulkBatch);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.BulkBatch = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.BulkBatch = (BulkBatch_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}