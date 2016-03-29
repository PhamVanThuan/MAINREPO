using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferMarketingSurveyTypeGroup", Schema = "dbo", Lazy = true)]
    public partial class ApplicationMarketingSurveyTypeGroup_DAO : DB_2AM<ApplicationMarketingSurveyTypeGroup_DAO>
    {
        private string _description;

        private int _key;

        private IList<ApplicationMarketingSurveyType_DAO> _applicationMarketingSurveyTypes;

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

        [PrimaryKey(PrimaryKeyType.Native, "OfferMarketingSurveyTypeGroupKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ApplicationMarketingSurveyType_DAO), ColumnKey = "OfferMarketingSurveyTypeGroupKey", Table = "OfferMarketingSurveyType", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ApplicationMarketingSurveyType_DAO> ApplicationMarketingSurveyTypes
        {
            get
            {
                return this._applicationMarketingSurveyTypes;
            }
            set
            {
                this._applicationMarketingSurveyTypes = value;
            }
        }
    }
}