using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferExceptionType", Schema = "dbo", Lazy = true)]
    public partial class ApplicationExceptionType_DAO : DB_2AM<ApplicationExceptionType_DAO>
    {
        private string _description;

        private int _key;

        private ApplicationExceptionTypeGroup_DAO _applicationExceptionTypeGroup;

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

        [PrimaryKey(PrimaryKeyType.Native, "OfferExceptionTypeKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferExceptionTypeGroupKey", NotNull = true)]
        [ValidateNonEmpty("Application Exception Type Group is a mandatory field")]
        public virtual ApplicationExceptionTypeGroup_DAO ApplicationExceptionTypeGroup
        {
            get
            {
                return this._applicationExceptionTypeGroup;
            }
            set
            {
                this._applicationExceptionTypeGroup = value;
            }
        }
    }
}