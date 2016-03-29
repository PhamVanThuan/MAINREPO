using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    //[GenericTest(Globals.TestType.Find)]
    [ActiveRecord("Employment", Schema = "dbo", DiscriminatorValue = "5", Lazy = true)]
    public class EmploymentUnknown_DAO : Employment_DAO
    {
        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentUnknown_DAO Find(int id)
        {
            return ActiveRecordBase<EmploymentUnknown_DAO>.Find(id).As<EmploymentUnknown_DAO>();
        }

        public new static EmploymentUnknown_DAO Find(object id)
        {
            return ActiveRecordBase<EmploymentUnknown_DAO>.Find(id).As<EmploymentUnknown_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentUnknown_DAO FindFirst()
        {
            return ActiveRecordBase<EmploymentUnknown_DAO>.FindFirst().As<EmploymentUnknown_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentUnknown_DAO FindOne()
        {
            return ActiveRecordBase<EmploymentUnknown_DAO>.FindOne().As<EmploymentUnknown_DAO>();
        }

        #endregion Static Overrides
    }
}