using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.DecisionTreeDesign.Models
{
    public class GetAllMessageVersionsQueryResult
    {
        public Guid Id { get; set; }

        public int Version { get; set; }

        public bool IsPublished { get; set; }

        public DateTime? PublishDate { get; set; }

        public string Publisher { get; set; }
    }
}
