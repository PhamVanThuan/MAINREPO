using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("CorrespondenceParameters", Schema = "dbo")]
    public partial class CorrespondenceParameters_DAO : DB_2AM<CorrespondenceParameters_DAO>
    {
        //private int _correspondenceKey;

        private string _reportParameterValue;

        private int _Key;

        private Correspondence_DAO _correspondence;

        private ReportParameter_DAO _reportParameter;

        [BelongsTo("CorrespondenceKey", NotNull = true)]
        [ValidateNonEmpty("Correspondence is a mandatory field")]
        public virtual Correspondence_DAO Correspondence
        {
            get
            {
                return this._correspondence;
            }
            set
            {
                this._correspondence = value;
            }
        }

        //[Property("CorrespondenceKey", ColumnType = "Int32", NotNull = true)]
        //public virtual int CorrespondenceKey
        //{
        //    get
        //    {
        //        return this._correspondenceKey;
        //    }
        //    set
        //    {
        //        this._correspondenceKey = value;
        //    }
        //}

        [Property("ReportParameterValue", ColumnType = "String")]
        public virtual string ReportParameterValue
        {
            get
            {
                return this._reportParameterValue;
            }
            set
            {
                this._reportParameterValue = value;
            }
        }

        [PrimaryKey(PrimaryKeyType.Native, "CorrespondenceParameterKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        [BelongsTo("ReportParameterKey", NotNull = true)]
        [ValidateNonEmpty("Report Parameter is a mandatory field")]
        public virtual ReportParameter_DAO ReportParameter
        {
            get
            {
                return this._reportParameter;
            }
            set
            {
                this._reportParameter = value;
            }
        }
    }
}