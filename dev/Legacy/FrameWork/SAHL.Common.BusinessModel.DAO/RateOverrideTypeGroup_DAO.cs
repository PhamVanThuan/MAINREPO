using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;


namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("RateOverrideTypeGroup", Schema = "dbo")]
    public partial class RateOverrideTypeGroup_DAO : DB_2AM<RateOverrideTypeGroup_DAO>
    {

        private string _description;
        
        private int _key;
        
        private IList<RateOverrideType_DAO> _rateOverrideTypes;
        
        [Property("Description", ColumnType="String", NotNull=true)]
        public virtual string Description {
            get {
                return this._description;
            }
            set {
                this._description = value;
            }
        }
        
        [PrimaryKey(PrimaryKeyType.Assigned, "RateOverrideTypeGroupKey", ColumnType="Int32")]
        public virtual int Key {
            get {
                return this._key;
            }
            set {
                this._key = value;
            }
        }
        
        [HasMany(typeof(RateOverrideType_DAO), ColumnKey="RateOverrideTypeGroupKey", Table="RateOverrideType")]
        public virtual IList<RateOverrideType_DAO> RateOverrideTypes {
            get {
                return this._rateOverrideTypes;
            }
            set {
                this._rateOverrideTypes = value;
            }
        }
    }
}
