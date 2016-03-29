using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.SPV_DAO
    /// </summary>
    public partial interface ISPV : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.ParentSPV
        /// </summary>
        ISPV ParentSPV
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVCompany
        /// </summary>
        ISPVCompany SPVCompany
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.Description
        /// </summary>
        System.String Description
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.ReportDescription
        /// </summary>
        System.String ReportDescription
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.ResetConfigurationKey
        /// </summary>
        Int32? ResetConfigurationKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.AnniversaryDay
        /// </summary>
        System.Int16 AnniversaryDay
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.FundingWarehouse
        /// </summary>
        Int32? FundingWarehouse
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.CreditProviderNumber
        /// </summary>
        System.String CreditProviderNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.RegistrationNumber
        /// </summary>
        System.String RegistrationNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.BankAccount
        /// </summary>
        IBankAccount BankAccount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.GeneralStatus
        /// </summary>
        IGeneralStatus GeneralStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.ChildSPVs
        /// </summary>
        IEventList<ISPV> ChildSPVs
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVMandates
        /// </summary>
        IEventList<ISPVMandate> SPVMandates
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVFees
        /// </summary>
        IEventList<ISPVFee> SPVFees
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVAttributes
        /// </summary>
        IEventList<ISPVAttribute> SPVAttributes
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.SPV_DAO.SPVBalances
        /// </summary>
        IEventList<ISPVBalance> SPVBalances
        {
            get;
        }
    }
}