using SAHL.Batch.Common.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Service.Services
{
    public class PersonalLoanLeadCreation : IPersonalLoanLeadCreationService
    {
        public bool CreatePersonalLoanLeadFromIdNumber(string idNumber, int messageId)
        {
            var successfullyCreated = false;
            var personalLoanServiceClient = new PersonalLoanService.PersonalLoanClient();
            successfullyCreated = personalLoanServiceClient.CreatePersonalLoanLeadFromIdNumber(idNumber, messageId);
            personalLoanServiceClient.Close();
            return successfullyCreated;
        }
    }
}
