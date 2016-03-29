using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO
    /// </summary>
    public partial class CDVExceptions : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO>, ICDVExceptions
    {
        public CDVExceptions(SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO CDVExceptions)
            : base(CDVExceptions)
        {
            this._DAO = CDVExceptions;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.NoOfDigits
        /// </summary>
        public Int32? NoOfDigits
        {
            get { return _DAO.NoOfDigits; }
            set { _DAO.NoOfDigits = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.Weightings
        /// </summary>
        public String Weightings
        {
            get { return _DAO.Weightings; }
            set { _DAO.Weightings = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.Modulus
        /// </summary>
        public Int32? Modulus
        {
            get { return _DAO.Modulus; }
            set { _DAO.Modulus = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.FudgeFactor
        /// </summary>
        public Int32? FudgeFactor
        {
            get { return _DAO.FudgeFactor; }
            set { _DAO.FudgeFactor = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.ExceptionCode
        /// </summary>
        public String ExceptionCode
        {
            get { return _DAO.ExceptionCode; }
            set { _DAO.ExceptionCode = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.UserID
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.DateChange
        /// </summary>
        public DateTime DateChange
        {
            get { return _DAO.DateChange; }
            set { _DAO.DateChange = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.ACBBank
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
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.ACBType
        /// </summary>
        public IACBType ACBType
        {
            get
            {
                if (null == _DAO.ACBType) return null;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IACBType, ACBType_DAO>(_DAO.ACBType);
                }
            }

            set
            {
                if (value == null)
                {
                    _DAO.ACBType = null;
                    return;
                }
                IDAOObject obj = value as IDAOObject;

                if (obj != null)
                    _DAO.ACBType = (ACBType_DAO)obj.GetDAOObject();
                else
                    throw new ArgumentException("The Business Object could not be cast to the underlying DAO Object.");
            }
        }
    }
}