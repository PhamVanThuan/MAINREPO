using System.Collections.Generic;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.DAO
{
    [ActiveRecord("Employment", Schema = "dbo", DiscriminatorColumn = "EmploymentTypeKey", DiscriminatorValue = "3", Lazy = true)]
    public class EmploymentSubsidised_DAO : Employment_DAO
    {
        private IList<Subsidy_DAO> _subsidies;

        // NOTE: Cascade is set to SaveUpdate, NOT ALL - we don't want to delete subsidies - rather set them to inactive
        [Lurker]
        [HasMany(typeof(Subsidy_DAO), ColumnKey = "EmploymentKey", Table = "Subsidy", Lazy = true, Cascade = ManyRelationCascadeEnum.SaveUpdate, Inverse = true)]
        public virtual IList<Subsidy_DAO> Subsidies
        {
            get
            {
                return this._subsidies;
            }
            set
            {
                this._subsidies = value;
            }
        }

        #region Static Overrides

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentSubsidised_DAO Find(int id)
        {
            return ActiveRecordBase<EmploymentSubsidised_DAO>.Find(id).As<EmploymentSubsidised_DAO>();
        }

        public new static EmploymentSubsidised_DAO Find(object id)
        {
            return ActiveRecordBase<EmploymentSubsidised_DAO>.Find(id).As<EmploymentSubsidised_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentSubsidised_DAO FindFirst()
        {
            return ActiveRecordBase<EmploymentSubsidised_DAO>.FindFirst().As<EmploymentSubsidised_DAO>();
        }

        /// <summary>
        /// Overridden for correct type.
        /// </summary>
        /// <returns></returns>
        public static EmploymentSubsidised_DAO FindOne()
        {
            return ActiveRecordBase<EmploymentSubsidised_DAO>.FindOne().As<EmploymentSubsidised_DAO>();
        }

        #endregion Static Overrides
    }
}