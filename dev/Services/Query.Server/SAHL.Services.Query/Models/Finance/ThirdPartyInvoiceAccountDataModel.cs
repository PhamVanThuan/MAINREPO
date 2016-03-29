using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Interfaces.Query.Models.Core;
using SAHL.Services.Query.Models.Account;
using SAHL.Services.Query.Models.Core;

namespace SAHL.Services.Query.Models.Finance
{
    public class ThirdPartyInvoiceAccountDataModel : IQueryDataModel
    {
        public ThirdPartyInvoiceAccountDataModel()
        {
            SetupRelationships();
        }

        public List<IRelationshipDefinition> Relationships { get; set; }
             
        public int Id { get; set; }
        public int? AccountKey { get; set; }
        public int? AccountStatusKey { get; set; }
        public DateTime? ChangeDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public decimal? FixedPayment { get; set; }
        public DateTime? OpenDate { get; set; }
        public int? SPVKey { get; set; }
        public string SpvDescription { get; set; }
        public string WorkflowProcess { get; set; }
        public string WorkflowStage { get; set; }
        public string AssignedTo { get; set; }
        
        private void SetupRelationships()
        {
            Relationships = new List<IRelationshipDefinition>();
        }

    }
}
