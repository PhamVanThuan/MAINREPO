using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Logging;
using SAHL.Services.Capitec.Models.Shared;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel.Activation;

namespace SAHL.Web.Services.Internal
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Capitec" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Capitec.svc or Capitec.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Capitec : WebServiceBase, ICapitec
    {
        private const string CapitecUser = "CapitecUser_{0}_{1}";

        public bool CreateCapitecNewPurchaseApplication(NewPurchaseApplication application, int messageId)
        {
            string principal = String.Format(CapitecUser, Guid.NewGuid(), DateTime.Now.ToFileTime());
            SetupPrincipal(principal);
            SPC.DomainMessages.Clear();

            using (TransactionScope transactionScope = new TransactionScope(OnDispose.Rollback))
            {
                try
                {
                    applicationRepository.CreateCapitecApplication(application);
                    if (DomainValidationFailed)
                    {
                        throw new DomainValidationException("DomainValidationFailed");
                    }
                    transactionScope.VoteCommit();
                    return true;
                }
                catch (DomainValidationException dex)
                {
                    LogException("CreateCapitecNewPurchaseApplication", "DomainValidationException", GetLoggingParameters(application, messageId), dex);

                    SendExceptionEmail(dex);
                }
                catch (Exception ex)
                {
                    LogException("CreateCapitecNewPurchaseApplication", "Exception", GetLoggingParameters(application, messageId), ex);

                    SendExceptionEmail(ex);

                    throw ex;
                }
            }
            return false;
        }

        public bool CreateCapitecSwitchLoanApplication(SwitchLoanApplication application, int messageId)
        {
            string principal = String.Format(CapitecUser, Guid.NewGuid(), DateTime.Now.ToFileTime());
            SetupPrincipal(principal);
            SPC.DomainMessages.Clear();

            using (TransactionScope transactionScope = new TransactionScope(OnDispose.Rollback))
            {
                try
                {
                    applicationRepository.CreateCapitecApplication(application);
                    if (DomainValidationFailed)
                    {
                        throw new DomainValidationException("DomainValidationFailed");
                    }
                    transactionScope.VoteCommit();
                    return true;
                }
                catch (DomainValidationException dex)
                {
                    LogException("CreateCapitecSwitchLoanApplication", "DomainValidationException", GetLoggingParameters(application, messageId), dex);

                    SendExceptionEmail(dex);
                }
                catch (Exception ex)
                {
                    LogException("CreateCapitecSwitchLoanApplication", "Exception", GetLoggingParameters(application, messageId), ex);

                    SendExceptionEmail(ex);

                    throw ex;
                }
            }
            return false;
        }

        private void SendExceptionEmail(Exception exception)
        {
            IControlRepository controlRepo = RepositoryFactory.GetRepository<IControlRepository>();

            // build up email 
            ICorrespondenceTemplate template = commonRepository.GetCorrespondenceTemplateByKey(CorrespondenceTemplates.HaloException);

            string to = controlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.HALOExceptionEmailAddress).ControlText;
            string subject = template.Subject;
            string body = String.Format(template.Template, this.GetType().FullName, MethodBase.GetCurrentMethod().Name, exception);

            string from = controlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.HALOEmailAddress).ControlText;

            // send email
            messageService.SendEmailInternal(from, to, "", "", subject, body, true);
        }

        private Dictionary<string, Object> GetLoggingParameters(CapitecApplication application, int messageId)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("Method", "ApplicationRepository.CreateCapitecApplication");
            parameters.Add("UniqueID", application.ReservedApplicationKey);
            parameters.Add(Logger.CORRELATIONID, messageId);
            return parameters;
        }
    }
}