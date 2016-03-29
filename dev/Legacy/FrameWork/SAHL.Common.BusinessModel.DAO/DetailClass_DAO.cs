using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("DetailClass", Schema = "dbo", Lazy = true)]
    public partial class DetailClass_DAO : DB_2AM<DetailClass_DAO>
    {
        private string _description;

        private int _Key;

        private IList<DetailType_DAO> _detailTypes;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "DetailClassKey", ColumnType = "Int32")]
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

        [HasMany(typeof(DetailType_DAO), ColumnKey = "DetailClassKey", Table = "DetailType", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<DetailType_DAO> DetailTypes
        {
            get
            {
                return this._detailTypes;
            }
            set
            {
                this._detailTypes = value;
            }
        }
    }
}