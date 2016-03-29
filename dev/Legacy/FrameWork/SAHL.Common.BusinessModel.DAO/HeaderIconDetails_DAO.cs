using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using Castle.Components.Validator;

using SAHL.Common.Globals; using SAHL.Common.BusinessModel.DAO.Attributes; namespace SAHL.Common.BusinessModel.DAO
{
    [DoNotTestWithGenericTestAttribute()]
    [GenericTest(TestType.Find)][ActiveRecord("HeaderIconDetails",  Schema = "dbo")]
    public partial class HeaderIconDetails_DAO : DB_2AM<HeaderIconDetails_DAO>
    {

        private int _genericKey;

        private string _description;

        private int _Key;

        private int _genericKeyTypeKey;

        // todo: Uncomment when HeaderIconType implemented
        //private HeaderIconType _headerIconType;

        [Property("GenericKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Generic Key is a mandatory field")]
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

        [PrimaryKey(PrimaryKeyType.Native, "HeaderIconDetailsKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        [Property("GenericKeyTypeKey", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Generic Key Type Key is a mandatory field")]
        public virtual int GenericKeyTypeKey
        {
            get
            {
                return this._genericKeyTypeKey;
            }
            set
            {
                this._genericKeyTypeKey = value;
            }
        }

        // todo: Uncomment when HeaderIconType implemented
        //[BelongsTo("HeaderIconTypeKey", NotNull = false)]
        //public virtual HeaderIconType HeaderIconType
        //{
        //    get
        //    {
        //        return this._headerIconType;
        //    }
        //    set
        //    {
        //        this._headerIconType = value;
        //    }
        //}
    }
}
