using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class HelpDeskQueryDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public HelpDeskQueryDataModel(int helpDeskCategoryKey, string description, DateTime insertDate, int? memoKey, DateTime? resolvedDate)
        {
            this.HelpDeskCategoryKey = helpDeskCategoryKey;
            this.Description = description;
            this.InsertDate = insertDate;
            this.MemoKey = memoKey;
            this.ResolvedDate = resolvedDate;
		
        }
		[JsonConstructor]
        public HelpDeskQueryDataModel(int helpDeskQueryKey, int helpDeskCategoryKey, string description, DateTime insertDate, int? memoKey, DateTime? resolvedDate)
        {
            this.HelpDeskQueryKey = helpDeskQueryKey;
            this.HelpDeskCategoryKey = helpDeskCategoryKey;
            this.Description = description;
            this.InsertDate = insertDate;
            this.MemoKey = memoKey;
            this.ResolvedDate = resolvedDate;
		
        }		

        public int HelpDeskQueryKey { get; set; }

        public int HelpDeskCategoryKey { get; set; }

        public string Description { get; set; }

        public DateTime InsertDate { get; set; }

        public int? MemoKey { get; set; }

        public DateTime? ResolvedDate { get; set; }

        public void SetKey(int key)
        {
            this.HelpDeskQueryKey =  key;
        }
    }
}