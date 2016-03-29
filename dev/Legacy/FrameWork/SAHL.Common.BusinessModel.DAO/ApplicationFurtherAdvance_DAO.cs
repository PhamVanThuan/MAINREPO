using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Application_DAO. Instantiated to represent a Further Advance Application. This object is used to facilitate
    /// the origination of a Further Advance on a Mortgage Loan Account.
    /// </summary>
    [ActiveRecord("Offer", Schema = "dbo", DiscriminatorValue = "3", Lazy = true)]
    [ConstructorInjector]
    public class ApplicationFurtherAdvance_DAO : Application_DAO
    {
        private ApplicationMortgageLoanDetail_DAO _applicationMortgageLoan;

        /// <summary>
        /// The ApplicationFurtherAdvance_DAO has a one to one relationship to the ApplicationMortgageLoanDetail_DAO. This
        /// means that the two classes share primary keys, in this case the Offer.OfferKey.
        /// </summary>
        [OneToOne(Cascade = CascadeEnum.All)]
        [Lurker]
        public virtual ApplicationMortgageLoanDetail_DAO ApplicationMortgageLoanDetail
        {
            get
            {
                return this._applicationMortgageLoan;
            }
            set
            {
                this._applicationMortgageLoan = value;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationFurtherAdvance_DAO Find(int id)
        {
            return ActiveRecordBase<ApplicationFurtherAdvance_DAO>.Find(id).As<ApplicationFurtherAdvance_DAO>();
        }

        public new static ApplicationFurtherAdvance_DAO Find(object id)
        {
            return ActiveRecordBase<ApplicationFurtherAdvance_DAO>.Find(id).As<ApplicationFurtherAdvance_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationFurtherAdvance_DAO FindFirst()
        {
            return ActiveRecordBase<ApplicationFurtherAdvance_DAO>.FindFirst().As<ApplicationFurtherAdvance_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationFurtherAdvance_DAO FindOne()
        {
            return ActiveRecordBase<ApplicationFurtherAdvance_DAO>.FindOne().As<ApplicationFurtherAdvance_DAO>();
        }

        #endregion Static Overrides
    }
}