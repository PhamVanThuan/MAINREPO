using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CancellationType_DAO
    /// </summary>
    public partial class CancellationType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CancellationType_DAO>, ICancellationType
    {
        public CancellationType(SAHL.Common.BusinessModel.DAO.CancellationType_DAO CancellationType)
            : base(CancellationType)
        {
            this._DAO = CancellationType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CancellationType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CancellationType_DAO.CancellationWebCode
        /// </summary>
        public String CancellationWebCode
        {
            get { return _DAO.CancellationWebCode; }
            set { _DAO.CancellationWebCode = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CancellationType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}