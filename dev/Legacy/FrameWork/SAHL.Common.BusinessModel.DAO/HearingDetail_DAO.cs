using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("HearingDetail", Schema = "debtcounselling")]
    public partial class HearingDetail_DAO : DB_2AM<HearingDetail_DAO>
    {
        private int _key;

        private DebtCounselling_DAO _debtCounselling;

        private HearingType_DAO _hearingType;

        private HearingAppearanceType_DAO _hearingAppearanceType;

        private Court_DAO _court;

        private string _caseNumber;

        private System.DateTime _hearingDate;

        private GeneralStatus_DAO _generalStatus;

        private string _comment;

        [PrimaryKey(PrimaryKeyType.Native, "HearingDetailKey", ColumnType = "Int32")]
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

        [Property("CaseNumber", ColumnType = "String", NotNull = true)]
        public virtual string CaseNumber
        {
            get
            {
                return this._caseNumber;
            }
            set
            {
                this._caseNumber = value;
            }
        }

        [Property("HearingDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Hearing Date is a mandatory field")]
        public virtual System.DateTime HearingDate
        {
            get
            {
                return this._hearingDate;
            }
            set
            {
                this._hearingDate = value;
            }
        }

        [BelongsTo("DebtCounsellingKey", NotNull = true)]
        public virtual DebtCounselling_DAO DebtCounselling
        {
            get
            {
                return this._debtCounselling;
            }
            set
            {
                this._debtCounselling = value;
            }
        }

        [BelongsTo("HearingTypeKey")]
        [ValidateNonEmpty("Hearing Type is a mandatory field")]
        public virtual HearingType_DAO HearingType
        {
            get
            {
                return this._hearingType;
            }
            set
            {
                this._hearingType = value;
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

        [BelongsTo("CourtKey")]
        public virtual Court_DAO Court
        {
            get
            {
                return this._court;
            }
            set
            {
                this._court = value;
            }
        }

        [BelongsTo("HearingAppearanceTypeKey")]
        [ValidateNonEmpty("Appearance Type is a mandatory field")]
        public virtual HearingAppearanceType_DAO HearingAppearanceType
        {
            get
            {
                return this._hearingAppearanceType;
            }
            set
            {
                this._hearingAppearanceType = value;
            }
        }

        [Property("Comment", ColumnType = "String")]
        public virtual string Comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                this._comment = value;
            }
        }
    }
}