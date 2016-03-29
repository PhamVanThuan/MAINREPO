using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferExceptionTypeGroup", Schema = "dbo", Lazy = true)]
    public partial class ApplicationExceptionTypeGroup_DAO : DB_2AM<ApplicationExceptionTypeGroup_DAO>
    {
        private string _description;

        private int _key;

        private IList<ApplicationExceptionType_DAO> _applicationExceptionTypes;

        [Property("Description", ColumnType = "String")]
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

        [PrimaryKey(PrimaryKeyType.Native, "OfferExceptionTypeGroupKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ApplicationExceptionType_DAO), ColumnKey = "OfferExceptionTypeGroupKey", Table = "OfferExceptionType")]
        public virtual IList<ApplicationExceptionType_DAO> ApplicationExceptionTypes
        {
            get
            {
                return this._applicationExceptionTypes;
            }
            set
            {
                this._applicationExceptionTypes = value;
            }
        }
    }
}