using System.Collections.Generic;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("HOCHistory", Schema = "dbo")]
    public partial class HOCHistory_DAO : DB_2AM<HOCHistory_DAO>
    {
        private int _key;

        private System.DateTime? _commencementDate;

        private System.DateTime? _cancellationDate;

        private System.DateTime _changeDate;

        private string _userID;

        private IList<HOCHistoryDetail_DAO> _hOCHistoryDetails;

        private HOC_DAO _hOC;

        private HOCInsurer_DAO _hOCInsurer;

        [PrimaryKey(PrimaryKeyType.Native, "HOCHistoryKey", ColumnType = "Int32")]
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

        [Property("CommencementDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? CommencementDate
        {
            get
            {
                return this._commencementDate;
            }
            set
            {
                this._commencementDate = value;
            }
        }

        [Property("CancellationDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? CancellationDate
        {
            get
            {
                return this._cancellationDate;
            }
            set
            {
                this._cancellationDate = value;
            }
        }

        [Property("ChangeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Change Date is a mandatory field")]
        public virtual System.DateTime ChangeDate
        {
            get
            {
                return this._changeDate;
            }
            set
            {
                this._changeDate = value;
            }
        }

        [Property("UserID", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("User ID is a mandatory field")]
        public virtual string UserID
        {
            get
            {
                return this._userID;
            }
            set
            {
                this._userID = value;
            }
        }

        [HasMany(typeof(HOCHistoryDetail_DAO), ColumnKey = "HOCHistoryKey", Table = "HOCHistoryDetail", Lazy = true, Cascade = ManyRelationCascadeEnum.AllDeleteOrphan, Inverse = true)]
        public virtual IList<HOCHistoryDetail_DAO> HOCHistoryDetails
        {
            get
            {
                return this._hOCHistoryDetails;
            }
            set
            {
                this._hOCHistoryDetails = value;
            }
        }

        [BelongsTo("FinancialServiceKey", NotNull = true)]
        [ValidateNonEmpty("HOC is a mandatory field")]
        public virtual HOC_DAO HOC
        {
            get
            {
                return this._hOC;
            }
            set
            {
                this._hOC = value;
            }
        }

        [BelongsTo("HOCInsurerKey", NotNull = true)]
        [ValidateNonEmpty("HOC Insurer is a mandatory field")]
        public virtual HOCInsurer_DAO HOCInsurer
        {
            get
            {
                return this._hOCInsurer;
            }
            set
            {
                this._hOCInsurer = value;
            }
        }
    }
}