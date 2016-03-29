using DomainService2.V3.Client.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.V3.Client
{
    public class ApplicationDomainService : LightweightV3ClientBase, IApplicationDomainService
    {
        public ApplicationDomainService()
            : base("ApplicationDomainService")
        {   }

        public void ConfirmApplicationAffordabilityAssessments(int applicationKey, string ADUserName)
        {
            var command = new ConfirmAffordabilityAssessmentsCommand(applicationKey, ADUserName);

            var response = this.PerformCommand(command);

            if(response.IsErrorResponse)
            {
                throw new Exception(string.Join(", ", response.Errors.Errors));
            }
        }
    }
}
