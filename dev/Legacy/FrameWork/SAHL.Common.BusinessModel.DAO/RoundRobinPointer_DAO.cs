using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("RoundRobinPointer", Schema = "dbo")]
    public partial class RoundRobinPointer_DAO : DB_2AM<RoundRobinPointer_DAO>
    {
        private int _roundRobinPointerIndexID;

        private string _description;

        private int _key;

        private IList<RoundRobinPointerDefinition_DAO> _roundRobinPointerDefinitions;

        private GeneralStatus_DAO _generalStatus;

        [Property("RoundRobinPointerIndexID", ColumnType = "Int32")]
        public virtual int RoundRobinPointerIndexID
        {
            get
            {
                return this._roundRobinPointerIndexID;
            }
            set
            {
                this._roundRobinPointerIndexID = value;
            }
        }

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

        [PrimaryKey(PrimaryKeyType.Assigned, "RoundRobinPointerKey", ColumnType = "Int32")]
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

        [HasMany(typeof(RoundRobinPointerDefinition_DAO), ColumnKey = "RoundRobinPointerKey", Table = "RoundRobinPointerDefinition")]
        public virtual IList<RoundRobinPointerDefinition_DAO> RoundRobinPointerDefinitions
        {
            get
            {
                return this._roundRobinPointerDefinitions;
            }
            set
            {
                this._roundRobinPointerDefinitions = value;
            }
        }

        [BelongsTo("GeneralStatusKey", NotNull = true)]
        public virtual GeneralStatus_DAO GeneralStatus
        {
            get
            {
                return this._generalStatus;
            }
            set
            {
                this._generalStatus = value;
            }
        }
    }
}