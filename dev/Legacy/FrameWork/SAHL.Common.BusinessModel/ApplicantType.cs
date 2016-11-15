using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ApplicantType_DAO
    /// </summary>
    public partial class ApplicantType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ApplicantType_DAO>, IApplicantType
    {
        public ApplicantType(SAHL.Common.BusinessModel.DAO.ApplicantType_DAO ApplicantType)
            : base(ApplicantType)
        {
            this._DAO = ApplicantType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicantType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ApplicantType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}