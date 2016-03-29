using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO
    /// </summary>
    public partial class AssetTransfer : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO>, IAssetTransfer
    {
        public AssetTransfer(SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO AssetTransfer)
            : base(AssetTransfer)
        {
            this._DAO = AssetTransfer;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.ClientSurname
        /// </summary>
        public String ClientSurname
        {
            get { return _DAO.ClientSurname; }
            set { _DAO.ClientSurname = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.SPVKey
        /// </summary>
        public Int32 SPVKey
        {
            get { return _DAO.SPVKey; }
            set { _DAO.SPVKey = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.LoanTotalBondAmount
        /// </summary>
        public Double LoanTotalBondAmount
        {
            get { return _DAO.LoanTotalBondAmount; }
            set { _DAO.LoanTotalBondAmount = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.LoanCurrentBalance
        /// </summary>
        public Double LoanCurrentBalance
        {
            get { return _DAO.LoanCurrentBalance; }
            set { _DAO.LoanCurrentBalance = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.UserName
        /// </summary>
        public String UserName
        {
            get { return _DAO.UserName; }
            set { _DAO.UserName = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.TransferedYN
        /// </summary>
        public Char TransferedYN
        {
            get { return _DAO.TransferedYN; }
            set { _DAO.TransferedYN = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AssetTransfer_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}