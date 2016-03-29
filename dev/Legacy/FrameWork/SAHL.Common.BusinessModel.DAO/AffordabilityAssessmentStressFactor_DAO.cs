using System.Collections.Generic;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("AffordabilityAssessmentStressFactor", Schema = "dbo", Lazy = true)]
    public partial class AffordabilityAssessmentStressFactor_DAO : DB_2AM<AffordabilityAssessmentStressFactor_DAO>
    {
        private int _key;

        private System.String _stressFactorPercentage;

        private System.Double _percentageIncreaseOnRepayments;

        [PrimaryKey(PrimaryKeyType.Native, "AffordabilityAssessmentStressFactorKey", ColumnType = "Int32")]
        public virtual int Key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value;
            }
        }

        [Property("StressFactorPercentage", ColumnType = "String", NotNull = true)]
        public virtual string StressFactorPercentage
        {
            get
            {
                return this._stressFactorPercentage;
            }
            set
            {
                this._stressFactorPercentage = value;
            }
        }

        [Property("PercentageIncreaseOnRepayments", ColumnType = "Double")]
        public virtual double PercentageIncreaseOnRepayments
        {
            get
            {
                return this._percentageIncreaseOnRepayments;
            }
            set
            {
                this._percentageIncreaseOnRepayments = value;
            }
        }
    }
}