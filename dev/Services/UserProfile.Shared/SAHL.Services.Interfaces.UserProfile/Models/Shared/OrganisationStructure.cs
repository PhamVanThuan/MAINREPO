using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.UserProfile.Models.Shared
{
    public class OrganisationStructure
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Type { get; set; } //should be enum
    }
}
