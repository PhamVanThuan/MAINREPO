using System;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("ITCXSL", Schema = "dbo")]
    public class ITCXSL_DAO : DB_2AM<ITCXSL_DAO>
    {
        private int _Key;
        private DateTime _EffectiveDate;
        private string _StyleSheet;

        [PrimaryKey(PrimaryKeyType.Native, "ITCXslKey")]
        public virtual int Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
            }
        }

        [Property("EffectiveDate", NotNull = true, ColumnType = "Timestamp")]
        [ValidateNonEmpty("Effective Date is a mandatory field")]
        public virtual DateTime EffectiveDate
        {
            get { return _EffectiveDate; }
            set { _EffectiveDate = value; }
        }

        [Property("StyleSheet", NotNull = true, ColumnType = "String")]
        [ValidateNonEmpty("StyleSheet is a mandatory field")]
        public virtual string StyleSheet
        {
            get { return _StyleSheet; }
            set { _StyleSheet = value; }
        }
    }
}