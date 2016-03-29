using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    //[DoNotTestWithGenericTest]
    [ActiveRecord("CampaignTargetResponse", Schema = "dbo", Lazy = true)]
    public partial class CampaignTargetResponse_DAO : DB_2AM<CampaignTargetResponse_DAO>
    {
        private int _key;

        private string _description;

        private IList<CampaignTargetContact_DAO> _campaignTargetContacts;

        [PrimaryKey(PrimaryKeyType.Assigned, "CampaignTargetResponseKey", ColumnType = "Int32")]
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

        [Property("Description", ColumnType = "String", NotNull = true)]
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

        [HasMany(typeof(CampaignTargetContact_DAO), ColumnKey = "CampaignTargetResponseKey", Table = "CampaignTargetContact")]
        public virtual IList<CampaignTargetContact_DAO> CampaignTargetContacts
        {
            get
            {
                return this._campaignTargetContacts;
            }
            set
            {
                this._campaignTargetContacts = value;
            }
        }
    }
}