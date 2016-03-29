using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ImportEmployment", Schema = "dbo")]
    public partial class ImportEmployment_DAO : DB_2AM<ImportEmployment_DAO>
    {

        private string _employmentTypeKey;

        private string _remunerationTypeKey;

        private string _employmentStatusKey;

        private string _employerName;

        private string _employerContactPerson;

        private string _employerPhoneCode;

        private string _employerPhoneNumber;

        private DateTime? _employmentStartDate;

        private DateTime? _employmentEndDate;

        private double _monthlyIncome;

        private int _key;

        private ImportLegalEntity_DAO _importLegalEntity;

        [Property("EmploymentTypeKey", ColumnType = "String")]
        public virtual string EmploymentTypeKey
        {
            get
            {
                return this._employmentTypeKey;
            }
            set
            {
                this._employmentTypeKey = value;
            }
        }

        [Property("RemunerationTypeKey", ColumnType = "String")]
        public virtual string RemunerationTypeKey
        {
            get
            {
                return this._remunerationTypeKey;
            }
            set
            {
                this._remunerationTypeKey = value;
            }
        }

        [Property("EmploymentStatusKey", ColumnType = "String")]
        public virtual string EmploymentStatusKey
        {
            get
            {
                return this._employmentStatusKey;
            }
            set
            {
                this._employmentStatusKey = value;
            }
        }

        [Property("EmployerName", ColumnType = "String")]
        public virtual string EmployerName
        {
            get
            {
                return this._employerName;
            }
            set
            {
                this._employerName = value;
            }
        }

        [Property("EmployerContactPerson", ColumnType = "String")]
        public virtual string EmployerContactPerson
        {
            get
            {
                return this._employerContactPerson;
            }
            set
            {
                this._employerContactPerson = value;
            }
        }

        [Property("EmployerPhoneCode", ColumnType = "String")]
        public virtual string EmployerPhoneCode
        {
            get
            {
                return this._employerPhoneCode;
            }
            set
            {
                this._employerPhoneCode = value;
            }
        }

        [Property("EmployerPhoneNumber", ColumnType = "String")]
        public virtual string EmployerPhoneNumber
        {
            get
            {
                return this._employerPhoneNumber;
            }
            set
            {
                this._employerPhoneNumber = value;
            }
        }

        [Property("EmploymentStartDate")]
        public virtual DateTime? EmploymentStartDate
        {
            get
            {
                return this._employmentStartDate;
            }
            set
            {
                this._employmentStartDate = value;
            }
        }

        [Property("EmploymentEndDate")]
        public virtual DateTime? EmploymentEndDate
        {
            get
            {
                return this._employmentEndDate;
            }
            set
            {
                this._employmentEndDate = value;
            }
        }

        [Property("MonthlyIncome", ColumnType = "Double")]
        public virtual double MonthlyIncome
        {
            get
            {
                return this._monthlyIncome;
            }
            set
            {
                this._monthlyIncome = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "EmploymentKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        [BelongsTo("LegalEntityKey", NotNull = true)]
        public virtual ImportLegalEntity_DAO ImportLegalEntity
        {
            get
            {
                return this._importLegalEntity;
            }
            set
            {
                this._importLegalEntity = value;
            }
        }
    }
}
