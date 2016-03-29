using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Web.Services.Internal.DataModel;
using SAHL.Web.Services.Internal.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace SAHL.Web.Services.Internal
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ClientSecure : WebServiceBase, IClientSecure
    {
        /// <summary>
        /// Get Message
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ServiceMessage GetServiceMessage(string legalEntityKey)
        {
            SetupPrincipal(legalEntityKey);
            ServiceMessage response = new ServiceMessage();
            if (SPC != null &&
                SPC.DomainMessages.Count > 0)
            {
                HandleDomainMessages(response);
                SPC.DomainMessages.Clear();
            }
            return response;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="leKey"></param>
        /// <returns></returns>
        public bool Login(string username, string password, out string leKey)
        {
            SetupPrincipal(username);
            string errorMessage = "The supplied credentials are invalid, please check your e-mail address and password.";
            leKey = String.Empty;
            ILegalEntity le = legalEntityRepository.GetLegalEntityClientByEmail(username);

            if (le != null)
            {
                //If no LE Login then check the old LegalEntity table to see if the password matches.
                if (le.LegalEntityLogin == null && !String.IsNullOrWhiteSpace(le.Password))
                {
                    string oldPassword = Security.Password.Encrypt(le.Password.Trim());
                    if (String.Equals(password, oldPassword)) //
                        CreateLegalEntityLogin(username, password, le);
                }

                //return the LEKey, this will be used as the unique identifier token 
                //for the Principal & SPC and data updates
                if (le.LegalEntityLogin != null && le.LegalEntityLogin.Password == password) //&& leLogin.GeneralStatus.Key == (int)GeneralStatuses.Active
                {
                    leKey = le.Key.ToString();
                    return true;
                }
            }

            //if (le == null || (le.LegalEntityLogin != null && le.LegalEntityLogin.Password != password))
            SPC.DomainMessages.Add(new SAHL.Common.DomainMessages.Error(errorMessage, errorMessage));
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="leKey"></param>
        /// <returns></returns>
        private ServiceMessage ValidRequest(string username, string password, out string legalEntityKey)
        {
            ServiceMessage sr = new ServiceMessage(true);
            if (!Login(username, password, out  legalEntityKey))
            {
                sr.ServiceMessages.Add(new Message(ServiceMessageType.Error, _validationLoginMessage));
            }
            else
            {
                SetupPrincipal(legalEntityKey);
            }
            return sr;
        }

        /// <summary>
        /// Send Confirmation Email
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="generatedPassword"></param>
        private void SendConfirmationEmail(string emailAddress, string generatedPassword)
        {

            //If everything succeeded, let's send the user their password
            //Confirm with claire
            string subject = lookupRepository.Controls.ObjectDictionary["ClientWebAccessRegistrationEmailSubject"].ControlText;
            string body = lookupRepository.Controls.ObjectDictionary["ClientWebAccessRegistrationEmailBody"].ControlText;
            string emailFrom = lookupRepository.Controls.ObjectDictionary["ResetPasswordMailFrom"].ControlText;

            body = String.Format(body, emailAddress, generatedPassword);

            messageService.SendEmailExternal(0, emailFrom, emailAddress, String.Empty, String.Empty, subject, body, String.Empty, String.Empty, String.Empty);

        }

        /// <summary>
        /// Render SQL Report
        /// </summary>
        /// <param name="reportkey"></param>
        /// <param name="sqlReportParameters"></param>
        /// <param name="leKey"></param>
        /// <returns></returns>
        public byte[] RenderSQLReport(out string contentType, out string fileExtension, int reportkey, IDictionary<string, string> sqlReportParameters, int reportFormatTypeKey, string username, string password)
        {
            IReportFormatType reportFormatType = lookupRepository.ReportFormatTypes.ObjectDictionary[reportFormatTypeKey.ToString()];

            fileExtension = reportFormatType.FileExtension;
            contentType = reportFormatType.ContentType;

            string leKey = string.Empty;
            ServiceMessage validateRequest = ValidRequest(username, password, out leKey);

            byte[] renderedReport = null;

            string renderedMessage = string.Empty;

            IReportRepository repRepo = RepositoryFactory.GetRepository<IReportRepository>();
            IReportStatement report = repRepo.GetReportStatementByKey(reportkey);
            renderedReport = repRepo.RenderSQLReport(report.StatementName, sqlReportParameters, reportFormatType.ReportServicesFormatType, out renderedMessage);


            if (!String.IsNullOrEmpty(renderedMessage))
            {
                validateRequest.ServiceMessages.Add(new Message(ServiceMessageType.Error, renderedMessage));
            }

            return renderedReport;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IList<Int32> MortgageLoanAccountKeys(string username, string password)
        {
            string leKey = String.Empty;
            ServiceMessage validateRequest = ValidRequest(username, password, out leKey);
            if (!validateRequest.Success)
                return new List<Int32>(); ;

            return legalEntityRepository.GetAllClientAccountsByLegalEntityKey(Convert.ToInt32(leKey));
        }

        public IDictionary<int, string> ReportFormats(string username, string password)
        {
            string leKey = String.Empty;
            var reportFormats = new Dictionary<int, string>();

            ServiceMessage validateRequest = ValidRequest(username, password, out leKey);
            if (!validateRequest.Success)
                return reportFormats;

            foreach (var formatType in lookupRepository.ReportFormatTypes)
            {
                if (!reportFormats.ContainsKey(formatType.Key))
                    reportFormats.Add(formatType.Key, formatType.Description);
            }

            return reportFormats;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ReportStatementKey"></param>
        /// <param name="mappedReportParams"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public List<SAHL.Web.Services.Internal.DataModel.ReportParameter> GetReportParametersByStatementKey(int ReportStatementKey, string username, string password)
        {
            string leKey = String.Empty;
            ServiceMessage validateRequest = ValidRequest(username, password, out leKey);

            List<SAHL.Web.Services.Internal.DataModel.ReportParameter> mappedReportParams = new List<ReportParameter>();

            IReportRepository reportRepo = RepositoryFactory.GetRepository<IReportRepository>();
            IReadOnlyEventList<IReportParameter> reportParams = reportRepo.GetReportParametersByReportStatementKey(ReportStatementKey);

            mappedReportParams = new List<ReportParameter>();

            foreach (var reportParam in reportParams)
            {
                mappedReportParams.Add(AutoMapper.Mapper.Map<IReportParameter, ReportParameter>(reportParam));
            };

            return mappedReportParams;
        }

        /// <summary>
        /// Forgotten Password
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public bool ResetPassword(string emailAddress)
        {
            SetupPrincipal(emailAddress);
            bool passwordReset = false;

            using (TransactionScope tran = new TransactionScope(TransactionMode.Inherits, OnDispose.Rollback))
            {
                var generatedPassword = Password.GenerateRandom(8);

                //Check if the LELogin exists
                ILegalEntity le = legalEntityRepository.GetLegalEntityClientByEmail(emailAddress);

                //No LegalEntity exists, do nothing
                if (le == null || le.Roles.Count ==0)
                    return true;

                try
                {
                    //ignore any LE validation
                    SPC.ExclusionSets.Add(RuleExclusionSets.LegalEntityExcludeAll);

                    //If there is a LegalEntity but no LE Login
                    //Create the LELogin record, set the pwd and email the client
                    if (le.LegalEntityLogin == null)
                    {
                        CreateLegalEntityLogin(emailAddress, Password.Encrypt(generatedPassword), le);
                    }
                    //If the LELogin record exists, reset the pwd and email the client
                    else if (le.LegalEntityLogin != null)
                    {
                        le.LegalEntityLogin.Password = Password.Encrypt(generatedPassword);
                        legalEntityRepository.SaveLegalEntityLogin(le.LegalEntityLogin);

                        //clear the old LE password just in case
                        if (!String.IsNullOrEmpty(le.Password))
                        {
                            le.Password = "";
                            legalEntityRepository.SaveLegalEntity(le, false);
                        }
                    }
                }
                finally
                {
                    SPC.ExclusionSets.Clear();
                }

                //An email will be sent with your password if you are a valid SAHL customer
                //If you have not received an email within 15 minutes please contact SAHL on TelNo to confirm your details

                SetupPrincipal(le.Key.ToString());
                //Send the mail
                SendConfirmationEmail(emailAddress, generatedPassword);
                tran.VoteCommit();
                passwordReset = true;
            }
            return passwordReset;
        }

        private void CreateLegalEntityLogin(string emailAddress, string encryptedPassword, ILegalEntity le)
        {
            try
            {
                SPC.ExclusionSets.Add(RuleExclusionSets.LegalEntityExcludeAll);

                using (TransactionScope tran = new TransactionScope(TransactionMode.Inherits, OnDispose.Rollback))
                {
                    ILegalEntityLogin legalEntityLogin = legalEntityRepository.CreateEmptyLegalEntityLogin();

                    legalEntityLogin.Username = " "; //emailAddress; //using the email address from the LE table
                    legalEntityLogin.Password = encryptedPassword;
                    legalEntityLogin.GeneralStatus = lookupRepository.GeneralStatuses[GeneralStatuses.Active];

                    //clear the old LE password just in case
                    le.Password = "";
                    le.LegalEntityLogin = legalEntityLogin;
                    legalEntityLogin.LegalEntity = le;

                    legalEntityRepository.SaveLegalEntityLogin(le.LegalEntityLogin);
                    legalEntityRepository.SaveLegalEntity(le, false);

                    tran.VoteCommit();
                }
            }
            finally
            {
                SPC.ExclusionSets.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public bool LegalEntityChangePassword(string userName, string password, string newPassword)
        {
            bool passwordReset = false;
            string leKey = String.Empty;
            ServiceMessage validateRequest = ValidRequest(userName, password, out leKey);

            if (!validateRequest.Success)
                return false;

            using (TransactionScope tran = new TransactionScope(TransactionMode.Inherits, OnDispose.Rollback))
            {
                ILegalEntity le = legalEntityRepository.GetLegalEntityClientByEmail(userName);
                ILegalEntityLogin leLogin = le.LegalEntityLogin;//legalEntityRepository.GetLegalEntityLogin(userName);
                leLogin.Password = newPassword;
                legalEntityRepository.SaveLegalEntityLogin(leLogin);

                tran.VoteCommit();
                passwordReset = true;
            }
            return passwordReset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="accountKey"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool RequestFunds(string userName, string password, int accountKey, decimal amount)
        {
            bool result = false;
            string leKey = String.Empty;
            ServiceMessage validateRequest = ValidRequest(userName, password, out leKey);

            if (!validateRequest.Success)
                return false;

            using (TransactionScope tran = new TransactionScope(TransactionMode.Inherits, OnDispose.Rollback))
            {
                result = legalEntityRepository.ClientRequestAdditionalFunds(accountKey, amount);

                tran.VoteCommit();
            }
            return result;
        }

        public int? GetSubsidyAccountKey(int legalEntityKey)
        {
            return legalEntityRepository.GetSubsidyAccountKey(legalEntityKey);
        }
    }
}
