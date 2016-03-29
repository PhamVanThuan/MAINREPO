using System;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// Employment_DAO is instantiated in order to create an Employment record for a Legal Entity. It is discriminated based on the
    /// Employment Type.
    /// </summary>
    public partial interface IEmployment : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The date on which the Legal Entity was employed with the employer
        /// </summary>
        DateTime? EmploymentStartDate
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the employment was ended.
        /// </summary>
        DateTime? EmploymentEndDate
        {
            get;
            set;
        }

        /// <summary>
        /// A Reference for the Legal Entity's Employment.
        /// </summary>
        System.String ContactPerson
        {
            get;
            set;
        }

        /// <summary>
        /// The Phone Number for the Contact Person.
        /// </summary>
        System.String ContactPhoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// The Area Code for the Contact Person's Phone Number.
        /// </summary>
        System.String ContactPhoneCode
        {
            get;
            set;
        }

        /// <summary>
        /// The SAHL Employee who confirmed the Legal Entity's Income.
        /// </summary>
        System.String ConfirmedBy
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the Legal Entity's Income was confirmed.
        /// </summary>
        DateTime? ConfirmedDate
        {
            get;
            set;
        }

        /// <summary>
        /// The UserID of the user who last updated the Employment Record.
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the Employment record was last changed.
        /// </summary>
        DateTime? ChangeDate
        {
            get;
            set;
        }

        /// <summary>
        /// The department in which the Legal Entity works.
        /// </summary>
        System.String Department
        {
            get;
            set;
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        System.Int32 Key
        {
            get;
            set;
        }

        /// <summary>
        /// Each Employment record belongs to a single Employer. This is the foreign key reference to the Employer table.
        /// </summary>
        IEmployer Employer
        {
            get;
            set;
        }

        /// <summary>
        /// Each Employment record belongs to a LegalEntity. This is the foreign key reference to the LegalEntity table.
        /// </summary>
        ILegalEntity LegalEntity
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Employment_DAO.ConfirmedEmploymentFlag
        /// </summary>
        Boolean? ConfirmedEmploymentFlag
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Employment_DAO.ConfirmedIncomeFlag
        /// </summary>
        Boolean? ConfirmedIncomeFlag
        {
            get;
            set;
        }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Employment_DAO.SalaryPaymentDay
        /// </summary>
        System.Int32? SalaryPaymentDay
        {
            get;
            set;
        }

        /// <summary>
        /// yes,no unknown
        /// </summary>
        bool? UnionMember { get; set; }

        /// <summary>
        /// SAHL.Common.BusinessModel.DAO.Employment_DAO.EmploymentVerificationProcesses
        /// </summary>
        IEventList<IEmploymentVerificationProcess> EmploymentVerificationProcesses
        {
            get;
        }
    }
}