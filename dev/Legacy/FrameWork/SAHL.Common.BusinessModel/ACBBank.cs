using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ACBBank_DAO
    /// </summary>
    public partial class ACBBank : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ACBBank_DAO>, IACBBank
    {
        public ACBBank(SAHL.Common.BusinessModel.DAO.ACBBank_DAO ACBBank)
            : base(ACBBank)
        {
            this._DAO = ACBBank;
        }

        /// <summary>
        /// Contains the description of the bank i.e. Nedbank, ABSA etc
        /// </summary>
        public String ACBBankDescription
        {
            get { return _DAO.ACBBankDescription; }
            set { _DAO.ACBBankDescription = value; }
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