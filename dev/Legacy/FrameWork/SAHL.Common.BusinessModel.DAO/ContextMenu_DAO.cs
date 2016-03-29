using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("ContextMenu", Schema = "dbo", Lazy = false)]
    public partial class ContextMenu_DAO : DB_2AM<ContextMenu_DAO>
    {
        private CBOMenu_DAO _cboMenu;

        private string _description;

        private string _uRL;

        private Feature_DAO _feature;

        private int _sequence;

        private int _key;

        private IList<ContextMenu_DAO> _childMenus;

        private ContextMenu_DAO _parentMenu;

        [BelongsTo(Column = "CoreBusinessObjectKey")]
        public virtual CBOMenu_DAO CBOMenu
        {
            get
            {
                return this._cboMenu;
            }
            set
            {
                this._cboMenu = value;
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

        [Property("URL", ColumnType = "String")]
        public virtual string URL
        {
            get
            {
                return this._uRL;
            }
            set
            {
                this._uRL = value;
            }
        }

        [BelongsTo(Column = "FeatureKey", NotNull = true)]
        [ValidateNonEmpty("Feature is a mandatory field")]
        public virtual Feature_DAO Feature
        {
            get
            {
                return this._feature;
            }
            set
            {
                this._feature = value;
            }
        }

        [Property("Sequence", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Sequence is a mandatory field")]
        public virtual int Sequence
        {
            get
            {
                return this._sequence;
            }
            set
            {
                this._sequence = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "ContextKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ContextMenu_DAO), ColumnKey = "ParentKey", Table = "ContextMenu", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = false)]
        public virtual IList<ContextMenu_DAO> ChildMenus
        {
            get
            {
                return this._childMenus;
            }
            set
            {
                this._childMenus = value;
            }
        }

        [BelongsTo("ParentKey", NotNull = false)]
        public virtual ContextMenu_DAO ParentMenu
        {
            get
            {
                return this._parentMenu;
            }
            set
            {
                this._parentMenu = value;
            }
        }
    }
}