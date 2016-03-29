using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("DebtCounselling", Schema = "migration")]
    public partial class MigrationDebtCounselling_DAO : DB_2AM<MigrationDebtCounselling_DAO>
    {
        private int _key;

        private int _accountKey;

        private int? _debtCounsellingConsultantKey;

        private int? _debtCounsellorKey;

        private int _proposalTypeKey;

        private System.DateTime? _dateOf171;

        private System.DateTime? _reviewDate;

        private System.DateTime? _courtOrderDate;

        private System.DateTime? _terminateDate;

        private System.DateTime? _sixtyDaysDate;

        private System.DateTime? _approvalDate;

        //private decimal? _approvalAmount;

        private int? _approvalUserKey;

        private System.DateTime? _paymentReceivedDate;

        private IList<MigrationDebtCounsellingExternalRole_DAO> _debtCounsellingExternalRoles;

        private IList<MigrationDebtCounsellingProposal_DAO> _debtCounsellingProposals;

        [PrimaryKey(PrimaryKeyType.Native, "DebtCounsellingKey", ColumnType = "Int32")]
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

        [Property("AccountKey", ColumnType = "Int32", NotNull = true)]
        public virtual int AccountKey
        {
            get
            {
                return this._accountKey;
            }
            set
            {
                this._accountKey = value;
            }
        }

        [Property("DebtCounsellingConsultantKey", ColumnType = "Int32")]
        public virtual int? DebtCounsellingConsultantKey
        {
            get
            {
                return this._debtCounsellingConsultantKey;
            }
            set
            {
                this._debtCounsellingConsultantKey = value;
            }
        }

        [Property("DebtCounsellorKey", ColumnType = "Int32")]
        public virtual int? DebtCounsellorKey
        {
            get
            {
                return this._debtCounsellorKey;
            }
            set
            {
                this._debtCounsellorKey = value;
            }
        }

        [Property("ProposalTypeKey", ColumnType = "Int32", NotNull = true)]
        public virtual int ProposalTypeKey
        {
            get
            {
                return this._proposalTypeKey;
            }
            set
            {
                this._proposalTypeKey = value;
            }
        }

        [Property("DateOf171", ColumnType = "Timestamp")]
        public virtual System.DateTime? DateOf171
        {
            get
            {
                return this._dateOf171;
            }
            set
            {
                this._dateOf171 = value;
            }
        }

        [Property("ReviewDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ReviewDate
        {
            get
            {
                return this._reviewDate;
            }
            set
            {
                this._reviewDate = value;
            }
        }

        [Property("CourtOrderDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? CourtOrderDate
        {
            get
            {
                return this._courtOrderDate;
            }
            set
            {
                this._courtOrderDate = value;
            }
        }

        [Property("TerminateDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? TerminateDate
        {
            get
            {
                return this._terminateDate;
            }
            set
            {
                this._terminateDate = value;
            }
        }

        [Property("SixtyDaysDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? SixtyDaysDate
        {
            get
            {
                return this._sixtyDaysDate;
            }
            set
            {
                this._sixtyDaysDate = value;
            }
        }

        [Property("ApprovalDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? ApprovalDate
        {
            get
            {
                return this._approvalDate;
            }
            set
            {
                this._approvalDate = value;
            }
        }

        //[Property("ApprovalAmount", ColumnType = "Decimal")]
        //public virtual decimal? ApprovalAmount
        //{
        //    get
        //    {
        //        return this._approvalAmount;
        //    }
        //    set
        //    {
        //        this._approvalAmount = value;
        //    }
        //}

        [Property("ApprovalUserKey", ColumnType = "Int32")]
        public virtual int? ApprovalUserKey
        {
            get
            {
                return this._approvalUserKey;
            }
            set
            {
                this._approvalUserKey = value;
            }
        }

        [Property("PaymentReceivedDate", ColumnType = "Timestamp")]
        public virtual System.DateTime? PaymentReceivedDate
        {
            get
            {
                return this._paymentReceivedDate;
            }
            set
            {
                this._paymentReceivedDate = value;
            }
        }

        [HasMany(typeof(MigrationDebtCounsellingExternalRole_DAO), ColumnKey = "DebtCounsellingKey", Table = "DebtCounsellingExternalRole")]
        public virtual IList<MigrationDebtCounsellingExternalRole_DAO> DebtCounsellingExternalRoles
        {
            get
            {
                return this._debtCounsellingExternalRoles;
            }
            set
            {
                this._debtCounsellingExternalRoles = value;
            }
        }

        [HasMany(typeof(MigrationDebtCounsellingProposal_DAO), ColumnKey = "DebtCounsellingKey", Table = "DebtCounsellingProposal")]
        public virtual IList<MigrationDebtCounsellingProposal_DAO> DebtCounsellingProposals
        {
            get
            {
                return this._debtCounsellingProposals;
            }
            set
            {
                this._debtCounsellingProposals = value;
            }
        }
    }
}