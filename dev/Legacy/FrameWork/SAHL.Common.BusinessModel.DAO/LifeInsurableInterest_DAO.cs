using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This class is used to represent the composite primary key in the LifeInsurableInterest_DAO class.
    /// </summary>

    [GenericTest(TestType.Find)]
    [ActiveRecord("LifeInsurableInterest", Schema = "dbo")]
    public partial class LifeInsurableInterest_DAO : DB_2AM<LifeInsurableInterest_DAO>
    {
        private LifeInsurableInterestType_DAO _lifeInsurableInterestType;
        private LegalEntity_DAO _legalEntity;
        private Account_DAO _account;
        private int _Key;

        [PrimaryKey(PrimaryKeyType.Native, "LifeInsurableInterestKey", ColumnType = "Int32")]
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

        [BelongsTo("LifeInsurableInterestTypeKey", NotNull = true)]
        [ValidateNonEmpty("Life Insurable Interest Type is a mandatory field")]
        public virtual LifeInsurableInterestType_DAO LifeInsurableInterestType
        {
            get { return _lifeInsurableInterestType; }
            set { _lifeInsurableInterestType = value; }
        }

        [BelongsTo("LegalEntityKey", NotNull = true)]
        [ValidateNonEmpty("Legal Entity is a mandatory field")]
        public virtual LegalEntity_DAO LegalEntity
        {
            get { return _legalEntity; }
            set { _legalEntity = value; }
        }

        [BelongsTo("AccountKey", NotNull = true)]
        [ValidateNonEmpty("Account is a mandatory field")]
        public virtual Account_DAO Account
        {
            get { return _account; }
            set { _account = value; }
        }
    }
}