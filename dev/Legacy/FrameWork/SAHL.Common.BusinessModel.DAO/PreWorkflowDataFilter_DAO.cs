using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO.Database;
using SAHL.Common.BusinessModel.DAO.Attributes;

namespace SAHL.Common.BusinessModel.DAO
{
    [GenericTest(Globals.TestType.Find)]
    [ActiveRecord("PreWorkflowDataFilter", Schema = "search")]
    public partial class PreWorkflowDataFilter_DAO : DB_2AM<PreWorkflowDataFilter_DAO>
    {
        private int _key;

        private int _workflowDataKey;

        private int _offerTypeKey;

        [PrimaryKey(PrimaryKeyType.Native, "PreWorkflowDataFilterKey", ColumnType = "Int32")]
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

        [Property("WorkflowDataKey", ColumnType = "Int32")]
        public virtual int WorkflowDataKey
        {
            get
            {
                return this._workflowDataKey;
            }
            set
            {
                this._workflowDataKey = value;
            }
        }

        [Property("OfferTypeKey", ColumnType = "Int32")]
        public virtual int OfferTypeKey
        {
            get
            {
                return this._offerTypeKey;
            }
            set
            {
                this._offerTypeKey = value;
            }
        }
    }
}