using Castle.ActiveRecord;
using Castle.Components.Validator;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("TransactionEffect", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class TransactionEffect_DAO : DB_2AM<TransactionEffect_DAO>
    {
        private int _key;

        private string _description;

        private double _coefficient;

        private double _balanceEffect;

        [PrimaryKey(PrimaryKeyType.Assigned, "TransactionEffectKey", ColumnType = "Int32")]
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

        [Property("Description", ColumnType = "String", NotNull = true)]
        [ValidateNonEmpty("Description is a mandatory field")]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [Property("Coefficient", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("Coefficient is a mandatory field")]
        public virtual double Coefficient
        {
            get
            {
                return this._coefficient;
            }
            set
            {
                this._coefficient = value;
            }
        }

        [Property("BalanceEffect", ColumnType = "Double", NotNull = true)]
        [ValidateNonEmpty("BalanceEffect is a mandatory field")]
        public virtual double BalanceEffect
        {
            get
            {
                return this._balanceEffect;
            }
            set
            {
                this._balanceEffect = value;
            }
        }
    }
}