using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord(DiscriminatorValue = "7", Lazy = true)]
    [ConstructorInjector]
    public class ApplicationMortgageLoanNewPurchase_DAO : Application_DAO
    {
        private ApplicationMortgageLoanDetail_DAO _applicationMortgageLoanDetail;

        /// <summary>
        /// The ApplicationMortgageLoanNewPurchase_DAO has a one to one relationship to the ApplicationMortgageLoanDetail_DAO. This
        /// means that the two classes share primary keys, in this case the Offer.OfferKey.
        /// </summary>
        [OneToOne(Cascade = CascadeEnum.All)]
        [Lurker]
        public virtual ApplicationMortgageLoanDetail_DAO ApplicationMortgageLoanDetail
        {
            get
            {
                return this._applicationMortgageLoanDetail;
            }
            set
            {
                this._applicationMortgageLoanDetail = value;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationMortgageLoanNewPurchase_DAO Find(int id)
        {
            return ActiveRecordBase<ApplicationMortgageLoanNewPurchase_DAO>.Find(id).As<ApplicationMortgageLoanNewPurchase_DAO>();
        }

        public new static ApplicationMortgageLoanNewPurchase_DAO Find(object id)
        {
            return ActiveRecordBase<ApplicationMortgageLoanNewPurchase_DAO>.Find(id).As<ApplicationMortgageLoanNewPurchase_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationMortgageLoanNewPurchase_DAO FindFirst()
        {
            return ActiveRecordBase<ApplicationMortgageLoanNewPurchase_DAO>.FindFirst().As<ApplicationMortgageLoanNewPurchase_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationMortgageLoanNewPurchase_DAO FindOne()
        {
            return ActiveRecordBase<ApplicationMortgageLoanNewPurchase_DAO>.FindOne().As<ApplicationMortgageLoanNewPurchase_DAO>();
        }

        #endregion Static Overrides
    }
}