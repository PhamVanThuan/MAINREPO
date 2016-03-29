using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// RateOverrideType_DAO is used in order to hold the descriptions of the types of Rate Overrides.
    /// </summary>
    [GenericTest(TestType.Find)]
    [ActiveRecord("RateOverrideType", Schema = "dbo", Lazy = true)]
    public partial class RateOverrideType_DAO : DB_2AM<RateOverrideType_DAO>
    {

        private string _description;

        private int _Key;

        private RateOverrideTypeGroup_DAO _rateOverrideTypeGroup;

        // todo: Uncomment when OSPRateOverrideType implemented
        //private IList<OSPRateOverrideType> _oSPRateOverrideTypes;

        //private IList<RateOverride_DAO> _rateOverrides;

        // todo: Uncomment when RateOverrideTypeGroupLink implemented
        //private IList<RateOverrideTypeGroup_DAO> _rateOverrideTypeGroups;
        /// <summary>
        /// The description of the Rate Override. e.g. Interest Only/Super Lo/
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
        [PrimaryKey(PrimaryKeyType.Native, "RateOverrideTypeKey", ColumnType = "Int32")]
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

        [BelongsTo("RateOverrideTypeGroupKey")]
        public virtual RateOverrideTypeGroup_DAO RateOverrideTypeGroup
        {
            get
            {
                return this._rateOverrideTypeGroup;
            }
            set
            {
                this._rateOverrideTypeGroup = value;
            }
        }

        //todo: Uncomment when OSPRateOverrideType implemented
        //[HasMany(typeof(OSPRateOverrideType), ColumnKey = "RateOverrideTypeKey", Table = "OSPRateOverrideType")]
        //public virtual IList<OSPRateOverrideType> OSPRateOverrideTypes
        //{
        //    get
        //    {
        //        return this._oSPRateOverrideTypes;
        //    }
        //    set
        //    {
        //        this._oSPRateOverrideTypes = value;
        //    }
        //}

        //[HasMany(typeof(RateOverride_DAO), ColumnKey = "RateOverrideTypeKey", Table = "RateOverride")]
        //public virtual IList<RateOverride_DAO> RateOverrides
        //{
        //    get
        //    {
        //        return this._rateOverrides;
        //    }
        //    set
        //    {
        //        this._rateOverrides = value;
        //    }
        //}

        //[HasAndBelongsToMany(typeof(RateOverrideTypeGroup_DAO), Table = "RateOverrideTypeGroupLink", ColumnKey = "RateOverrideTypeKey", ColumnRef = "RateOverrideTypeGroupKey",Lazy=true)]
        //public virtual IList<RateOverrideTypeGroup_DAO> RateOverrideTypeGroups
        //{
        //    get { return _rateOverrideTypeGroups; }
        //    set { _rateOverrideTypeGroups = value; }
        //}
    }
}
