using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class CreditPricingTreeResultDataModel :  IDataModel
    {
        public CreditPricingTreeResultDataModel(string messages, string treeQuery, Guid applicationID, DateTime queryDate)
        {
            this.Messages = messages;
            this.TreeQuery = treeQuery;
            this.ApplicationID = applicationID;
            this.QueryDate = queryDate;
		
        }

        public CreditPricingTreeResultDataModel(Guid id, string messages, string treeQuery, Guid applicationID, DateTime queryDate)
        {
            this.Id = id;
            this.Messages = messages;
            this.TreeQuery = treeQuery;
            this.ApplicationID = applicationID;
            this.QueryDate = queryDate;
		
        }		

        public Guid Id { get; set; }

        public string Messages { get; set; }

        public string TreeQuery { get; set; }

        public Guid ApplicationID { get; set; }

        public DateTime QueryDate { get; set; }
    }
}