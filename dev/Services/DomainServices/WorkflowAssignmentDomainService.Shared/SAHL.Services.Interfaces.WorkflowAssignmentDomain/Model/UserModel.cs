using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.WorkflowAssignmentDomain.Model
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public int UserOrganisationStructureKey { get; set; }
        public UserModel(string userName, int userOrganisationStructureKey, string fullName)
        {
            UserName = userName;
            UserOrganisationStructureKey = userOrganisationStructureKey;
            FullName = fullName;
        }
    }
}
