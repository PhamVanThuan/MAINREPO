using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.CDV_DAO
    /// </summary>
    public partial interface ICDV : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.ACBTypeNumber
        /// </summary>
        System.Int32 ACBTypeNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.StreamCode
        /// </summary>
        Int32? StreamCode
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.ExceptionStreamCode
        /// </summary>
        Int32? ExceptionStreamCode
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.Weightings
        /// </summary>
        System.String Weightings
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.Modulus
        /// </summary>
        Int32? Modulus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.FudgeFactor
        /// </summary>
        Int32? FudgeFactor
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.ExceptionCode
        /// </summary>
        System.String ExceptionCode
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.AccountIndicator
        /// </summary>
        Int32? AccountIndicator
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.UserID
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.DateChange
        /// </summary>
        System.DateTime DateChange
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.ACBBank
        /// </summary>
        IACBBank ACBBank
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.CDV_DAO.ACBBranch
        /// </summary>
        IACBBranch ACBBranch
        {
            get;
            set;
        }
    }
}