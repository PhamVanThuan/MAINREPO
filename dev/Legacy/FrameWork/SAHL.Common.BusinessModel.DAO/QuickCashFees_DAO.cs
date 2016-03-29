using System;
using System.Collections.Generic;
using System.Collections;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{

    [ActiveRecord("QuickCashFees", Schema = "dbo")]
    public partial class QuickCashFees_DAO : DB_2AM<QuickCashFees_DAO>
           
    {
        private double _fee;

        private double _feePercentage;

        private double _key;

        [Property("Fee", ColumnType = "Double")]
        public virtual double Fee
        {
            get
            {
                return this._fee;
            }
            set
            {
                this._fee = value;
            }
        }

        [Property("FeePercentage", ColumnType = "Double")]
        public virtual double FeePercentage
        {
            get
            {
                return this._feePercentage;
            }
            set
            {
                this._feePercentage = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "FeeRange", ColumnType = "Double")]
        public virtual double Key
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
