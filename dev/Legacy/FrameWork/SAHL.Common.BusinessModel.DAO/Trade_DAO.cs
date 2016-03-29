using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Trade", Schema = "dbo", Lazy = true)]
    public class Trade_DAO : DB_2AM<Trade_DAO>
    {
        private int _key;
        private string _TradeType;
        private string _Company;
        private DateTime _TradeDate;
        private DateTime _StartDate;
        private DateTime _EndDate;
        private ResetConfiguration_DAO _ResetConfiguration;
        private double _StrikeRate;
        private double _TradeBalance;
        private double _CAPBalance;
        private double? _Premium;
        private CapType_DAO _CapType;

        [PrimaryKey(PrimaryKeyType.Native, "TradeKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return _key;
            }
            set { _key = value; }
        }

        [BelongsTo("ResetConfigurationKey", NotNull = true)]
        [ValidateNonEmpty("Reset Configuration is a mandatory field")]
        public virtual ResetConfiguration_DAO ResetConfiguration
        {
            get { return _ResetConfiguration; }
            set { _ResetConfiguration = value; }
        }

        [Lurker]
        [BelongsTo("CapTypeKey", NotNull = false)]
        public virtual CapType_DAO CapType
        {
            get { return _CapType; }
            set { _CapType = value; }
        }

        [Property("TradeType", ColumnType = "String", NotNull = true, Length = 10)]
        [ValidateNonEmpty("Trade Type is a mandatory field")]
        public virtual string TradeType
        {
            get { return _TradeType; }
            set { _TradeType = value; }
        }

        [Property("Company", ColumnType = "String", NotNull = true)]
        public virtual string Company { get { return _Company; } set { _Company = value; } }

        [Property("TradeDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Trade Date is a mandatory field")]
        public virtual DateTime TradeDate { get { return _TradeDate; } set { _TradeDate = value; } }

        [Property("StartDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("Start Date is a mandatory field")]
        public virtual DateTime StartDate { get { return _StartDate; } set { _StartDate = value; } }

        [Property("EndDate", ColumnType = "Timestamp", NotNull = true)]
        [ValidateNonEmpty("End Date is a mandatory field")]
        public virtual DateTime EndDate { get { return _EndDate; } set { _EndDate = value; } }

        [Property("StrikeRate", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Strike Rate is a mandatory field")]
        public virtual double StrikeRate { get { return _StrikeRate; } set { _StrikeRate = value; } }

        [Property("TradeBalance", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Trade Balance is a mandatory field")]
        public virtual double TradeBalance { get { return _TradeBalance; } set { _TradeBalance = value; } }

        [Property("CapBalance", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("CAP Balance is a mandatory field")]
        public virtual double CapBalance { get { return _CAPBalance; } set { _CAPBalance = value; } }

        [Property("Premium", ColumnType = "Double", NotNull = false)]
        public virtual double? Premium { get { return _Premium; } set { _Premium = value; } }
    }
}