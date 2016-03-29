using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("DebtCounsellorDetail", Schema = "debtcounselling")]
    public partial class DebtCounsellorDetail_DAO : DB_2AM<DebtCounsellorDetail_DAO>
    {
        private string _nCRDCRegistrationNumber;

        private LegalEntity_DAO _legalEntity;

        private int _legalEntityKey;

        [PrimaryKey(PrimaryKeyType.Foreign, Column = "LegalEntityKey")]
        public virtual int Key
        {
            get { return _legalEntityKey; }
            set { _legalEntityKey = value; }
        }

        [Property("NCRDCRegistrationNumber", ColumnType = "String", NotNull = true)]
        public virtual string NCRDCRegistrationNumber
        {
            get
            {
                return this._nCRDCRegistrationNumber;
            }
            set
            {
                this._nCRDCRegistrationNumber = value;
            }
        }

        [OneToOne]
        public virtual LegalEntity_DAO LegalEntity
        {
            get
            {
                return this._legalEntity;
            }
            set
            {
                this._legalEntity = value;
            }
        }
    }
}