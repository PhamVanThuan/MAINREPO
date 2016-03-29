using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord(DiscriminatorValue = "8", Lazy = true)]
    [ConstructorInjector]
    public class ApplicationMortgageLoanRefinance_DAO : Application_DAO
    {
        //private ApplicationMortgageLoanDetail_DAO _applicationMortgageLoan;

        private ApplicationMortgageLoanDetail_DAO _applicationMortgageLoanDetail;

        /// <summary>
        /// The ApplicationMortgageLoanRefinance_DAO has a one to one relationship to the ApplicationMortgageLoanDetail_DAO. This means
        /// that the two classes share primary keys, in this case the Offer.OfferKey.
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
        public static ApplicationMortgageLoanRefinance_DAO Find(int id)
        {
            return ActiveRecordBase<ApplicationMortgageLoanRefinance_DAO>.Find(id).As<ApplicationMortgageLoanRefinance_DAO>();
        }

        public new static ApplicationMortgageLoanRefinance_DAO Find(object id)
        {
            return ActiveRecordBase<ApplicationMortgageLoanRefinance_DAO>.Find(id).As<ApplicationMortgageLoanRefinance_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationMortgageLoanRefinance_DAO FindFirst()
        {
            return ActiveRecordBase<ApplicationMortgageLoanRefinance_DAO>.FindFirst().As<ApplicationMortgageLoanRefinance_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationMortgageLoanRefinance_DAO FindOne()
        {
            return ActiveRecordBase<ApplicationMortgageLoanRefinance_DAO>.FindOne().As<ApplicationMortgageLoanRefinance_DAO>();
        }

        #endregion Static Overrides
    }
}