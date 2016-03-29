using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ReasonDescription_DAO contains the text versions of the Reasons. It also links the Reason to a Translatable Item
    /// from which a translated version of the reason can be taken.
    /// </summary>
    /// <seealso cref="Reason_DAO"/>
    /// <seealso cref="ReasonDefinition_DAO"/>
    /// <seealso cref="ReasonType_DAO"/>
    [GenericTest(TestType.Find)]
    [ActiveRecord("ReasonDescription", Schema = "dbo")]
    public partial class ReasonDescription_DAO : DB_2AM<ReasonDescription_DAO>
    {
        private string _description;

        private int _key;

        // commented, this is a lookup.
        //private IList<ReasonDefinition> _reasonDefinitions;

        private TranslatableItem_DAO _translatableItem;

        /// <summary>
        /// The text description of the Reason
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
        [PrimaryKey(PrimaryKeyType.Assigned, "ReasonDescriptionKey", ColumnType = "Int32")]
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

        // commented, this is a lookup.
        //[HasMany(typeof(ReasonDefinition), ColumnKey = "ReasonDescriptionKey", Table = "ReasonDefinition")]
        //public virtual IList<ReasonDefinition_DAO> ReasonDefinitions
        //{
        //    get
        //    {
        //        return this._reasonDefinitions;
        //    }
        //    set
        //    {
        //        this._reasonDefinitions = value;
        //    }
        //}
        /// <summary>
        /// Each Reason belongs to a Translatable Item in the TranslatableItem table. This is the foreign key reference to the
        /// TranslatableItem table.
        /// </summary>
        [BelongsTo("TranslatableItemKey", NotNull = false)]
        public virtual TranslatableItem_DAO TranslatableItem
        {
            get
            {
                return this._translatableItem;
            }
            set
            {
                this._translatableItem = value;
            }
        }
    }
}