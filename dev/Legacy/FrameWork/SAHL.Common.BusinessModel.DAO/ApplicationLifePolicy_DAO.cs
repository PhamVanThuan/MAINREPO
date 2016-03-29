using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// This object is used to facilitate the origination of a life insurance policy.
    /// </summary>
    [ActiveRecord("Offer", Schema = "dbo", DiscriminatorValue = "5", Lazy = true)]
    [ConstructorInjector]
    public class ApplicationLife_DAO : Application_DAO
    {
        private ApplicationLifeDetail_DAO _applicationLifeDetail;

        // TODO: This needs to be changed as it is causing performance issues when searching against
        // life applications as it doesn't get lazily loaded - it gets loaded for each hit
        [OneToOne(Cascade = CascadeEnum.All)]
        [Lurker]
        public virtual ApplicationLifeDetail_DAO ApplicationLifeDetail
        {
            get
            {
                return _applicationLifeDetail;
            }
            set
            {
                _applicationLifeDetail = value;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationLife_DAO Find(int id)
        {
            return ActiveRecordBase<ApplicationLife_DAO>.Find(id).As<ApplicationLife_DAO>();
        }

        public new static ApplicationLife_DAO Find(object id)
        {
            return ActiveRecordBase<ApplicationLife_DAO>.Find(id).As<ApplicationLife_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationLife_DAO FindFirst()
        {
            return ActiveRecordBase<ApplicationLife_DAO>.FindFirst().As<ApplicationLife_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationLife_DAO FindOne()
        {
            return ActiveRecordBase<ApplicationLife_DAO>.FindOne().As<ApplicationLife_DAO>();
        }

        #endregion Static Overrides
    }
}