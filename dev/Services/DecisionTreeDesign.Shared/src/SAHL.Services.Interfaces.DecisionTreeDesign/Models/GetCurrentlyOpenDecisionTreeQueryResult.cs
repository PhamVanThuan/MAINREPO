using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetCurrentlyOpenDecisionTreeQueryResult
    {
        public Guid Id { get; set; }
        public Guid DocumentVersionId { get; set; }
        public string Username { get; set; }
        public DateTime OpenDate { get; set; }
        public Guid DocumentTypeId { get; set; }
    }
}
