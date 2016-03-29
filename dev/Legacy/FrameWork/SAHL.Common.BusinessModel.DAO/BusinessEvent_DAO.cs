using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("BusinessEvent", Schema = "Survey", Lazy = true)]
    public partial class BusinessEvent_DAO : DB_2AM<BusinessEvent_DAO>
    {
        private int _key;

        private int _genericKey;

        private GenericKeyType_DAO _genericKeyType;

        private string _description;

        private IList<BusinessEventQuestionnaire_DAO> _businessEventQuestionnaires;

        [PrimaryKey(PrimaryKeyType.Native, "BusinessEventKey", ColumnType = "Int32")]
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

        [Property("GenericKey", ColumnType = "Int32", NotNull = true)]
        public virtual int GenericKey
        {
            get
            {
                return this._genericKey;
            }
            set
            {
                this._genericKey = value;
            }
        }

        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        public virtual GenericKeyType_DAO GenericKeyType
        {
            get
            {
                return this._genericKeyType;
            }
            set
            {
                this._genericKeyType = value;
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

        [HasMany(typeof(BusinessEventQuestionnaire_DAO), ColumnKey = "BusinessEventKey", Table = "BusinessEventQuestionnaire")]
        public virtual IList<BusinessEventQuestionnaire_DAO> BusinessEventQuestionnaires
        {
            get
            {
                return this._businessEventQuestionnaires;
            }
            set
            {
                this._businessEventQuestionnaires = value;
            }
        }
    }
}