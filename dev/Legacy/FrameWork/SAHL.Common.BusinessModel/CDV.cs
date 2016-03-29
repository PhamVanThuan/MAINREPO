using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CDV_DAO
    /// </summary>
    public partial class CDV : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CDV_DAO>, ICDV
    {
        public CDV(SAHL.Common.BusinessModel.DAO.CDV_DAO CDV)
            : base(CDV)
        {
            this._DAO = CDV;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.ACBTypeNumber
        /// </summary>
        public Int32 ACBTypeNumber
        {
            get { return _DAO.ACBTypeNumber; }
            set { _DAO.ACBTypeNumber = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.StreamCode
        /// </summary>
        public Int32? StreamCode
        {
            get { return _DAO.StreamCode; }
            set { _DAO.StreamCode = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.ExceptionStreamCode
        /// </summary>
        public Int32? ExceptionStreamCode
        {
            get { return _DAO.ExceptionStreamCode; }
            set { _DAO.ExceptionStreamCode = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.Weightings
        /// </summary>
        public String Weightings
        {
            get { return _DAO.Weightings; }
            set { _DAO.Weightings = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.Modulus
        /// </summary>
        public Int32? Modulus
        {
            get { return _DAO.Modulus; }
            set { _DAO.Modulus = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.FudgeFactor
        /// </summary>
        public Int32? FudgeFactor
        {
            get { return _DAO.FudgeFactor; }
            set { _DAO.FudgeFactor = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.ExceptionCode
        /// </summary>
        public String ExceptionCode
        {
            get { return _DAO.ExceptionCode; }
            set { _DAO.ExceptionCode = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.AccountIndicator
        /// </summary>
        public Int32? AccountIndicator
        {
            get { return _DAO.AccountIndicator; }
            set { _DAO.AccountIndicator = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.UserID
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.DateChange
        /// </summary>
        public DateTime DateChange
        {
            get { return _DAO.DateChange; }
            set { _DAO.DateChange = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.ACBBank
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
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.ACBBranch
        /// </summary>
        public IACBBranch ACBBranch
        {
            get
            {
                if (null == _DAO.ACBBranch) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IACBBranch, ACBBranch_DAO>(_DAO.ACBBranch);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ACBBranch = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ACBBranch = (ACBBranch_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}