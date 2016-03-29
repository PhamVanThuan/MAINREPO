using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("CreditMatrixUnsecuredLending", Schema = "dbo")]
    public partial class CreditMatrixUnsecuredLending_DAO : DB_2AM<CreditMatrixUnsecuredLending_DAO>
    {
        private char _newBusinessIndicator;

        private System.DateTime? _implementationDate;

        private int _key;

        private IList<CreditCriteriaUnsecuredLending_DAO> _creditCriteriaUnsecuredLendings;

        [PrimaryKey(PrimaryKeyType.Native, "CreditMatrixUnsecuredLendingKey", ColumnType = "Int32")]
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

        [Property("NewBusinessIndicator", ColumnType = "AnsiChar", NotNull = true)]
        public virtual char NewBusinessIndicator
        {
            get
            {
                return this._newBusinessIndicator;
            }
            set
            {
                this._newBusinessIndicator = value;
            }
        }

        [Property("ImplementationDate")]
        public virtual System.DateTime? ImplementationDate
        {
            get
            {
                return this._implementationDate;
            }
            set
            {
                this._implementationDate = value;
            }
        }

        [HasMany(typeof(CreditCriteriaUnsecuredLending_DAO), ColumnKey = "CreditMatrixUnsecuredLendingKey", Table = "CreditCriteriaUnsecuredLending")]
        public virtual IList<CreditCriteriaUnsecuredLending_DAO> CreditCriteriaUnsecuredLendings
        {
            get
            {
                return this._creditCriteriaUnsecuredLendings;
            }
            set
            {
                this._creditCriteriaUnsecuredLendings = value;
            }
        }
    }
}