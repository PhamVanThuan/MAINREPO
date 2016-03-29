using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ACBBranch_DAO
    /// </summary>
    public partial class ACBBranch : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ACBBranch_DAO>, IACBBranch
    {
        public ACBBranch(SAHL.Common.BusinessModel.DAO.ACBBranch_DAO ACBBranch)
            : base(ACBBranch)
        {
            this._DAO = ACBBranch;
        }

        /// <summary>
        /// The primary key from the ACBBank table to which the branch belongs.
        /// </summary>
        public IACBBank ACBBank
        {
            get
            {
                if (null == _DAO.ACBBank) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IACBBank, ACBBank_DAO>(_DAO.ACBBank);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ACBBank = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ACBBank = (ACBBank_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }

        /// <summary>
        /// The description of the branch. e.g. Durban North
        /// </summary>
        public String ACBBranchDescription
        {
            get { return _DAO.ACBBranchDescription; }
            set { _DAO.ACBBranchDescription = value; }
        }

        /// <summary>
        /// Indicates whether this branch record is active or not.
        /// </summary>
        public Char ActiveIndicator
        {
            get { return _DAO.ActiveIndicator; }
            set { _DAO.ActiveIndicator = value; }
        }

        /// <summary>
        /// The distinct branch code which is allocated to each branch of a bank.
        /// </summary>
        public String Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }
    }
}