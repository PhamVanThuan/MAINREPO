using System;
using System.Data;
using System.Configuration;
using System.Web;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common;
using SAHL.Common.Security;
using System.Security.Principal;
using System.Collections.Generic;

namespace SAHL.Common.BusinessModel.Rules.Correspondence
{

    [RuleDBTag("CorrespondenceAlreadySent",
"Warns if trying to send a document that has already been semt",
"SAHL.Rules.DLL",
"SAHL.Common.BusinessModel.Rules.Correspondence.CorrespondenceAlreadySent", false)]
    [RuleInfo]
    public class CorrespondenceAlreadySent : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            #region Check for allowed object type(s)
            if (Parameters.Length == 0)
                throw new ArgumentException("The CorrespondenceAlreadySent rule expects a Domain object to be passed.");

            if (!(Parameters[0] is List<int>))
                throw new ArgumentException("The CorrespondenceAlreadySent rule expects the following objects to be passed: List<int>.");

            #endregion

            List<int> reportStatementKeys = Parameters[0] as List<int>;
            int genericKey = Convert.ToInt32(Parameters[1]);
            int genericKeyTypeKey = Convert.ToInt32(Parameters[2]);
            ICorrespondenceRepository correspondenceRepo = RepositoryFactory.GetRepository<ICorrespondenceRepository>();

            // loop thru each of the corrspondence records that we are going to save and check if any of them have already been sent.
            List<ICorrespondence> sentCorrespondence = new List<ICorrespondence>();
            foreach (int reportStatementKey in reportStatementKeys)
            {
                // get all the correspondence for  this report and generickey
                IEventList<ICorrespondence> existingCorrespondenceList = correspondenceRepo.GetCorrespondenceByReportStatementAndGenericKey(reportStatementKey, genericKey,genericKeyTypeKey, false);
                // sort this list so we get the latest correspondence first
                List<ICorrespondence> existingCorrespondenceListSorted = new List<ICorrespondence>(existingCorrespondenceList);
                existingCorrespondenceListSorted.Sort(delegate(ICorrespondence c1, ICorrespondence c2) { return c2.Key.CompareTo(c1.Key); });

                foreach (ICorrespondence existingCorrespondence in existingCorrespondenceListSorted)
                {
                    //// check if the correspondence medium is the same
                    //if (existingCorrespondence.CorrespondenceMedium.Key == correspondence.CorrespondenceMedium.Key)
                    //{
                        sentCorrespondence.Add(existingCorrespondence);
                    //    break;
                    //}
                }
            }

            if (sentCorrespondence.Count > 0)
            {
                // build the warning message
                string errorMessage = "The following documents have already been sent.";
                int idx = 0;
                foreach (ICorrespondence corr in sentCorrespondence)
                {
                    idx++;
                    errorMessage += "<br/>" + idx + ". " + corr.ReportStatement.ReportName + " on " + (corr.CompletedDate.HasValue ? corr.CompletedDate.Value.ToString(SAHL.Common.Constants.DateTimeFormat) : corr.ChangeDate.ToString(SAHL.Common.Constants.DateTimeFormat)) + " by " + corr.UserID + " via " + corr.CorrespondenceMedium.Description;
                }
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            return 1;
        }        
    }
}
