using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from Employment_DAO and is used to represent an Employment type of Self Employed.
    /// </summary>
    [ActiveRecord("Employment", Schema = "dbo", DiscriminatorValue = "2", Lazy = true)]
    public class EmploymentSelfEmployed_DAO : Employment_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentSelfEmployed_DAO Find(int id)
        {
            return ActiveRecordBase<EmploymentSelfEmployed_DAO>.Find(id).As<EmploymentSelfEmployed_DAO>();
        }

        public new static EmploymentSelfEmployed_DAO Find(object id)
        {
            return ActiveRecordBase<EmploymentSelfEmployed_DAO>.Find(id).As<EmploymentSelfEmployed_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentSelfEmployed_DAO FindFirst()
        {
            return ActiveRecordBase<EmploymentSelfEmployed_DAO>.FindFirst().As<EmploymentSelfEmployed_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentSelfEmployed_DAO FindOne()
        {
            return ActiveRecordBase<EmploymentSelfEmployed_DAO>.FindOne().As<EmploymentSelfEmployed_DAO>();
        }

        #endregion Static Overrides
    }
}