using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using Castle.Components.Validator;

using SAHL.Common.Globals; using SAHL.Common.BusinessModel.DAO.Attributes; namespace SAHL.Common.BusinessModel.DAO
{
    [DoNotTestWithGenericTestAttribute()]
    [GenericTest(TestType.Find)][ActiveRecord("HeaderIconType",  Schema = "dbo")]
    public partial class HeaderIconType_DAO : DB_2AM<HeaderIconType_DAO>
    {

        private string _description;

        private string _icon;

        private string _statementName;

        private int _Key;
        // todo: Uncomment when HeaderIcon implemented
        //private IList<HeaderIcon> _headerIcons;

        private IList<HeaderIconDetails_DAO> _headerIconDetails;

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

        [Property("Icon", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Icon is a mandatory field")]
        public virtual string Icon
        {
            get
            {
                return this._icon;
            }
            set
            {
                this._icon = value;
            }
        }

        [Property("StatementName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Statement Name is a mandatory field")]
        public virtual string StatementName
        {
            get
            {
                return this._statementName;
            }
            set
            {
                this._statementName = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "HeaderIconTypeKey", ColumnType = "Int32")]
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

        // todo: Uncomment when HeaderIcon implemented
        //[HasMany(typeof(HeaderIcon), ColumnKey = "HeaderIconTypeKey", Table = "HeaderIcon")]
        //public virtual IList<HeaderIcon> HeaderIcons
        //{
        //    get
        //    {
        //        return this._headerIcons;
        //    }
        //    set
        //    {
        //        this._headerIcons = value;
        //    }
        //}

        [HasMany(typeof(HeaderIconDetails_DAO), ColumnKey = "HeaderIconTypeKey", Table = "HeaderIconDetails")]
        public virtual IList<HeaderIconDetails_DAO> HeaderIconDetails
        {
            get
            {
                return this._headerIconDetails;
            }
            set
            {
                this._headerIconDetails = value;
            }
        }
    }
}
