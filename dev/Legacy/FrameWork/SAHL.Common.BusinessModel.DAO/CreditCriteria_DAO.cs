using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("CreditCriteria", Schema = "dbo", Lazy = true)]
    public partial class CreditCriteria_DAO : DB_2AM<CreditCriteria_DAO>
    {
        private double? _minLoanAmount;

        private Double? _maxLoanAmount;

        private Double? _minPropertyValue;

        private Double? _maxPropertyValue;

        private Double? _lTV;

        private Double? _pTI;

        private Double? _minIncomeAmount;

        private Double? _maxIncomeAmount;

        private bool? _exceptionCriteria;

        private int _key;

        private Category_DAO _category;

        private CreditMatrix_DAO _creditMatrix;

        private EmploymentType_DAO _employmentType;

        private Margin_DAO _margin;

        private MortgageLoanPurpose_DAO _mortgageLoanPurpose;

        private int? _minEmpiricaScore;

        [Property("MinLoanAmount")]
        public virtual double? MinLoanAmount
        {
            get
            {
                return this._minLoanAmount;
            }
            set
            {
                this._minLoanAmount = value;
            }
        }

        [Property("MaxLoanAmount")]
        public virtual Double? MaxLoanAmount
        {
            get
            {
                return this._maxLoanAmount;
            }
            set
            {
                this._maxLoanAmount = value;
            }
        }

        [Property("MinPropertyValue")]
        public virtual Double? MinPropertyValue
        {
            get
            {
                return this._minPropertyValue;
            }
            set
            {
                this._minPropertyValue = value;
            }
        }

        [Property("MaxPropertyValue")]
        public virtual Double? MaxPropertyValue
        {
            get
            {
                return this._maxPropertyValue;
            }
            set
            {
                this._maxPropertyValue = value;
            }
        }

        [Property("LTV")]
        public virtual Double? LTV
        {
            get
            {
                //if (this._lTV != null)
                //    return this._lTV/100;
                //return null;
                return this._lTV;
            }
            set
            {
                this._lTV = value;
            }
        }

        [Property("PTI")]
        public virtual Double? PTI
        {
            get
            {
                //if (this._pTI != null)
                //    return this._pTI/100;
                //return null;
                return this._pTI;
            }
            set
            {
                this._pTI = value;
            }
        }

        [Property("MinIncomeAmount")]
        public virtual Double? MinIncomeAmount
        {
            get
            {
                return this._minIncomeAmount;
            }
            set
            {
                this._minIncomeAmount = value;
            }
        }

        [Property("MaxIncomeAmount")]
        public virtual Double? MaxIncomeAmount
        {
            get
            {
                return this._maxIncomeAmount;
            }
            set
            {
                this._maxIncomeAmount = value;
            }
        }

        [Property("ExceptionCriteria")]
        public virtual bool? ExceptionCriteria
        {
            get
            {
                return this._exceptionCriteria;
            }
            set
            {
                this._exceptionCriteria = value;
            }
        }

        [Property("MinEmpiricaScore")]
        public virtual int? MinEmpiricaScore
        {
            get
            {
                return this._minEmpiricaScore;
            }
            set
            {
                this._minEmpiricaScore = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "CreditCriteriaKey", ColumnType = "Int32")]
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

        [BelongsTo("CategoryKey", NotNull = true)]
        [ValidateNonEmpty("Category is a mandatory field")]
        public virtual Category_DAO Category
        {
            get
            {
                return this._category;
            }
            set
            {
                this._category = value;
            }
        }

        [BelongsTo("CreditMatrixKey", NotNull = true)]
        [ValidateNonEmpty("Credit Matrix is a mandatory field")]
        public virtual CreditMatrix_DAO CreditMatrix
        {
            get
            {
                return this._creditMatrix;
            }
            set
            {
                this._creditMatrix = value;
            }
        }

        [BelongsTo("EmploymentTypeKey", NotNull = true)]
        [ValidateNonEmpty("Employment Type is a mandatory field")]
        public virtual EmploymentType_DAO EmploymentType
        {
            get
            {
                return this._employmentType;
            }
            set
            {
                this._employmentType = value;
            }
        }

        [BelongsTo("MarginKey", NotNull = true)]
        [ValidateNonEmpty("Margin is a mandatory field")]
        public virtual Margin_DAO Margin
        {
            get
            {
                return this._margin;
            }
            set
            {
                this._margin = value;
            }
        }

        [BelongsTo("MortgageLoanPurposeKey", NotNull = true)]
        [ValidateNonEmpty("Mortgage Loan Purpose is a mandatory field")]
        public virtual MortgageLoanPurpose_DAO MortgageLoanPurpose
        {
            get
            {
                return this._mortgageLoanPurpose;
            }
            set
            {
                this._mortgageLoanPurpose = value;
            }
        }
    }
}