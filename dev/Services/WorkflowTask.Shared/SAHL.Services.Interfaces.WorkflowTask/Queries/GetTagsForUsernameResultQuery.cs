using System.ComponentModel.DataAnnotations;
using Microsoft.SqlServer.Server;
using SAHL.Core.Data;
using SAHL.Core.Services;
using System;

namespace SAHL.Services.Interfaces.WorkflowTask.Queries
{
    public class GetTagsForUsernameResultQuery : ServiceQuery<GetTagsForUsernameResult>, IWorkflowTaskQuery
    {
        [Required]
        public string Username { get; private set; }

        public GetTagsForUsernameResultQuery(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException("username");
            }
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Argument should not be null or whitespace", "username");
            }
            this.Username = username;
        }

        public Guid Id { get; private set; }
        public IServiceQueryResult<GetTagsForUsernameResult> Result { get; set; }
    }
}