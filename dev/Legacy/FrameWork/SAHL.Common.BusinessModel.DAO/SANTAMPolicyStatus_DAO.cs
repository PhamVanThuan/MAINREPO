using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("SANTAMPolicyStatus", Schema = "dbo")]
    public partial class SANTAMPolicyStatus_DAO : DB_2AM<SANTAMPolicyStatus_DAO>
    {
        private string _description;

        private int _key;

        private IList<SANTAMPolicyTracking_DAO> _sANTAMPolicyTrackings;

        [Property("Description", ColumnType = "String", NotNull = true)]
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

        [PrimaryKey(PrimaryKeyType.Assigned, "SANTAMPolicyStatusKey", ColumnType = "Int32")]
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

        [HasMany(typeof(SANTAMPolicyTracking_DAO), ColumnKey = "SANTAMPolicyStatusKey", Table = "SANTAMPolicyTracking")]
        public virtual IList<SANTAMPolicyTracking_DAO> SANTAMPolicyTrackings
        {
            get
            {
                return this._sANTAMPolicyTrackings;
            }
            set
            {
                this._sANTAMPolicyTrackings = value;
            }
        }
    }
}