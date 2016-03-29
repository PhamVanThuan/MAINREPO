using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// LegalEntityStatus_DAO is used to store the different statuses that a Legal Entity can be in.
    /// </summary>
    [GenericTest(TestType.Find)]
    [ActiveRecord("LegalEntityStatus", Schema = "dbo", Lazy = true)]
    public partial class LegalEntityStatus_DAO : DB_2AM<LegalEntityStatus_DAO>
    {
        private string _description;

        private int _key;

        //private IList<LegalEntity_DAO> _legalEntities;
        /// <summary>
        /// The description of the Legal Entity Status. e.g. Alive/Deceased/Disabled
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
        /// Primary Key
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Assigned, "LegalEntityStatusKey", ColumnType = "Int32")]
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

        // Commented, this is a lookup.
        //[HasMany(typeof(LegalEntity_DAO), ColumnKey = "LegalEntityStatusKey", Table = "LegalEntity", Lazy=true)]
        //public virtual IList<LegalEntity_DAO> LegalEntities
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