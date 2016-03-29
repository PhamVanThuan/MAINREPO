using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(TestType.Find)]
    [ActiveRecord("BehaviouralScoreCategory", Schema = "dbo")]
    public partial class BehaviouralScoreCategory_DAO : DB_2AM<BehaviouralScoreCategory_DAO>
    {
        private int _key;
        private string _description;
        private string _thresholdColour;
        private double _behaviouralScore;

        [PrimaryKey(PrimaryKeyType.Assigned, "BehaviouralScoreCategoryKey", ColumnType = "Int32")]
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

        [Property("Description", ColumnType = "String", NotNull = true)]
        public virtual string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [Property("BehaviouralScore", ColumnType = "Double", NotNull = true)]
        public virtual double BehaviouralScore
        {
            get
            {
                return this._behaviouralScore;
            }
            set
            {
                this._behaviouralScore = value;
            }
        }

        [Property("ThresholdColour", ColumnType = "String", NotNull = true)]
        public virtual string ThresholdColour
        {
            get
            {
                return this._thresholdColour;
            }
            set
            {
                this._thresholdColour = value;
            }
        }
    }
}