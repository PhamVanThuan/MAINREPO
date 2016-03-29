using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.DAO.Database;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("OfferExpenseOfferInformationQuickCashDetail", Schema = "dbo")]
    public partial class ApplicationExpenseApplicationInformationQuickCashDetail_DAO : DB_2AM<ApplicationExpenseApplicationInformationQuickCashDetail_DAO>
    {

        private int _key;

        private ApplicationExpense_DAO _applicationExpense;

        private ApplicationInformationQuickCashDetail_DAO _applicationInformationQuickCashDetail;

        [PrimaryKey(PrimaryKeyType.Native, "OfferExpenseOfferInformationQuickCashDetailKey", ColumnType = "Int32")]
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

        [BelongsTo("OfferExpenseKey",  NotNull = true)]
        public virtual ApplicationExpense_DAO ApplicationExpense
        {
            get
            {
                return this._applicationExpense;
            }
            set
            {
                this._applicationExpense = value;
            }
        }

        [BelongsTo("OfferInformationQuickCashDetailKey", NotNull = true)]
        public virtual ApplicationInformationQuickCashDetail_DAO ApplicationInformationQuickCashDetail
        {
            get
            {
                return this._applicationInformationQuickCashDetail;
            }
            set
            {
                this._applicationInformationQuickCashDetail = value;
            }
        }

    }
}
