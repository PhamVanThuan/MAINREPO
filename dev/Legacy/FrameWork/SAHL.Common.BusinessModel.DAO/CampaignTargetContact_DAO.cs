using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("CampaignTargetContact", Schema = "dbo", Lazy = true)]
    public partial class CampaignTargetContact_DAO : DB_2AM<CampaignTargetContact_DAO>
    {
        private int _key;

        private int _legalEntityKey;

        private System.DateTime _changeDate;

        private int _adUserKey;

        private CampaignTarget_DAO _campaignTarget;

        private CampaignTargetResponse_DAO _campaignTargetResponse;

        [PrimaryKey(PrimaryKeyType.Native, "CampaignTargetContactKey", ColumnType = "Int32")]
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

        [Property("LegalEntityKey", ColumnType = "Int32", NotNull = true)]
        public virtual int LegalEntityKey
        {
            get
            {
                return this._legalEntityKey;
            }
            set
            {
                this._legalEntityKey = value;
            }
        }

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

        [Property("AdUserKey", ColumnType = "Int32", NotNull = true)]
        public virtual int AdUserKey
        {
            get
            {
                return this._adUserKey;
            }
            set
            {
                this._adUserKey = value;
            }
        }

        [BelongsTo("CampaignTargetKey", NotNull = true)]
        public virtual CampaignTarget_DAO CampaignTarget
        {
            get
            {
                return this._campaignTarget;
            }
            set
            {
                this._campaignTarget = value;
            }
        }

        [BelongsTo("CampaignTargetResponseKey", NotNull = true)]
        public virtual CampaignTargetResponse_DAO CampaignTargetResponse
        {
            get
            {
                return this._campaignTargetResponse;
            }
            set
            {
                this._campaignTargetResponse = value;
            }
        }
    }
}