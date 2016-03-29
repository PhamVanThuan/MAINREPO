using SAHL.DomainProcessManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public interface IApplicationDebitOrderModelManager
    {
        ApplicationDebitOrderModel PopulateApplicationDebitOrder(List<ApplicantModel> applicants);
    }
}