using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.WCFServices.ComcorpConnector.Server.Rules
{
    public interface IRuleHelper
    {
        string GetApplicantNameForErrorMessage(Applicant applicant);
    }
}