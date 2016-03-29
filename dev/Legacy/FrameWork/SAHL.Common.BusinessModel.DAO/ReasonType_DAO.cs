using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// ReasonType_DAO is used to assign each reason a type. It is also specifies the Generic Key type which applies to the Reason.
    /// </summary>
    /// <seealso cref="Reason_DAO"/>
    /// <seealso cref="ReasonDescription_DAO"/>
    /// <seealso cref="ReasonTypeGroup_DAO"/>
    /// <seealso cref="ReasonDefinition_DAO"/>
    /// <seealso cref="GenericKeyType_DAO"/>
    [GenericTest(TestType.Find)]
    [ActiveRecord("ReasonType", Schema = "dbo")]
    public partial class ReasonType_DAO : DB_2AM<ReasonType_DAO>
    {
        private string _description;

        private GenericKeyType_DAO _genericKeyType;

        private int _key;

        // private IList<ReasonDefinition_DAO> _reasonDefinitions;

        private ReasonTypeGroup_DAO _reasonTypeGroup;

        /// <summary>
        /// The Description of the Reason Type.
        /// </summary>
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

        /// <summary>
        /// Each Reason Type (and the Reasons associated to the Reason Type) belong to a specific GenericKeyType. For example
        /// a Reason Type of 'Credit Decline Loan' would be linked to an OfferInformationKey via this GenericKeyType link.
        /// </summary>
        [BelongsTo("GenericKeyTypeKey", NotNull = true)]
        [ValidateNonEmpty("Generic Key Type is a mandatory field")]
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

        /// <summary>
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "ReasonTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(ReasonDefinition_DAO), ColumnKey = "ReasonTypeKey", Table = "ReasonDefinition")]
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
        /// Each of the ReasonTypes belong to a ReasonTypeGroup. This is the foreign key reference to the ReasonTypeGroup table.
        /// </summary>
        [BelongsTo("ReasonTypeGroupKey", NotNull = true)]
        [ValidateNonEmpty("Reason Type Group is a mandatory field")]
        public virtual ReasonTypeGroup_DAO ReasonTypeGroup
        {
            get
            {
                return this._reasonTypeGroup;
            }
            set
            {
                this._reasonTypeGroup = value;
            }
        }
    }
}