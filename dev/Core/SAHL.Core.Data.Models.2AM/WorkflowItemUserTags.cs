using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class WorkflowItemUserTagsDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public WorkflowItemUserTagsDataModel(long workFlowItemId, string aDUsername, Guid tagId, DateTime createDate)
        {
            this.WorkFlowItemId = workFlowItemId;
            this.ADUsername = aDUsername;
            this.TagId = tagId;
            this.CreateDate = createDate;
		
        }
		[JsonConstructor]
        public WorkflowItemUserTagsDataModel(int itemKey, long workFlowItemId, string aDUsername, Guid tagId, DateTime createDate)
        {
            this.ItemKey = itemKey;
            this.WorkFlowItemId = workFlowItemId;
            this.ADUsername = aDUsername;
            this.TagId = tagId;
            this.CreateDate = createDate;
		
        }		

        public int ItemKey { get; set; }

        public long WorkFlowItemId { get; set; }

        public string ADUsername { get; set; }

        public Guid TagId { get; set; }

        public DateTime CreateDate { get; set; }

        public void SetKey(int key)
        {
            this.ItemKey =  key;
        }
    }
}