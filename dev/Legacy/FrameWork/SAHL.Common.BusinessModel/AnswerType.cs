using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AnswerType_DAO
    /// </summary>
    public partial class AnswerType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AnswerType_DAO>, IAnswerType
    {
        public AnswerType(SAHL.Common.BusinessModel.DAO.AnswerType_DAO AnswerType)
            : base(AnswerType)
        {
            this._DAO = AnswerType;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AnswerType_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AnswerType_DAO.Description
        /// </summary>
        public String Description
        {
            get { return _DAO.Description; }
            set { _DAO.Description = value; }
        }
    }
}