using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferAttribute", Schema = "dbo")]
    public partial class ApplicationAttribute_DAO : DB_2AM<ApplicationAttribute_DAO>
    {

        private int _applicationKey;

        private int _key;

        private ApplicationAttributeType_DAO _applicationAttributeType;

        [Property("OfferKey", ColumnType = "Int32", NotNull = true)]
        public virtual int ApplicationKey
        {
            get
            {
                return this._applicationKey;
            }
            set
            {
                this._applicationKey = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OfferAttributeKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferAttributeTypeKey", NotNull = true)]
        public virtual ApplicationAttributeType_DAO ApplicationAttributeType
        {
            get
            {
                return this._applicationAttributeType;
            }
            set
            {
                this._applicationAttributeType = value;
            }
        }
    }
    
}
