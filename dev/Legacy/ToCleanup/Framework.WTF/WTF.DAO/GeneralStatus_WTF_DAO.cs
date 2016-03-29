
using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("GeneralStatus", Schema = "dbo")]
    public partial class GeneralStatus_WTF_DAO : DB_Test_WTF<GeneralStatus_WTF_DAO>
    {

        private string _description;

        private int _key;

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

        [PrimaryKey(PrimaryKeyType.Native, "GeneralStatusKey", ColumnType = "Int32")]
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
    }
}

