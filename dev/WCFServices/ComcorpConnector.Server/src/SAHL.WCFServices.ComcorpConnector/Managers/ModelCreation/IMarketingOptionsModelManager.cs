using SAHL.DomainProcessManager.Models;
using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.WCFServices.ComcorpConnector.Managers.ModelCreation
{
    public interface IMarketingOptionsModelManager
    {
        List<ApplicantMarketingOptionModel> PopulateMarketingOptions(Applicant comcorpApplicant);
    }
}