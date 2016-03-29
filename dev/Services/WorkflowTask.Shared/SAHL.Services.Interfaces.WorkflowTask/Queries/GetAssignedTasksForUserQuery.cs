using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SAHL.Core.Services;


namespace SAHL.Services.Interfaces.WorkflowTask.Queries
{
    public class GetAssignedTasksForUserQuery : ServiceQuery<GetAssignedTasksForUserQueryResult>, IWorkflowTaskQuery
    {
        public GetAssignedTasksForUserQuery(string username, List<string> capabilities)
        {
            SetUsername(username);
            SetCapabilities(capabilities);
        }

        [Required]
        public List<string> Capabilites { get; private set; }

        [Required]
        public string Username { get; private set; }

        private void SetCapabilities(List<string> capabilities)
        {
            this.Capabilites = capabilities ?? new List<string>();
        }

        private void SetUsername(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException("username");
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Argument should not be empty or whitespace", "username");
            }
            this.Username = username;
        }
    }
}
