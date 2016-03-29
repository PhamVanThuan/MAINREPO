using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.CacheData;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Logging;
using SAHL.Common.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading;

namespace SAHL.Web.Services.Internal
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class PersonalLoan : IPersonalLoan
    {
		private const string PLBatchUserNameFormat = "PL_Batch_User_{0}";
        public bool CreatePersonalLoanLeadFromIdNumber(string idNumber, int messageId)
        {
            SetupPrincipal(String.Format(PLBatchUserNameFormat, Guid.NewGuid().ToString().Substring(0, 8)));
            SPC.DomainMessages.Clear();
            var successfullyCreated = false;

            using (TransactionScope transactionScope = new TransactionScope(OnDispose.Rollback))
            {
                try
                {
                    var applicationUnsecuredLendingRepository = RepositoryFactory.GetRepository<IApplicationUnsecuredLendingRepository>();
                    successfullyCreated = applicationUnsecuredLendingRepository.CreatePersonalLoanLead(idNumber);
                    if (!DomainValidationFailed)
                    {
                        transactionScope.VoteCommit();
                    }
                    else
                    {
                        LogDomainValidationIssues(idNumber, messageId, null);
                    }
                }
                catch (System.Exception exception)
                {
                    if (transactionScope != null)
                        transactionScope.VoteRollBack();

                    successfullyCreated = false;
                    LogDomainValidationIssues(idNumber, messageId, exception);
                    throw;
                }
            }

            return successfullyCreated;
        }

        private void LogDomainValidationIssues(string idNumber, int messageId, System.Exception exception)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "CreatePersonalLoanLeadFromIdNumber");
            parameters.Add("UniqueID", idNumber);
            parameters.Add(Logger.CORRELATIONID, messageId);
            AddCachedMessagesToParameters(parameters);

            LogPlugin.Logger.LogErrorMessageWithException(
                                      "CreatePersonalLoanLeadFromIdNumber"
                                    , string.Empty
                                    , exception
                                    , parameters
                                );
        }

        private void AddCachedMessagesToParameters(Dictionary<string, object> parameters)
        {
            if (SPC.DomainMessages.HasErrorMessages)
            {
                parameters.Add("SAHL.Common.DomainMessages.Errors", SPC.DomainMessages.ErrorMessages);
            }

            if (SPC.DomainMessages.HasWarningMessages)
            {
                parameters.Add("SAHL.Common.DomainMessages.Warnings", SPC.DomainMessages.WarningMessages);
            }
        }

        private void SetupPrincipal(string uniqueKey)
        {
            SAHLPrincipal principal = new SAHLPrincipal(new GenericIdentity(uniqueKey));
            Thread.CurrentPrincipal = principal;
        }

        private SAHLPrincipalCache SPC
        {
            get
            {
                return SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            }
        }

        private bool DomainValidationFailed
        {
            get
            {
                if (SPC.DomainMessages.HasErrorMessages || SPC.DomainMessages.HasWarningMessages)
                    return true;
                else
                    return false;
            }
        }
    }
}