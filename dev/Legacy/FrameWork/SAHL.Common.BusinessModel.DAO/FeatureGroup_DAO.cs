using System;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("FeatureGroup", Schema = "dbo", Lazy = false)]
    public partial class FeatureGroup_DAO : DB_2AM<FeatureGroup_DAO>
    {
        #region Private Attributes

        private string _aDUserGroup;

        private Feature_DAO _feature;

        private int _key;

        #endregion Private Attributes

        #region Properties

        [Property("ADUserGroup", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("AD User Group is a mandatory field")]
        public virtual string ADUserGroup
        {
            get
            {
                return this._aDUserGroup;
            }
            set
            {
                this._aDUserGroup = value;
            }
        }

        [BelongsTo(Column = "FeatureKey", NotNull = true)]
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

        [PrimaryKey(PrimaryKeyType.Native, "FeatureGroupKey", ColumnType = "Int32")]
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

        #endregion Properties

        #region Methods

        /// <summary>
        /// Returns a distinct list of groups in the FeatureGroup table.
        /// </summary>
        public static String[] FindAllGroups()
        {
            SimpleQuery q = new SimpleQuery(typeof(Feature_DAO), typeof(String), @"
                SELECT DISTINCT fg.ADUserGroup from FeatureGroup_DAO fg
                ");
            return (String[])FeatureGroup_DAO.ExecuteQuery(q);
        }

        #endregion Methods
    }
}