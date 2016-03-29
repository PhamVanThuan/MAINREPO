using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferCampaign", Schema = "dbo", Lazy = true)]
    public partial class ApplicationCampaign_DAO : DB_2AM<ApplicationCampaign_DAO>
    {
        private string _description;

        private System.DateTime _startDate;

        private int _key;

        //private IList<Application_DAO> _applications;

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

        [Property("StartDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Start Date is a mandatory field")]
        public virtual System.DateTime StartDate
        {
            get
            {
                return this._startDate;
            }
            set
            {
                this._startDate = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OfferCampaignKey", ColumnType = "Int32")]
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

        // commented, this will be a performance hogg.
        //[HasMany(typeof(Application_DAO), ColumnKey = "OfferCampaignKey", Table = "Offer", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        //public virtual IList<Application_DAO> Applications
        //{
        //    get
        //    {
        //        return this._applications;
        //    }
        //    set
        //    {
        //        this._applications = value;
        //    }
        //}
    }
}