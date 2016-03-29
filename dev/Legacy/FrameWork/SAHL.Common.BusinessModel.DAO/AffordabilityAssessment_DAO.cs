using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("AffordabilityAssessment", Schema = "dbo", Lazy = true)]
    public partial class AffordabilityAssessment_DAO : DB_2AM<AffordabilityAssessment_DAO>
    {
        private int _key;

        private int _genericKey;

        private AffordabilityAssessmentStatus_DAO _affordabilityAssessmentStatus;

        private AffordabilityAssessmentStressFactor_DAO _affordabilityAssessmentStressFactor;

        private DateTime _modifiedDate;

        private ADUser_DAO _modifiedByUser;

        private int _numberOfContributingApplicants;

        private int _numberOfHouseholdDependants;

        private int? _minimumMonthlyFixedExpenses;

        private GeneralStatus_DAO _generalStatus;

        private GenericKeyType_DAO _genericKeyType;

        private DateTime? _confirmedDate;

        private string _notes;

        [PrimaryKey(PrimaryKeyType.Native, "AffordabilityAssessmentKey", ColumnType = "Int32")]
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

        [Property("GenericKey", ColumnType = "Int32", NotNull = true)]
        public virtual int GenericKey
        {
            get
            {
                return this._genericKey;
            }
            set
            {
                this._genericKey = value;
            }
        }

        [BelongsTo("AffordabilityAssessmentStatusKey", NotNull = true)]
        public virtual AffordabilityAssessmentStatus_DAO AffordabilityAssessmentStatus
        {
            get
            {
                return this._affordabilityAssessmentStatus;
            }
            set
            {
                this._affordabilityAssessmentStatus = value;
            }
        }


        [BelongsTo("AffordabilityAssessmentStressFactorKey", NotNull = true)]
        public virtual AffordabilityAssessmentStressFactor_DAO AffordabilityAssessmentStressFactor
        {
            get
            {
                return this._affordabilityAssessmentStressFactor;
            }
            set
            {
                this._affordabilityAssessmentStressFactor = value;
            }
        }

        [Property("ModifiedDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual System.DateTime ModifiedDate
        {
            get
            {
                return this._modifiedDate ;
            }
            set
            {
                this._modifiedDate = value;
            }
        }

        [BelongsTo("ModifiedByUserId", NotNull = true)]
        public virtual ADUser_DAO ModifiedByUser
        {
            get
            {
                return this._modifiedByUser;
            }
            set
            {
                this._modifiedByUser = value;
            }
        }

        [Property("NumberOfContributingApplicants", ColumnType = "Int32", NotNull = true)]
        public virtual int NumberOfContributingApplicants
        {
            get
            {
                return this._numberOfContributingApplicants;
            }
            set
            {
                this._numberOfContributingApplicants = value;
            }
        }

        [Property("NumberOfHouseholdDependants", ColumnType = "Int32", NotNull = true)]
        public virtual int NumberOfHouseholdDependants
        {
            get
            {
                return this._numberOfHouseholdDependants;
            }
            set
            {
                this._numberOfHouseholdDependants = value;
            }
        }

        [Property("MinimumMonthlyFixedExpenses", ColumnType = "Int32")]
        public virtual int? MinimumMonthlyFixedExpenses
        {
            get
            {
                return this._minimumMonthlyFixedExpenses;
            }
            set
            {
                this._minimumMonthlyFixedExpenses = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        public virtual GenericKeyType_DAO GenericKeyType
        {
            get
            {
                return this._genericKeyType;
            }
            set
            {
                this._genericKeyType = value;
            }
        }

        [Property("ConfirmedDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ConfirmedDate
        {
            get
            {
                return this._confirmedDate;
            }
            set
            {
                this._confirmedDate = value;
            }
        }

        [Property("Notes", ColumnType = "String")]
        public virtual string Notes
        {
            get
            {
                return this._notes;
            }
            set
            {
                this._notes = value;
            }
        }
    }
}
