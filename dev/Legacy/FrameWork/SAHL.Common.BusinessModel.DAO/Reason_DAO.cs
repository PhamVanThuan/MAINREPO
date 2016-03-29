using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Reason is a link between a ReasonDefinition and an generic object in the system. It is used to supply a reason for an action that is performed on that object.
    /// </summary>
    /// <seealso cref="ReasonDefinition_DAO"/>
    /// <seealso cref="GenericKeyType_DAO"/>
    [ActiveRecord("Reason", Schema = "dbo")]
    public partial class Reason_DAO : DB_2AM<Reason_DAO>
    {
        private int _genericKey;

        private string _comment;

        private int _key;

        private ReasonDefinition_DAO _reasonDefinition;

        private StageTransition_DAO _stageTransition;

        /// <summary>
        /// The Primary Key value of the GenericKey which the Reason is attached to.
        /// </summary>
        [Property("GenericKey", ColumnType = "Int32")]
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

        /// <summary>
        /// A comment for the reason.
        /// </summary>
        [Property("Comment", ColumnType = "String")]
        public virtual string Comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                this._comment = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "ReasonKey", ColumnType = "Int32")]
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
        /// This is the foreign key reference to the ReasonDefinition table. Each Reason has a Definition which is stored in
        /// the ReasonDefinition table.
        /// </summary>
        [BelongsTo("ReasonDefinitionKey", NotNull = true)]
        [ValidateNonEmpty("Reason Definition is a mandatory field")]
        public virtual ReasonDefinition_DAO ReasonDefinition
        {
            get
            {
                return this._reasonDefinition;
            }
            set
            {
                this._reasonDefinition = value;
            }
        }

        [BelongsTo("StageTransitionKey", NotNull = false)]
        public virtual StageTransition_DAO StageTransition
        {
            get
            {
                return this._stageTransition;
            }
            set
            {
                this._stageTransition = value;
            }
        }
    }
}