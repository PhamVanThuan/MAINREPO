using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Interfaces;

namespace SAHL.Common.BusinessModel.Interfaces
{
    /// <summary>
    /// The Employer_DAO class is used to instantiate a new Employer.
    /// </summary>
    public partial interface IEmployer : IEntityValidation, IBusinessModelObject
    {
        /// <summary>
        /// The Employer's Name
        /// </summary>
        System.String Name
        {
            get;
            set;
        }

        /// <summary>
        /// The Employers Telephone Number
        /// </summary>
        System.String TelephoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// The Area Code of the Employer's Telephone Number
        /// </summary>
        System.String TelephoneCode
        {
            get;
            set;
        }

        /// <summary>
        /// A Contact Person for the Employer
        /// </summary>
        System.String ContactPerson
        {
            get;
            set;
        }

        /// <summary>
        /// An email address for the Employer
        /// </summary>
        System.String ContactEmail
        {
            get;
            set;
        }

        /// <summary>
        /// The Employer's Accountant.
        /// </summary>
        System.String AccountantName
        {
            get;
            set;
        }

        /// <summary>
        /// A Contact Person for the Accountant.
        /// </summary>
        System.String AccountantContactPerson
        {
            get;
            set;
        }

        /// <summary>
        /// The Area Code for the Accountant's telephone number.
        /// </summary>
        System.String AccountantTelephoneCode
        {
            get;
            set;
        }

        /// <summary>
        /// The Telephone Number for the Accountant.
        /// </summary>
        System.String AccountantTelephoneNumber
        {
            get;
            set;
        }

        /// <summary>
        /// An email address for the Accountant.
        /// </summary>
        System.String AccountantEmail
        {
            get;
            set;
        }

        /// <summary>
        /// Each Employer belongs to a specific business type. This is a foreign key reference to the EmployerBusinessType table.
        /// </summary>
        IEmployerBusinessType EmployerBusinessType
        {
            get;
            set;
        }

        /// <summary>
        /// The UserID of the person who last updated the Employer record.
        /// </summary>
        System.String UserID
        {
            get;
            set;
        }

        /// <summary>
        /// The date on which the Employer record was last changed.
        /// </summary>
        System.DateTime ChangeDate
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
        /// Each Employer belongs to a specific Employment Sector. This is a foreign key reference to the EmploymentSector table.
        /// </summary>
        IEmploymentSector EmploymentSector
        {
            get;
            set;
        }
    }
}