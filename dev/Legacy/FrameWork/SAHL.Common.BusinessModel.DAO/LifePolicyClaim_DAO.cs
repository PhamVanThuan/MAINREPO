using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("LifePolicyClaim", Schema = "dbo", Lazy = true)]
    [ConstructorInjector]
    public partial class LifePolicyClaim_DAO : DB_2AM<LifePolicyClaim_DAO>
    {
        private int _key;

        private FinancialService_DAO _financialService;

        private ClaimStatus_DAO _claimStatus;

        private ClaimType_DAO _claimType;

        private System.DateTime _claimDate;

        /// <summary>
        /// Used for Activerecord exclusively, please use Key.
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "LifePolicyClaimKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get { return _key; }
            set { _key = value; }
        }

        [BelongsTo("FinancialServiceKey", NotNull = true)]
        [ValidateNonEmpty("Financial Service is a mandatory field")]
        public virtual FinancialService_DAO FinancialService
        {
            get
            {
                return this._financialService;
            }
            set
            {
                this._financialService = value;
            }
        }

        [BelongsTo("ClaimStatusKey", NotNull = true)]
        [ValidateNonEmpty("Claim Status is a mandatory field")]
        public virtual ClaimStatus_DAO ClaimStatus
        {
            get
            {
                return this._claimStatus;
            }
            set
            {
                this._claimStatus = value;
            }
        }

        [BelongsTo("ClaimTypeKey", NotNull = true)]
        [ValidateNonEmpty("Claim Type is a mandatory field")]
        public virtual ClaimType_DAO ClaimType
        {
            get
            {
                return this._claimType;
            }
            set
            {
                this._claimType = value;
            }
        }

        [Property("ClaimDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Claim Date is a mandatory field")]
        public virtual System.DateTime ClaimDate
        {
            get
            {
                return this._claimDate;
            }
            set
            {
                this._claimDate = value;
            }
        }
    }
}