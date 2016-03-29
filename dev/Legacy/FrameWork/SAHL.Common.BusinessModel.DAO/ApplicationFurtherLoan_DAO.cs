using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Application_DAO. Instantiated to represent a Further Loan Application. This object is used to facilitate
    /// the origination of a Further Loan on a Mortgage Loan Account.
    /// </summary>
    [ActiveRecord("Offer", Schema = "dbo", DiscriminatorValue = "4", Lazy = true)]
    [ConstructorInjector]
    public class ApplicationFurtherLoan_DAO : Application_DAO
    {
        private ApplicationMortgageLoanDetail_DAO _applicationMortgageLoanDetail;

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
        public static ApplicationFurtherLoan_DAO Find(int id)
        {
            return ActiveRecordBase<ApplicationFurtherLoan_DAO>.Find(id).As<ApplicationFurtherLoan_DAO>();
        }

        public new static ApplicationFurtherLoan_DAO Find(object id)
        {
            return ActiveRecordBase<ApplicationFurtherLoan_DAO>.Find(id).As<ApplicationFurtherLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationFurtherLoan_DAO FindFirst()
        {
            return ActiveRecordBase<ApplicationFurtherLoan_DAO>.FindFirst().As<ApplicationFurtherLoan_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationFurtherLoan_DAO FindOne()
        {
            return ActiveRecordBase<ApplicationFurtherLoan_DAO>.FindOne().As<ApplicationFurtherLoan_DAO>();
        }

        #endregion Static Overrides
    }
}