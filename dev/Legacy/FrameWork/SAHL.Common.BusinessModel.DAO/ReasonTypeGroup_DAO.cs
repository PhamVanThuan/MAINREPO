using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ReasonTypeGroup_DAO is used to group the various Reason Types together. It also contains a ParentKey which allows
    /// a hierarchy of ReasonTypeGroups to be built up.
    /// </summary>
    /// <seealso cref="Reason_DAO"/>
    /// <seealso cref="ReasonDescription_DAO"/>
    /// <seealso cref="ReasonDefinition_DAO"/>
    /// <seealso cref="ReasonType_DAO"/>
    [GenericTest(TestType.Find)]
    [ActiveRecord("ReasonTypeGroup", Schema = "dbo")]
    public partial class ReasonTypeGroup_DAO : DB_2AM<ReasonTypeGroup_DAO>
    {
        private string _description;

        private int _key;

        private IList<ReasonType_DAO> _reasonTypes;

        private IList<ReasonTypeGroup_DAO> _children;

        private ReasonTypeGroup_DAO _parent;

        /// <summary>
        /// The description of the ReasonTypeGroup.
        /// </summary>
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

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "ReasonTypeGroupKey", ColumnType = "Int32")]
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

        /// <summary>
        /// There is a one-to-many relationship between ReasonTypeGroups and ReasonTypes. A single ReasonTypeGroup has many ReasonTypes
        /// which form part of the group.
        /// </summary>
        [HasMany(typeof(ReasonType_DAO), ColumnKey = "ReasonTypeGroupKey", Table = "ReasonType", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ReasonType_DAO> ReasonTypes
        {
            get
            {
                return this._reasonTypes;
            }
            set
            {
                this._reasonTypes = value;
            }
        }

        /// <summary>
        /// This property will retrieve the children ReasonTypeGroups. e.g. Credit Decline Income and Credit Decline Profile Reason Type
        /// groups could all belong to a parent Reason Type group of <strong>Credit</strong>. This property could consist of many
        /// Reason Type groups.
        /// </summary>
        [HasMany(typeof(ReasonTypeGroup_DAO), ColumnKey = "ParentKey", Table = "ReasonTypeGroup", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ReasonTypeGroup_DAO> Children
        {
            get
            {
                return this._children;
            }
            set
            {
                this._children = value;
            }
        }

        /// <summary>
        /// This property is the ReasonTypeGroupKey which is serving as the Parent group. This property would only be a single
        /// Reason type group.
        /// </summary>
        [BelongsTo("ParentKey", NotNull = false)]
        public virtual ReasonTypeGroup_DAO Parent
        {
            get
            {
                return this._parent;
            }
            set
            {
                this._parent = value;
            }
        }
    }
}