using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO
    /// </summary>
    public partial interface IAccountTypeRecognition : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.RangeStart
        /// </summary>
        Int64? RangeStart
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.RangeEnd
        /// </summary>
        Int64? RangeEnd
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.NoOfDigits1
        /// </summary>
        Int32? NoOfDigits1
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.NoOfDigits2
        /// </summary>
        Int32? NoOfDigits2
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.DigitNo1
        /// </summary>
        Int32? DigitNo1
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.MustEqual1
        /// </summary>
        Int32? MustEqual1
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.DigitNo2
        /// </summary>
        Int32? DigitNo2
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.MustEqual2
        /// </summary>
        Int32? MustEqual2
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.DropDigits
        /// </summary>
        System.Char DropDigits
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.StartDropDigits
        /// </summary>
        Int32? StartDropDigits
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.EndDropDigits
        /// </summary>
        Int32? EndDropDigits
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.DateChange
        /// </summary>
        DateTime? DateChange
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.ACBBank
        /// </summary>
        IACBBank ACBBank
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.AccountTypeRecognition_DAO.ACBType
        /// </summary>
        IACBType ACBType
        {
            get;
            set;
        }
    }
}