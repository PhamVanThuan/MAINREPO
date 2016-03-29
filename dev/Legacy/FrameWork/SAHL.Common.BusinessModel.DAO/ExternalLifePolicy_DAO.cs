using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;
using System;
using System.Linq;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ExternalLifePolicy", Lazy = true, Schema = "dbo")]
    public partial class ExternalLifePolicy_DAO : DB_2AM<ExternalLifePolicy_DAO>
    {
        private int key;
        private Insurer_DAO _Insurer;
        private DateTime _CommencementDate;
        private LifePolicyStatus_DAO _LifePolicyStatus;
        private DateTime? _CloseDate;
        private double _SumInsured;
        private bool _PolicyCeded;
        private string _PolicyNumber;
        private LegalEntity_DAO _LegalEntity;

        [PrimaryKey(PrimaryKeyType.Native, "ExternalLifePolicyKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = value;
            }
        }

        [Property("PolicyNumber", ColumnType = "String", NotNull = true)]
        public virtual string PolicyNumber
        {
            get
            {
                return _PolicyNumber;
            }
            set
            {
                _PolicyNumber = value;
            }
        }

        [BelongsTo("InsurerKey", NotNull = true)]
        [ValidateNonEmpty("Insurer is a mandatory field")]
        public virtual Insurer_DAO Insurer
        {
            get
            {
                return _Insurer;
            }
            set
            {
                _Insurer = value;
            }
        }

        [Property("CommencementDate", ColumnType = "Timestamp", NotNull = true)]
        public virtual DateTime CommencementDate
        {
            get
            {
                return _CommencementDate;
            }
            set
            {
                _CommencementDate = value;
            }
        }

        [BelongsTo("LifePolicyStatusKey", NotNull = true)]
        [ValidateNonEmpty("Life Policy Status is a mandatory field")]
        public virtual LifePolicyStatus_DAO LifePolicyStatus
        {
            get
            {
                return _LifePolicyStatus;
            }
            set
            {
                _LifePolicyStatus = value;
            }
        }

        [Property("CloseDate", ColumnType = "Timestamp", NotNull = false)]
        public virtual DateTime? CloseDate
        {
            get
            {
                return _CloseDate;
            }
            set
            {
                    _CloseDate = value;
            }
        }

        [Property("SumInsured", ColumnType = "Double", NotNull = true)]
        public virtual double SumInsured
        {
            get
            {
                return _SumInsured;
            }
            set
            {
                _SumInsured = value;
            }
        }

        [Property("PolicyCeded", ColumnType = "Boolean", NotNull = true)]
        public virtual bool PolicyCeded
        {
            get
            {
                return _PolicyCeded;
            }
            set
            {
                _PolicyCeded = value;
            }
        }

        [BelongsTo("LegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity is a mandatory field")]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return _LegalEntity;
            }
            set
            {
                _LegalEntity = value;
            }
        }
    }
}