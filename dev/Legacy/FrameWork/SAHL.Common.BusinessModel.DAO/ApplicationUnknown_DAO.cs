using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Application_DAO and instantiated to represent a Unknown Application.
    /// </summary>
    [ActiveRecord(DiscriminatorValue = "9", Lazy = true)]
    public class ApplicationUnknown_DAO : Application_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationUnknown_DAO Find(int id)
        {
            return ActiveRecordBase<ApplicationUnknown_DAO>.Find(id).As<ApplicationUnknown_DAO>();
        }

        public static ApplicationUnknown_DAO Find(object id)
        {
            return ActiveRecordBase<ApplicationUnknown_DAO>.Find(id).As<ApplicationUnknown_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationUnknown_DAO FindFirst()
        {
            return ActiveRecordBase<ApplicationUnknown_DAO>.FindFirst().As<ApplicationUnknown_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static ApplicationUnknown_DAO FindOne()
        {
            return ActiveRecordBase<ApplicationUnknown_DAO>.FindOne().As<ApplicationUnknown_DAO>();
        }

        #endregion Static Overrides
    }
}