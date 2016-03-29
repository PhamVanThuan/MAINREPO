using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Cleanup_DAO
    /// </summary>
    public partial class Cleanup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Cleanup_DAO>, ICleanup
    {
        public Cleanup(SAHL.Common.BusinessModel.DAO.Cleanup_DAO Cleanup)
            : base(Cleanup)
        {
            this._DAO = Cleanup;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Cleanup_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Cleanup_DAO.Name
        /// </summary>
        public String Name
        {
            get { return _DAO.Name; }
            set { _DAO.Name = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Cleanup_DAO.Query
        /// </summary>
        public String Query
        {
            get { return _DAO.Query; }
            set { _DAO.Query = value; }
        }
    }
}