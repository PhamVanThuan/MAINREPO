using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Feature", Schema = "dbo", Lazy = false)]
    public partial class Feature_DAO : DB_2AM<Feature_DAO>
    {
        #region Private Attributes

        private string _shortName;

        private string _longName;

        private bool _hasAccess;

        private int _sequence;

        private int _key;

        private IList<Feature_DAO> _childFeatures;

        private Feature_DAO _parentFeature;

        private IList<FeatureGroup_DAO> _featureGroups;

        #endregion Private Attributes

        #region Properties

        [Property("ShortName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Short Name is a mandatory field")]
        public virtual string ShortName
        {
            get
            {
                return this._shortName;
            }
            set
            {
                this._shortName = value;
            }
        }

        [Property("LongName", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Long Name is a mandatory field")]
        public virtual string LongName
        {
            get
            {
                return this._longName;
            }
            set
            {
                this._longName = value;
            }
        }

        [Property("HasAccess", ColumnType = "Boolean", NotNull = true)]
        [ValidateNonEmpty("Has Access is a mandatory field")]
        public virtual bool HasAccess
        {
            get
            {
                return this._hasAccess;
            }
            set
            {
                this._hasAccess = value;
            }
        }

        [Property("Sequence", ColumnType = "Int32", NotNull = true)]
        [ValidateNonEmpty("Sequence is a mandatory field")]
        public virtual int Sequence
        {
            get
            {
                return this._sequence;
            }
            set
            {
                this._sequence = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "FeatureKey", ColumnType = "Int32")]
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

        [HasMany(typeof(Feature_DAO), ColumnKey = "ParentKey", Table = "Feature", Lazy = true)]
        public virtual IList<Feature_DAO> ChildFeatures
        {
            get
            {
                return this._childFeatures;
            }
            set
            {
                this._childFeatures = value;
            }
        }

        [BelongsTo("ParentKey", NotNull = false)]
        public virtual Feature_DAO ParentFeature
        {
            get
            {
                return this._parentFeature;
            }
            set
            {
                this._parentFeature = value;
            }
        }

        [HasMany(typeof(FeatureGroup_DAO), ColumnKey = "FeatureKey", Table = "FeatureGroup", Lazy = false)]
        public virtual IList<FeatureGroup_DAO> FeatureGroups
        {
            get
            {
                return this._featureGroups;
            }
            set
            {
                this._featureGroups = value;
            }
        }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}