using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CorrespondenceMedium_DAO
    /// </summary>
    public partial class CorrespondenceMedium : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CorrespondenceMedium_DAO>, ICorrespondenceMedium
    {
        public CorrespondenceMedium(SAHL.Common.BusinessModel.DAO.CorrespondenceMedium_DAO CorrespondenceMedium)
            : base(CorrespondenceMedium)
        {
            this._DAO = CorrespondenceMedium;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceMedium_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CorrespondenceMedium_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}