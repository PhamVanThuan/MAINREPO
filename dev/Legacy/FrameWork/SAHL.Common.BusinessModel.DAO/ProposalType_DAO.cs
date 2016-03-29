using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ProposalType", Schema = "debtcounselling", Lazy = false)]
    public partial class ProposalType_DAO : DB_2AM<ProposalType_DAO>
    {
        private int _key;

        private string _description;

        [PrimaryKey(PrimaryKeyType.Native, "ProposalTypeKey", ColumnType = "Int32")]
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