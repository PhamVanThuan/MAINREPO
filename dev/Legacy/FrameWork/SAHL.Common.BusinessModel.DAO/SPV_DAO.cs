using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("SPV", Schema = "spv", Lazy = true)]
    public partial class SPV_DAO : DB_2AM<SPV_DAO>
    {
        private int _key;

        private SPV_DAO _parentSPV;

        private SPVCompany_DAO _SPVCompany;

        private string _description;

        private string _ReportDescription;

        private int? _fFResetConfigurationKey;

        private short _anniversaryDay;

        private int? _fundingWarehouse;

        private string _creditProviderNumber;

        private string _registrationNumber;

        private BankAccount_DAO _bankAccount;

        private GeneralStatus_DAO _generalStatus;

        private IList<SPV_DAO> _childSPVs;

        private IList<SPVMandate_DAO> _sPVMandates;

        private IList<SPVFee_DAO> _SPVFees;

        private IList<SPVAttribute_DAO> _SPVAttributes;

        private IList<SPVBalance_DAO> _sPVBalances;

        [PrimaryKey(PrimaryKeyType.Assigned, "SPVKey", ColumnType = "Int32", UnsavedValue = "-1")]
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

        [BelongsTo("ParentSPVKey", NotNull = false)]
        public virtual SPV_DAO ParentSPV
        {
            get
            {
                return this._parentSPV;
            }
            set
            {
                this._parentSPV = value;
            }
        }

        [BelongsTo("SPVCompanyKey", NotNull = false)]
        public virtual SPVCompany_DAO SPVCompany
        {
            get
            {
                return this._SPVCompany;
            }
            set
            {
                this._SPVCompany = value;
            }
        }

		[Lurker]
        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [Property("ReportDescription", ColumnType = "String")]
        public virtual string ReportDescription
        {
            get
            {
                return this._ReportDescription;
            }
            set
            {
                this._ReportDescription = value;
            }
        }

        [Property("ResetConfigurationKey")]
        public virtual int? ResetConfigurationKey
        {
            get
            {
                return this._fFResetConfigurationKey;
            }
            set
            {
                this._fFResetConfigurationKey = value;
            }
        }

        [Property("AnniversaryDay", ColumnType = "Int16", NotNull = true)]
        [ValidateNonEmpty("Anniversary Day is a mandatory field")]
        public virtual short AnniversaryDay
        {
            get
            {
                return this._anniversaryDay;
            }
            set
            {
                this._anniversaryDay = value;
            }
        }

        [Property("FundingWarehouse")]
        public virtual int? FundingWarehouse
        {
            get
            {
                return this._fundingWarehouse;
            }
            set
            {
                this._fundingWarehouse = value;
            }
        }

        [Property("CreditProviderNumber", ColumnType = "String")]
        public virtual string CreditProviderNumber
        {
            get
            {
                return this._creditProviderNumber;
            }
            set
            {
                this._creditProviderNumber = value;
            }
        }

        [Property("RegistrationNumber", ColumnType = "String")]
        public virtual string RegistrationNumber
        {
            get
            {
                return this._registrationNumber;
            }
            set
            {
                this._registrationNumber = value;
            }
        }

        [BelongsTo("BankAccountKey")]
        public virtual BankAccount_DAO BankAccount
        {
            get
            {
                return this._bankAccount;
            }
            set
            {
                this._bankAccount = value;
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

        [HasMany(typeof(SPV_DAO), ColumnKey = "ParentSPV", Table = "SPV", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<SPV_DAO> ChildSPVs
        {
            get
            {
                return this._childSPVs;
            }
            set
            {
                this._childSPVs = value;
            }
        }

        [HasMany(typeof(SPVMandate_DAO), ColumnKey = "SPVKey", Table = "SPVMandate", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<SPVMandate_DAO> SPVMandates
        {
            get
            {
                return this._sPVMandates;
            }
            set
            {
                this._sPVMandates = value;
            }
        }

        [HasMany(typeof(SPVFee_DAO), ColumnKey = "SPVKey", Table = "SPVFee", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<SPVFee_DAO> SPVFees
        {
            get
            {
                return this._SPVFees;
            }
            set
            {
                this._SPVFees = value;
            }
        }

        [HasMany(typeof(SPVAttribute_DAO), ColumnKey = "SPVKey", Table = "SPVAttribute", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<SPVAttribute_DAO> SPVAttributes
        {
            get
            {
                return this._SPVAttributes;
            }
            set
            {
                this._SPVAttributes = value;
            }
        }

        [HasMany(typeof(SPVBalance_DAO), ColumnKey = "SPVKey", Table = "SPVBalance", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<SPVBalance_DAO> SPVBalances
        {
            get
            {
                return this._sPVBalances;
            }
            set
            {
                this._sPVBalances = value;
            }
        }
    }
}