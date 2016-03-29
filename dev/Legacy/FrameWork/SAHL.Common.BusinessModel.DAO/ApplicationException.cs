using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferException", Schema = "dbo")]
    public partial class ApplicationException_DAO : DB_2AM<ApplicationException_DAO>
    {

        private Application_DAO _application;

        private bool _overRidden;

        private int _key;

        private ApplicationExceptionType_DAO _applicationExceptionType;

        [BelongsTo("OfferKey", NotNull=true)]
        public virtual Application_DAO Application
        {
            get
            {
                return this._application;
            }
            set
            {
                this._application = value;
            }
        }

        [Property("OverRidden", ColumnType = "Boolean")]
        public virtual bool OverRidden
        {
            get
            {
                return this._overRidden;
            }
            set
            {
                this._overRidden = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "OfferExceptionKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferExceptionTypeKey", NotNull = true)]
        public virtual ApplicationExceptionType_DAO ApplicationExceptionType
        {
            get
            {
                return this._applicationExceptionType;
            }
            set
            {
                this._applicationExceptionType = value;
            }
        }
    }
    
}
