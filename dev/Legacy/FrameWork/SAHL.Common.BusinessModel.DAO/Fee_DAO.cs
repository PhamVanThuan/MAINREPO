using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("Fee", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class Fee_DAO : DB_2AM<Fee_DAO>
    {
        private int _key;

        private double _amount;

        private DateTime? _lastPostedDate;

        private FinancialService_DAO _financialService;

        private GeneralStatus_DAO _generalStatus;

        private FeeType_DAO _feeType;

        [PrimaryKey(PrimaryKeyType.Native, "FeeKey", ColumnType = "Int32")]
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

        [Property("Amount", ColumnType = "Double", NotNull = true)]
        public virtual double Amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
            }
        }

        [Property("LastPostedDate", ColumnType = "Timestamp")]
        public virtual DateTime? LastPostedDate
        {
            get
            {
                return this._lastPostedDate;
            }
            set
            {
                this._lastPostedDate = value;
            }
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

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
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

        [BelongsTo("FeeTypeKey", NotNull = true)]
        [ValidateNonEmpty("Fee Type is a mandatory field")]
        public virtual FeeType_DAO FeeType
        {
            get
            {
                return this._feeType;
            }
            set
            {
                this._feeType = value;
            }
        }
    }
}