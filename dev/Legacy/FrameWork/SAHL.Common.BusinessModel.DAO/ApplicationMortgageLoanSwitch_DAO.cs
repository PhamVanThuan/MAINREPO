using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord(DiscriminatorValue = "6", Lazy = true)]
    [ConstructorInjector]
    public class ApplicationMortgageLoanSwitch_DAO : Application_DAO
    {
        private ApplicationMortgageLoanDetail_DAO _applicationMortgageLoanDetail;

        /// <summary>
        /// The ApplicationMortgageLoanSwitch_DAO has a one to one relationship to the ApplicationMortgageLoanDetail_DAO. This means
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
        public static ApplicationMortgageLoanSwitch_DAO Find(int id)
        {
            return ActiveRecordBase<ApplicationMortgageLoanSwitch_DAO>.Find(id).As<ApplicationMortgageLoanSwitch_DAO>();
        }

        public new static ApplicationMortgageLoanSwitch_DAO Find(object id)
        {
            return ActiveRecordBase<ApplicationMortgageLoanSwitch_DAO>.Find(id).As<ApplicationMortgageLoanSwitch_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationMortgageLoanSwitch_DAO FindFirst()
        {
            return ActiveRecordBase<ApplicationMortgageLoanSwitch_DAO>.FindFirst().As<ApplicationMortgageLoanSwitch_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationMortgageLoanSwitch_DAO FindOne()
        {
            return ActiveRecordBase<ApplicationMortgageLoanSwitch_DAO>.FindOne().As<ApplicationMortgageLoanSwitch_DAO>();
        }

        #endregion Static Overrides
    }
}