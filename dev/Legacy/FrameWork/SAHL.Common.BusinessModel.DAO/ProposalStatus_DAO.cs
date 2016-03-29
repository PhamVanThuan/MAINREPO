using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ProposalStatus", Schema = "debtcounselling", Lazy = false)]
    public partial class ProposalStatus_DAO : DB_2AM<ProposalStatus_DAO>
    {
        private int _key;

        private string _description;

        [PrimaryKey(PrimaryKeyType.Native, "ProposalStatusKey", ColumnType = "Int32")]
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

    }
}