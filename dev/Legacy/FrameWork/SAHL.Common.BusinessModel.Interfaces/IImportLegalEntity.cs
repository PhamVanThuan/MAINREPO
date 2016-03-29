using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO
    /// </summary>
    public partial interface IImportLegalEntity : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.MaritalStatusKey
        /// </summary>
        System.String MaritalStatusKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.GenderKey
        /// </summary>
        System.String GenderKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.CitizenTypeKey
        /// </summary>
        System.String CitizenTypeKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.SalutationKey
        /// </summary>
        System.String SalutationKey
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.FirstNames
        /// </summary>
        System.String FirstNames
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.Initials
        /// </summary>
        System.String Initials
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.Surname
        /// </summary>
        System.String Surname
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.PreferredName
        /// </summary>
        System.String PreferredName
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.IDNumber
        /// </summary>
        System.String IDNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.PassportNumber
        /// </summary>
        System.String PassportNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.TaxNumber
        /// </summary>
        System.String TaxNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.HomePhoneCode
        /// </summary>
        System.String HomePhoneCode
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.HomePhoneNumber
        /// </summary>
        System.String HomePhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.WorkPhoneCode
        /// </summary>
        System.String WorkPhoneCode
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.WorkPhoneNumber
        /// </summary>
        System.String WorkPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.CellPhoneNumber
        /// </summary>
        System.String CellPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.EmailAddress
        /// </summary>
        System.String EmailAddress
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.FaxCode
        /// </summary>
        System.String FaxCode
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.FaxNumber
        /// </summary>
        System.String FaxNumber
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.ImportID
        /// </summary>
        System.Int32 ImportID
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO.ImportApplication
        /// </summary>
        IImportApplication ImportApplication
        {
            get;
            set;
        }
    }
}