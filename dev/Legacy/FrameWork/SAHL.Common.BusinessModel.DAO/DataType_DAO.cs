using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("DataType", Schema = "dbo")]
    public partial class DataType_DAO : DB_2AM<DataType_DAO>
    {
        private string _description;

        private int _Key;

        //private IList<OrganisationStructureAttributeType_DAO> _organisationStructureAttributeTypes;
        [Property("Description", NotNull = true)]
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

        [PrimaryKey(PrimaryKeyType.Native, "DataTypeKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        // commented, link from this direction seems to be not required.
        //[HasMany(typeof(OrganisationStructureAttributeType_DAO), ColumnKey = "DataTypeKey", Table = "OrganisationStructureAttributeType")]
        //public virtual IList<OrganisationStructureAttributeType_DAO> OrganisationStructureAttributeTypes
        //{
        //    get
        //    {
        //        return this._organisationStructureAttributeTypes;
        //    }
        //    set
        //    {
        //        this._organisationStructureAttributeTypes = value;
        //    }
        //}
    }
}