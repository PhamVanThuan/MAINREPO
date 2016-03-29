using SAHL.WCFServices.ComcorpConnector.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.WCFServices.ComcorpConnector.Server.Rules
{
    public class RuleHelper : IRuleHelper
    {
        public string GetApplicantNameForErrorMessage(Applicant applicant)
        {
            string applicantName = "Applicant";
            if (applicant.FirstName != null)
            {
                applicantName = applicant.FirstName;
            }
            if (applicant.Surname != null)
            {
                applicantName += " " + applicant.Surname;
            }
            applicantName += " : ";

            return applicantName;
        }
    }
}