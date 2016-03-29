using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Application_DAO and instantiated to represent a ReAdvance Application.
    /// This object is used to facilitate the origination of a re advance on a mortgage loan account.
    /// </summary>
    [ActiveRecord("Offer", Schema = "dbo", DiscriminatorValue = "2", Lazy = true)]
    [ConstructorInjector]
    public class ApplicationReAdvance_DAO : Application_DAO
    {
        private ApplicationMortgageLoanDetail_DAO _applicationMortgageLoan;

        /// <summary>
        /// The ApplicationReadvance_DAO has a one to one relationship to the ApplicationMortgageLoanDetail_DAO. This means
        /// that the two classes share primary keys, in this case the Offer.OfferKey.
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
        public static ApplicationReAdvance_DAO Find(int id)
        {
            return ActiveRecordBase<ApplicationReAdvance_DAO>.Find(id).As<ApplicationReAdvance_DAO>();
        }

        public new static ApplicationReAdvance_DAO Find(object id)
        {
            return ActiveRecordBase<ApplicationReAdvance_DAO>.Find(id).As<ApplicationReAdvance_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationReAdvance_DAO FindFirst()
        {
            return ActiveRecordBase<ApplicationReAdvance_DAO>.FindFirst().As<ApplicationReAdvance_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationReAdvance_DAO FindOne()
        {
            return ActiveRecordBase<ApplicationReAdvance_DAO>.FindOne().As<ApplicationReAdvance_DAO>();
        }

        #endregion Static Overrides
    }
}