using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// LegalEntityType_DAO is used in order to store the different Legal Entity types that exist. The LegalEntityType forms
    /// the basis for the Legal Entity discrimination.
    /// </summary>
    [GenericTest(TestType.Find)]
    [ActiveRecord("LegalEntityType", Schema = "dbo", Lazy = true)]
    public partial class LegalEntityType_DAO : DB_2AM<LegalEntityType_DAO>
    {
        private string _description;

        private int _legalEntityTypeKey;

        //        private IList<LegalEntity> _legalEntities;
        /// <summary>
        /// The Legal Entity Type Description. e.g. Natural Person/Trust
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
        [PrimaryKey(PrimaryKeyType.Assigned, "LegalEntityTypeKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._legalEntityTypeKey;
            }
            set
            {
                this._legalEntityTypeKey = value;
            }
        }

        //[HasMany(typeof(LegalEntity), ColumnKey = "LegalEntityTypeKey", Table = "LegalEntity")]
        //public virtual IList<LegalEntity> LegalEntities
        //{
        //    get
        //    {
        //        return this._legalEntities;
        //    }
        //    set
        //    {
        //        this._legalEntities = value;
        //    }
        //}
    }
}