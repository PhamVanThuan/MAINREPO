using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from Employment_DAO and is used to represent an Employment type of Unemployed.
    /// </summary>
    [ActiveRecord("Employment", Schema = "dbo", DiscriminatorValue = "4", Lazy = true)]
    public class EmploymentUnemployed_DAO : Employment_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentUnemployed_DAO Find(int id)
        {
            return ActiveRecordBase<EmploymentUnemployed_DAO>.Find(id).As<EmploymentUnemployed_DAO>();
        }

        public new static EmploymentUnemployed_DAO Find(object id)
        {
            return ActiveRecordBase<EmploymentUnemployed_DAO>.Find(id).As<EmploymentUnemployed_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentUnemployed_DAO FindFirst()
        {
            return ActiveRecordBase<EmploymentUnemployed_DAO>.FindFirst().As<EmploymentUnemployed_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentUnemployed_DAO FindOne()
        {
            return ActiveRecordBase<EmploymentUnemployed_DAO>.FindOne().As<EmploymentUnemployed_DAO>();
        }

        #endregion Static Overrides
    }
}