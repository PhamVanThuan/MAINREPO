using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO
    /// </summary>
    public partial interface IImportApplication : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ApplicationAmount
        /// </summary>
        System.Double ApplicationAmount
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ApplicationStartDate
        /// </summary>
        DateTime? ApplicationStartDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ApplicationEndDate
        /// </summary>
        DateTime? ApplicationEndDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.MortgageLoanPurposeKey
        /// </summary>
        System.String MortgageLoanPurposeKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ApplicantTypeKey
        /// </summary>
        System.String ApplicantTypeKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.NumberApplicants
        /// </summary>
        System.Int32 NumberApplicants
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.HomePurchaseDate
        /// </summary>
        DateTime? HomePurchaseDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.BondRegistrationDate
        /// </summary>
        DateTime? BondRegistrationDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.CurrentBondValue
        /// </summary>
        System.Double CurrentBondValue
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.DeedsOfficeDate
        /// </summary>
        DateTime? DeedsOfficeDate
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.BondFinancialInstitution
        /// </summary>
        System.String BondFinancialInstitution
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ExistingLoan
        /// </summary>
        System.Double ExistingLoan
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.PurchasePrice
        /// </summary>
        System.Double PurchasePrice
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.Reference
        /// </summary>
        System.String Reference
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ErrorMsg
        /// </summary>
        System.String ErrorMsg
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ImportID
        /// </summary>
        System.Int32 ImportID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ImportLegalEntities
        /// </summary>
        IEventList<IImportLegalEntity> ImportLegalEntities
        {
            get;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ImportStatus
        /// </summary>
        IImportStatus ImportStatus
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportApplication_DAO.ImportFile
        /// </summary>
        IImportFile ImportFile
        {
            get;
            set;
        }
    }
}