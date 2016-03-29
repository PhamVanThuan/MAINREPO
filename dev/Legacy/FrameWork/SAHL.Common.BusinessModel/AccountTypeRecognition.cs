using System;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO
    /// </summary>
    public partial class AccountTypeRecognition : BusinessModelBase<SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO>, IAccountTypeRecognition
    {
        public AccountTypeRecognition(SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO AccountTypeRecognition)
            : base(AccountTypeRecognition)
        {
            this._DAO = AccountTypeRecognition;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.RangeStart
        /// </summary>
        public Int64? RangeStart
        {
            get { return _DAO.RangeStart; }
            set { _DAO.RangeStart = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.RangeEnd
        /// </summary>
        public Int64? RangeEnd
        {
            get { return _DAO.RangeEnd; }
            set { _DAO.RangeEnd = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.NoOfDigits1
        /// </summary>
        public Int32? NoOfDigits1
        {
            get { return _DAO.NoOfDigits1; }
            set { _DAO.NoOfDigits1 = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.NoOfDigits2
        /// </summary>
        public Int32? NoOfDigits2
        {
            get { return _DAO.NoOfDigits2; }
            set { _DAO.NoOfDigits2 = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.DigitNo1
        /// </summary>
        public Int32? DigitNo1
        {
            get { return _DAO.DigitNo1; }
            set { _DAO.DigitNo1 = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.MustEqual1
        /// </summary>
        public Int32? MustEqual1
        {
            get { return _DAO.MustEqual1; }
            set { _DAO.MustEqual1 = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.DigitNo2
        /// </summary>
        public Int32? DigitNo2
        {
            get { return _DAO.DigitNo2; }
            set { _DAO.DigitNo2 = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.MustEqual2
        /// </summary>
        public Int32? MustEqual2
        {
            get { return _DAO.MustEqual2; }
            set { _DAO.MustEqual2 = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.DropDigits
        /// </summary>
        public Char DropDigits
        {
            get { return _DAO.DropDigits; }
            set { _DAO.DropDigits = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.StartDropDigits
        /// </summary>
        public Int32? StartDropDigits
        {
            get { return _DAO.StartDropDigits; }
            set { _DAO.StartDropDigits = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.EndDropDigits
        /// </summary>
        public Int32? EndDropDigits
        {
            get { return _DAO.EndDropDigits; }
            set { _DAO.EndDropDigits = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.UserID
        /// </summary>
        public String UserID
        {
            get { return _DAO.UserID; }
            set { _DAO.UserID = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.DateChange
        /// </summary>
        public DateTime? DateChange
        {
            get { return _DAO.DateChange; }
            set { _DAO.DateChange = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.Key
        /// </summary>
        public Int32 Key
        {
            get { return _DAO.Key; }
            set { _DAO.Key = value; }
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.ACBBank
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
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.ACBType
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