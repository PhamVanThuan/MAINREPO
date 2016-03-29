using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("MortgageLoan", Schema = "fin", Lazy = true, CustomAccess = Constants.ReadonlyAccess)]
    public partial class MortgageLoan_DAO : FinancialService_DAO
    {
        private CreditMatrix_DAO _creditMatrix;

        private Property_DAO _property;

        private MortgageLoanPurpose_DAO _mortgageLoanPurpose;

        private IList<Bond_DAO> _bonds;

        /// <summary>
        /// Used for Activerecord exclusively, please use Key.
        /// </summary>
        [JoinedKey("FinancialServiceKey")]
        public override int Key
        {
            get { return this._key; }
            set { this._key = value; }
        }

        [BelongsTo("CreditMatrixKey", NotNull = true)]
        public virtual CreditMatrix_DAO CreditMatrix
        {
            get
            {
                return this._creditMatrix;
            }
            set
            {
                this._creditMatrix = value;
            }
        }

        [BelongsTo("PropertyKey", NotNull = true)]
        public virtual Property_DAO Property
        {
            get
            {
                return this._property;
            }
            set
            {
                this._property = value;
            }
        }

        [BelongsTo("MortgageLoanPurposeKey", NotNull = true)]
        public virtual MortgageLoanPurpose_DAO MortgageLoanPurpose
        {
            get
            {
                return this._mortgageLoanPurpose;
            }
            set
            {
                this._mortgageLoanPurpose = value;
            }
        }

        [HasAndBelongsToMany(typeof(Bond_DAO), Table = "BondMortgageLoan", ColumnKey = "FinancialServiceKey", ColumnRef = "BondKey", Lazy = true)]
        public virtual IList<Bond_DAO> Bonds
        {
            get
            {
                return _bonds;
            }
            set
            {
                _bonds = value;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static MortgageLoan_DAO Find(int id)
        {
            return ActiveRecordBase<MortgageLoan_DAO>.Find(id).As<MortgageLoan_DAO>();
        }

        public new static MortgageLoan_DAO Find(object id)
        {
            return ActiveRecordBase<MortgageLoan_DAO>.Find(id).As<MortgageLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static MortgageLoan_DAO FindFirst()
        {
            return ActiveRecordBase<MortgageLoan_DAO>.FindFirst().As<MortgageLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static MortgageLoan_DAO FindOne()
        {
            return ActiveRecordBase<MortgageLoan_DAO>.FindOne().As<MortgageLoan_DAO>();
        }

        #endregion Static Overrides
    }
}