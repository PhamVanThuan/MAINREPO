using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ACBType_DAO
    /// </summary>
    public partial class ACBType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ACBType_DAO>, IACBType
    {
        public ACBType(SAHL.Common.BusinessModel.DAO.ACBType_DAO ACBType)
            : base(ACBType)
        {
            this._DAO = ACBType;
        }

        /// <summary>
        /// The type of bank account. i.e. A Cheque or Savings account
        /// </summary>
        public String ACBTypeDescription
        {
            get { return _DAO.ACBTypeDescription; }
            set { _DAO.ACBTypeDescription = value; }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}