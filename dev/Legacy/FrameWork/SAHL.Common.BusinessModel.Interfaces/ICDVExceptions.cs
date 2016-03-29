using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO
    /// </summary>
    public partial interface ICDVExceptions : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.NoOfDigits
        /// </summary>
        Int32? NoOfDigits
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.Weightings
        /// </summary>
        System.String Weightings
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.Modulus
        /// </summary>
        Int32? Modulus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.FudgeFactor
        /// </summary>
        Int32? FudgeFactor
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.ExceptionCode
        /// </summary>
        System.String ExceptionCode
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.DateChange
        /// </summary>
        System.DateTime DateChange
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.ACBBank
        /// </summary>
        IACBBank ACBBank
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDVExceptions_DAO.ACBType
        /// </summary>
        IACBType ACBType
        {
            get;
            set;
        }
    }
}