using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.DecisionTreeDesign.Managers.CurrentlyOpenDocument.Models
{
    public class OpenDocumentsView
    {
        public Guid Id { get; set; }
        public Guid DocumentVersionId { get; set; }
        public string Username { get; set; }
        public DateTime OpenDate { get; set; }
        public Guid DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
    }
}
