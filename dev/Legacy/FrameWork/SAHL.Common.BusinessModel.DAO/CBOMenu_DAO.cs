using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("CoreBusinessObjectMenu", Schema = "dbo", Lazy = false)]
    public partial class CBOMenu_DAO : DB_2AM<CBOMenu_DAO>
    {
        #region Private Attributes

        private string _description;

        private char _nodeType;

        private string _uRL;

        private string _statementNameKey;

        private int _sequence;

        private string _menuIcon;

        private Feature_DAO _feature;

        private bool _hasOriginationSource;

        private bool _isRemovable;

        private int _expandLevel;

        private bool _includeParentHeaderIcons;

        private int _key;

        private GenericKeyType_DAO _genericKeyType;

        private IList<CBOMenu_DAO> _childMenus;

        private IList<ContextMenu_DAO> _contextMenus;

        private IList<InputGenericType_DAO> _inputGenericTypes;

        private CBOMenu_DAO _parentMenu;

        #endregion Private Attributes

        #region Properties

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

        [Property("NodeType", ColumnType = "Char")]
        public virtual char NodeType
        {
            get
            {
                return this._nodeType;
            }
            set
            {
                this._nodeType = value;
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

        [Property("StatementNameKey", ColumnType = "String")]
        public virtual string StatementNameKey
        {
            get
            {
                return this._statementNameKey;
            }
            set
            {
                this._statementNameKey = value;
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

        [Property("MenuIcon", ColumnType = "String")]
        public virtual string MenuIcon
        {
            get
            {
                return this._menuIcon;
            }
            set
            {
                this._menuIcon = value;
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

        [BelongsTo(Column = "GenericKeyTypeKey", NotNull = false)]
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

        [Property("HasOriginationSource", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Has Origination Source is a mandatory field")]
        public virtual bool HasOriginationSource
        {
            get
            {
                return this._hasOriginationSource;
            }
            set
            {
                this._hasOriginationSource = value;
            }
        }

        [Property("IsRemovable", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Is Removable is a mandatory field")]
        public virtual bool IsRemovable
        {
            get
            {
                return this._isRemovable;
            }
            set
            {
                this._isRemovable = value;
            }
        }

        [Property("ExpandLevel", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Expand Level is a mandatory field")]
        public virtual int ExpandLevel
        {
            get
            {
                return this._expandLevel;
            }
            set
            {
                this._expandLevel = value;
            }
        }

        [Property("IncludeParentHeaderIcons", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Include Parent Header Icons is a mandatory field")]
        public virtual bool IncludeParentHeaderIcons
        {
            get
            {
                return this._includeParentHeaderIcons;
            }
            set
            {
                this._includeParentHeaderIcons = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "CoreBusinessObjectKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(CBOMenu_DAO), ColumnKey = "ParentKey", Table = "CoreBusinessObjectMenu", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, OrderBy = "Sequence", Lazy = false)]
        [Lurker]
        [HasMany(typeof(CBOMenu_DAO), ColumnKey = "ParentKey", Table = "CoreBusinessObjectMenu", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true, Lazy = false)]
        public virtual IList<CBOMenu_DAO> ChildMenus
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
        public virtual CBOMenu_DAO ParentMenu
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

        //[HasMany(typeof(ContextMenu_DAO), ColumnKey = "CoreBusinessObjectKey", Table = "contextmenu", Inverse = true, OrderBy = "Sequence", Lazy = false)]
        [Lurker]
        [HasMany(typeof(ContextMenu_DAO), ColumnKey = "CoreBusinessObjectKey", Table = "contextmenu", Inverse = true, Lazy = false)]
        public virtual IList<ContextMenu_DAO> ContextMenus
        {
            get
            {
                return this._contextMenus;
            }
            set
            {
                this._contextMenus = value;
            }
        }

        [HasMany(typeof(InputGenericType_DAO), ColumnKey = "CoreBusinessObjectKey", Table = "InputGenericType", Inverse = true, Lazy = false)]
        public virtual IList<InputGenericType_DAO> InputGenericTypes
        {
            get
            {
                return this._inputGenericTypes;
            }
            set
            {
                this._inputGenericTypes = value;
            }
        }

        #endregion Properties

        #region Methods

        public static object GetEntity(CBOMenu_DAO Menu)
        {
            return null;
        }

        #endregion Methods
    }
}