using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("ReportGroup", Schema = "dbo")]
    public partial class ReportGroup_DAO : DB_2AM<ReportGroup_DAO>
    {
        private string _description;

        private int _key;

        // commented, this is a lookup.
        //private IList<ReportStatement> _reportStatements;

        private Feature_DAO _feature;

        private IList<ReportStatement_DAO> _reportStatements;

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

        [PrimaryKey(PrimaryKeyType.Assigned, "ReportGroupKey", ColumnType = "Int32")]
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

        [HasMany(typeof(ReportStatement_DAO), ColumnKey = "ReportGroupKey", Table = "ReportStatement", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<ReportStatement_DAO> ReportStatements
        {
            get
            {
                return this._reportStatements;
            }
            set
            {
                this._reportStatements = value;
            }
        }

        [BelongsTo("FeatureKey", NotNull = true)]
        [ValidateNonEmpty("Feature is a mandatory field")]
        public virtual Feature_DAO Feature
        {
            get
            {
                return this._feature;
            }
            set
            {
                this._feature = value;
            }
        }
    }
}