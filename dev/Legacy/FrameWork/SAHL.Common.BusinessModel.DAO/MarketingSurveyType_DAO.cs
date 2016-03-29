using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferMarketingSurveyType", Schema = "dbo")]
    public partial class ApplicationMarketingSurveyType_DAO : DB_2AM<ApplicationMarketingSurveyType_DAO>
    {
        private string _description;

        private int _key;

        private ApplicationMarketingSurveyTypeGroup_DAO _applicationMarketingSurveyTypeGroup;

        // commented, this is a lookup.
        //private IList<Offer> _applicationMarketingSurveyTypes;

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

        [PrimaryKey(PrimaryKeyType.Native, "OfferMarketingSurveyTypeKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferMarketingSurveyTypeGroupKey", NotNull = true)]
        [ValidateNonEmpty("Application Marketing Survey Type Group is a mandatory field")]
        public virtual ApplicationMarketingSurveyTypeGroup_DAO ApplicationMarketingSurveyTypeGroup
        {
            get
            {
                return this._applicationMarketingSurveyTypeGroup;
            }
            set
            {
                this._applicationMarketingSurveyTypeGroup = value;
            }
        }

        // commented, this is a lookup.
        //[HasAndBelongsToMany(typeof(Offer), ColumnRef = "OfferKey", ColumnKey = "OfferMarketingSurveyTypeKey", Schema = "dbo", Table = "OfferMarketingSurvey")]
        //public virtual IList<Offer> ApplicationMarketingSurveyTypes
        //{
        //    get
        //    {
        //        return this._applicationMarketingSurveyTypes;
        //    }
        //    set
        //    {
        //        this._applicationMarketingSurveyTypes = value;
        //    }
        //}
    }
}