using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// The Employer_DAO class is used to instantiate a new Employer.
    /// </summary>
    [ActiveRecord("Employer", Schema = "dbo")]
    public partial class Employer_DAO : DB_2AM<Employer_DAO>
    {
        private string _name;

        private string _telephoneNumber;

        private string _telephoneCode;

        private string _contactPerson;

        private string _contactEmail;

        private string _accountantName;

        private string _accountantContactPerson;

        private string _accountantTelephoneCode;

        private string _accountantTelephoneNumber;

        private string _accountantEmail;

        private EmployerBusinessType_DAO _employerBusinessType;

        private string _userID;

        private System.DateTime _changeDate;

        private int _Key;

        private EmploymentSector_DAO _employmentSector;

        /// <summary>
        /// The Employer's Name
        /// </summary>
        [Property("Name", ColumnType = "String", NotNull = true, Length = 50)]
        [ValidateNonEmpty("Employer Name is a mandatory field")]
        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        /// <summary>
        /// The Employers Telephone Number
        /// </summary>
        [Property("TelephoneNumber", ColumnType = "String", Length = 15, NotNull = true)]
        [ValidateNonEmpty("Telephone Number is a mandatory field")]
        public virtual string TelephoneNumber
        {
            get
            {
                return this._telephoneNumber;
            }
            set
            {
                this._telephoneNumber = value;
            }
        }

        /// <summary>
        /// The Area Code of the Employer's Telephone Number
        /// </summary>
        [Property("TelephoneCode", ColumnType = "String", Length = 10, NotNull = true)]
        [ValidateNonEmpty("Telephone Code is a mandatory field")]
        public virtual string TelephoneCode
        {
            get
            {
                return this._telephoneCode;
            }
            set
            {
                this._telephoneCode = value;
            }
        }

        /// <summary>
        /// A Contact Person for the Employer
        /// </summary>
        [Property("ContactPerson", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Contact Person is a mandatory field")]
        public virtual string ContactPerson
        {
            get
            {
                return this._contactPerson;
            }
            set
            {
                this._contactPerson = value;
            }
        }

        /// <summary>
        /// An email address for the Employer
        /// </summary>
        [Property("ContactEmail", ColumnType = "String")]
        public virtual string ContactEmail
        {
            get
            {
                return this._contactEmail;
            }
            set
            {
                this._contactEmail = value;
            }
        }

        /// <summary>
        /// The Employer's Accountant.
        /// </summary>
        [Property("AccountantName", ColumnType = "String")]
        public virtual string AccountantName
        {
            get
            {
                return this._accountantName;
            }
            set
            {
                this._accountantName = value;
            }
        }

        /// <summary>
        /// A Contact Person for the Accountant.
        /// </summary>
        [Property("AccountantContactPerson", ColumnType = "String")]
        public virtual string AccountantContactPerson
        {
            get
            {
                return this._accountantContactPerson;
            }
            set
            {
                this._accountantContactPerson = value;
            }
        }

        /// <summary>
        /// The Area Code for the Accountant's telephone number.
        /// </summary>
        [Property("AccountantTelephoneCode", ColumnType = "String", Length = 10)]
        public virtual string AccountantTelephoneCode
        {
            get
            {
                return this._accountantTelephoneCode;
            }
            set
            {
                this._accountantTelephoneCode = value;
            }
        }

        /// <summary>
        /// The Telephone Number for the Accountant.
        /// </summary>
        [Property("AccountantTelephoneNumber", ColumnType = "String", Length = 15)]
        public virtual string AccountantTelephoneNumber
        {
            get
            {
                return this._accountantTelephoneNumber;
            }
            set
            {
                this._accountantTelephoneNumber = value;
            }
        }

        /// <summary>
        /// An email address for the Accountant.
        /// </summary>
        [Property("AccountantEmail", ColumnType = "String")]
        public virtual string AccountantEmail
        {
            get
            {
                return this._accountantEmail;
            }
            set
            {
                this._accountantEmail = value;
            }
        }

        /// <summary>
        /// Each Employer belongs to a specific business type. This is a foreign key reference to the EmployerBusinessType table.
        /// </summary>
        [BelongsTo("EmployerBusinessTypeKey", NotNull = true)]
        [ValidateNonEmpty("Employer Business Type is a mandatory field")]
        public virtual EmployerBusinessType_DAO EmployerBusinessType
        {
            get
            {
                return this._employerBusinessType;
            }
            set
            {
                this._employerBusinessType = value;
            }
        }

        /// <summary>
        /// The UserID of the person who last updated the Employer record.
        /// </summary>
        [Property("UserID", ColumnType = "String", NotNull = true)]
        public virtual string UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        /// <summary>
        /// The date on which the Employer record was last changed.
        /// </summary>
        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "EmployerKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        /// <summary>
        /// Each Employer belongs to a specific Employment Sector. This is a foreign key reference to the EmploymentSector table.
        /// </summary>
        [BelongsTo("EmploymentSectorKey", NotNull = true)]
        [ValidateNonEmpty("Employment Sector is a mandatory field")]
        public virtual EmploymentSector_DAO EmploymentSector
        {
            get
            {
                return this._employmentSector;
            }
            set
            {
                this._employmentSector = value;
            }
        }

        /// <summary>
        /// Gets a list of all employers starting with the supplied <c>prefix</c>.
        /// </summary>
        public static IList<Employer_DAO> FindByPrefix(string prefix, int maxRowCount)
        {
            SimpleQuery q = new SimpleQuery(typeof(Employer_DAO), @"
                from Employer_DAO e
                where e.Name LIKE ?
                ",
                prefix + "%"
            );
            q.SetQueryRange(maxRowCount);
            return new List<Employer_DAO>((Employer_DAO[])ExecuteQuery(q));
        }
    }
}