using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// LegalEntityRelationshipType_DAO is used to store the different relationship types that can exist between Legal Entities.
    /// </summary>
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("LegalEntityRelationshipType", Schema = "dbo", Lazy = true)]
    public partial class LegalEntityRelationshipType_DAO : DB_2AM<LegalEntityRelationshipType_DAO>
    {
        private string _description;

        private int _key;

        //private IList<LegalEntityRelationship_DAO> _legalEntityRelationships;
        /// <summary>
        /// The description of the relationship. e.g. Spouse/Partner
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
        [PrimaryKey(PrimaryKeyType.Assigned, "RelationshipTypeKey", ColumnType = "Int32")]
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

        //[HasMany(typeof(LegalEntityRelationship_DAO), ColumnKey = "RelationshipTypeKey", Table = "LegalEntityRelationship", Lazy = true)]
        //public virtual IList<LegalEntityRelationship_DAO> LegalEntityRelationships
        //{
        //    get
        //    {
        //        return this._legalEntityRelationships;
        //    }
        //    set
        //    {
        //        this._legalEntityRelationships = value;
        //    }
        //}
    }
}