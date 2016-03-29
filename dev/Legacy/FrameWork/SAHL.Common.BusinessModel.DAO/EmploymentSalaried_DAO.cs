using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    /// <summary>
    /// Derived from the Employment_DAO and is used to represent an Employment type of Salaried.
    /// </summary>
    [ActiveRecord("Employment", Schema = "dbo", DiscriminatorValue = "1", Lazy = true)]
    public class EmploymentSalaried_DAO : Employment_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentSalaried_DAO Find(int id)
        {
            return ActiveRecordBase<EmploymentSalaried_DAO>.Find(id).As<EmploymentSalaried_DAO>();
        }

        public new static EmploymentSalaried_DAO Find(object id)
        {
            return ActiveRecordBase<EmploymentSalaried_DAO>.Find(id).As<EmploymentSalaried_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentSalaried_DAO FindFirst()
        {
            return ActiveRecordBase<EmploymentSalaried_DAO>.FindFirst().As<EmploymentSalaried_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentSalaried_DAO FindOne()
        {
            return ActiveRecordBase<EmploymentSalaried_DAO>.FindOne().As<EmploymentSalaried_DAO>();
        }

        #endregion Static Overrides
    }
}