using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ReasonDefinition_DAO links the Reason Description and the Reason Type.
    /// </summary>
    /// <seealso cref="Reason_DAO"/>
    /// <seealso cref="ReasonDescription_DAO"/>
    /// <seealso cref="ReasonType_DAO"/>
    [ActiveRecord("ReasonDefinition", Schema = "dbo", Lazy = true)]
    public partial class ReasonDefinition_DAO : DB_2AM<ReasonDefinition_DAO>
    {
        private bool _allowComment;

        private int _key;

        private bool _enforceComment;

        private IList<OriginationSourceProduct_DAO> _originationSourceProducts;

        // commented, this is a lookup.
        //private IList<Reason_DAO> _reasons;

        private ReasonDescription_DAO _reasonDescription;

        private ReasonType_DAO _reasonType;

        private GeneralStatus_DAO _generalStatus;

        /// <summary>
        /// An indicator as to whether a comment can be stored against the reason.
        /// </summary>
        [Property("AllowComment", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Allow Comment is a mandatory field")]
        public virtual bool AllowComment
        {
            get
            {
                return this._allowComment;
            }
            set
            {
                this._allowComment = value;
            }
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native, "ReasonDefinitionKey", ColumnType = "Int32")]
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

        [Property("EnforceComment", ColumnType = "Boolean", NotNull = true)]
        public virtual bool EnforceComment
        {
            get
            {
                return this._enforceComment;
            }
            set
            {
                this._enforceComment = value;
            }
        }

        /// <summary>
        /// a list of OriginationSourceProducts this reasondefinition is applicable to.
        /// </summary>
        /// <seealso cref="OriginationSourceProduct_DAO"/>
        [HasAndBelongsToMany(typeof(OriginationSourceProduct_DAO), Table = "OriginationSourceProductReasonDefinition", ColumnKey = "ReasonDefinitionKey", ColumnRef = "OriginationSourceProductKey", Lazy = true)]
        public virtual IList<OriginationSourceProduct_DAO> OriginationSourceProducts
        {
            get
            {
                return this._originationSourceProducts;
            }
            set
            {
                this._originationSourceProducts = value;
            }
        }

        // commented, this is a lookup.
        //[HasMany(typeof(Reason_DAO), ColumnKey = "ReasonDefinitionKey", Table = "Reason")]
        //public virtual IList<Reason_DAO> Reasons
        //{
        //    get
        //    {
        //        return this._reasons;
        //    }
        //    set
        //    {
        //        this._reasons = value;
        //    }
        //}

        /// <summary>
        /// Each Reason Definition belongs to a Reason Description, this is the foreign key reference to the ReasonDescription table.
        /// </summary>
        [BelongsTo("ReasonDescriptionKey", NotNull = true)]
        [ValidateNonEmpty("Reason Description is a mandatory field")]
        public virtual ReasonDescription_DAO ReasonDescription
        {
            get
            {
                return this._reasonDescription;
            }
            set
            {
                this._reasonDescription = value;
            }
        }

        /// <summary>
        /// Each Reason Definition belongs to a Reason Type, this is the foreign key reference to the ReasonType table.
        /// </summary>
        [BelongsTo("ReasonTypeKey", NotNull = true)]
        [ValidateNonEmpty("Reason Type is a mandatory field")]
        public virtual ReasonType_DAO ReasonType
        {
            get
            {
                return this._reasonType;
            }
            set
            {
                this._reasonType = value;
            }
        }

        /// <summary>
        /// The foreign key reference to the GeneralStatus table.
        /// </summary>
        ///
        [BelongsTo("GeneralStatusKey", NotNull = true)]
        [ValidateNonEmpty("General Status is a mandatory field")]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }
    }
}