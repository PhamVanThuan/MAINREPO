using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.V3.Client
{
    public interface IApplicationDomainService
    {
        void ConfirmApplicationAffordabilityAssessments(int applicationKey, string ADUserName);
    }
}
