using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using System.ServiceModel.Activation;
using SAHL.Common.Globals;
using SAHL.Web.Services.Internal.Security;

using SAHL.Common.Service.Interfaces;
using SAHL.Web.Services.Internal.DataModel;
using SAHL.Common.Collections.Interfaces;
using System.Security.Principal;
using System.Web;
using SAHL.Common.CacheData;
using SAHL.Common.Exceptions;

namespace SAHL.Web.Services.Internal
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Attorney : AttorneyBase, IAttorney
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
            string errorMessage = String.Empty;
            leKey = String.Empty;
            ILegalEntityLogin leLogin = legalEntityRepository.GetLegalEntityLogin(username);
            //return the LEKey, this will be used as the unique identifier token 
            //for the Principal & SPC and data updates
            //Ensure that the Legal Entity Login is active before attempting to login
            if (leLogin != null && leLogin.Password != password)
            {
                errorMessage = "The supplied credentials are invalid, please check your username and password.";
                SPC.DomainMessages.Add(new SAHL.Common.DomainMessages.Error(errorMessage, errorMessage));
                return false;
            }
            if (leLogin == null)
            {
                errorMessage = "The user does not exist, please use the 'Register' feature to create an account before logging in.";
                SPC.DomainMessages.Add(new SAHL.Common.DomainMessages.Error(errorMessage, errorMessage));
                return false;
            }
            if (leLogin != null && leLogin.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
            {
                errorMessage = "The user has been deactivated, Contact SA Home Loans.";
                SPC.DomainMessages.Add(new SAHL.Common.DomainMessages.Error(errorMessage, errorMessage));
                return false;
            }
            if (leLogin != null && leLogin.Password == password && leLogin.GeneralStatus.Key == (int)GeneralStatuses.Active)
            {
                leKey = leLogin.LegalEntity.Key.ToString();
                return true;
            }
            leKey = String.Empty;
            return false;
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public bool RegisterUser(string emailAddress)
        {
            SetupPrincipal(emailAddress);

            //If the person is registered already, return false
            if (!CanRegisterUser(emailAddress))
            {
                return false;
            }

            bool userSuccesfullyRegistered = false;
            using (TransactionScope transactionScope = new TransactionScope(OnDispose.Rollback))
            {
                try
                {
                    ILegalEntityLogin legalEntityLogin = legalEntityRepository.CreateEmptyLegalEntityLogin();
                    ILegalEntityNaturalPerson person = legalEntityRepository.GetWebAccessLegalEntity(emailAddress);

                    legalEntityLogin.Username = emailAddress;
                    var generatedPassword = Password.GenerateRandom(8);
                    legalEntityLogin.Password = Password.Encrypt(generatedPassword);

                    legalEntityLogin.GeneralStatus = lookupRepository.GeneralStatuses[GeneralStatuses.Active];

                    person.LegalEntityLogin = legalEntityLogin;
                    legalEntityLogin.LegalEntity = person;

                    legalEntityRepository.SaveLegalEntityLogin(legalEntityLogin);
                    legalEntityRepository.SaveLegalEntity(person, false);

                    //If everything succeeded, let's send the user their password
                    //Confirm with claire
                    SendConfirmationEmail(emailAddress, generatedPassword);
                    transactionScope.VoteCommit();
                    userSuccesfullyRegistered = true;
                }
                catch (Exception)
                {
                    userSuccesfullyRegistered = false;
                }
            }
            return userSuccesfullyRegistered;
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
            string subject = lookupRepository.Controls.ObjectDictionary["AttorneyWebAccessRegistrationEmailSubject"].ControlText;
            string body = lookupRepository.Controls.ObjectDictionary["AttorneyWebAccessRegistrationEmailBody"].ControlText;
            body = String.Format(body, emailAddress, generatedPassword);

            messageService.SendEmailExternal(0, "donotreply@SAHL.com", emailAddress, String.Empty, String.Empty, subject, body, String.Empty, String.Empty, String.Empty);

        }

        /// <summary>
        /// Render SQL Report
        /// </summary>
        /// <param name="reportkey"></param>
        /// <param name="sqlReportParameters"></param>
        /// <param name="leKey"></param>
        /// <returns></returns>
        public byte[] RenderSQLReport(int reportkey, IDictionary<string, string> sqlReportParameters, string username, string password)
        {
            string leKey = string.Empty;
            ServiceMessage validateRequest = ValidRequest(username, password, out leKey);

            byte[] renderedReport = null;

            string renderedMessage = string.Empty;

            IReportRepository repRepo = RepositoryFactory.GetRepository<IReportRepository>();
            IReportStatement report = repRepo.GetReportStatementByKey(reportkey);
            renderedReport = repRepo.RenderSQLReport(report.StatementName, sqlReportParameters, out renderedMessage);


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
        public bool ForgottenPassword(string emailAddress)
        {
            SetupPrincipal(emailAddress);
            bool passwordReset = false;
            using (TransactionScope tran = new TransactionScope(OnDispose.Rollback))
            {
                string errorMessage = String.Empty;
                ILegalEntityLogin leL = legalEntityRepository.GetLegalEntityLogin(emailAddress);

                if (leL == null)
                    return false;

                ILegalEntity person = leL.LegalEntity;

                if (leL == null)
                {
                    errorMessage = "The user does not exist, please use the 'Register' feature to create an account before logging in.";
                    SPC.DomainMessages.Add(new SAHL.Common.DomainMessages.Error(errorMessage, errorMessage));
                    return false;
                }
                if (leL != null && leL.GeneralStatus.Key == (int)GeneralStatuses.Inactive)
                {
                    errorMessage = "A user with this email address has already been registered, Contact SA Home Loans.";
                    SPC.DomainMessages.Add(new SAHL.Common.DomainMessages.Error(errorMessage, errorMessage));
                    return false;
                }

                if (!IsValidWebUser(person.Key))
                    return false;

                SetupPrincipal(person.Key.ToString());

                var generatedPassword = Password.GenerateRandom(8);
                person.LegalEntityLogin.Password = Password.Encrypt(generatedPassword);
                legalEntityRepository.SaveLegalEntityLogin(person.LegalEntityLogin);
                legalEntityRepository.SaveLegalEntity(person, false);
                //Send the mail
                SendConfirmationEmail(emailAddress, generatedPassword);
                tran.VoteCommit();
                passwordReset = true;
            }
            return passwordReset;
        }

        /// <summary>
        /// Get Debt Counselling By Key
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        public DebtCounselling GetDebtCounsellingByKey(int debtCounsellingKey, string username, string password)
        {
            string leKey = String.Empty;
            ServiceMessage validateRequest = ValidRequest(username, password, out leKey);
            DebtCounselling debtCounsellor = null;


            IDebtCounselling debtCounselling = debtCounsellingRepository.GetDebtCounsellingByKey(debtCounsellingKey);
            debtCounsellor = AutoMapper.Mapper.Map<IDebtCounselling, DebtCounselling>(debtCounselling);
            return debtCounsellor;

        }

        /// <summary>
        /// Get Notes for Debt Counselling case
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        public List<SAHL.Web.Services.Internal.DataModel.NoteDetail> GetNotesByDebtCounselling(int debtCounsellingKey, string username, string password)
        {
            string leKey = String.Empty;
            ServiceMessage validateRequest = ValidRequest(username, password, out leKey);
            List<SAHL.Web.Services.Internal.DataModel.NoteDetail> mappedNotes = new List<NoteDetail>();

            INoteRepository noteRepo = RepositoryFactory.GetRepository<INoteRepository>();
            List<INoteDetail> notes = noteRepo.GetNoteDetailsByGenericKeyAndType(debtCounsellingKey, (int)GenericKeyTypes.DebtCounselling2AM);
            mappedNotes = new List<SAHL.Web.Services.Internal.DataModel.NoteDetail>();
            // Do Mapping
            foreach (var noteDetail in notes)
            {
                mappedNotes.Add(AutoMapper.Mapper.Map<INoteDetail, NoteDetail>(noteDetail));
            };

            return mappedNotes;
        }

        /// <summary>
        /// Search For Cases
        /// </summary>
        /// <param name="legalEntityKey"></param>
        /// <param name="accountNumber"></param>
        /// <param name="idNumber"></param>
        /// <param name="legalEntityName"></param>
        /// <returns></returns>
        public List<DebtCounselling> SearchForCases(int legalEntityKey, int accountNumber, string idNumber, string legalEntityName, string username, string password)
        {
            string leKey = string.Empty;
            List<DebtCounselling> searchResults = new List<DebtCounselling>();
            ServiceMessage validateRequest = ValidRequest(username, password, out leKey);

            List<IDebtCounselling> debtCounsellingCases = debtCounsellingRepository.SearchDebtCounsellingCasesForAttorney(legalEntityKey, idNumber, accountNumber, legalEntityName);

            searchResults = AutoMapper.Mapper.Map<List<IDebtCounselling>, List<DebtCounselling>>(debtCounsellingCases);
            return searchResults;
        }

        /// <summary>
        /// Save Note Detail
        /// </summary>
        /// <param name="noteDetail"></param>
        /// <param name="legalEntityKey"></param>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        public bool SaveNoteDetail(SAHL.Web.Services.Internal.DataModel.NoteDetail noteDetail, int legalEntityKey, int debtCounsellingKey, string username, string password)
        {
            try
            {
                string leKey = String.Empty;
                ServiceMessage validateRequest = ValidRequest(username, password, out leKey);

                using (TransactionScope txn = new TransactionScope(OnDispose.Rollback))
                {
                    try
                    {
                        INoteRepository noteRepo = RepositoryFactory.GetRepository<INoteRepository>();
                        ILookupRepository lookUpRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                        ILegalEntityRepository leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                        INote note = noteRepo.CreateEmptyNote();
                        note.GenericKey = debtCounsellingKey;
                        note.GenericKeyType = lookUpRepo.GenericKeyType.ObjectDictionary[Convert.ToInt32(GenericKeyTypes.DebtCounselling2AM).ToString()];
                        noteRepo.SaveNote(note);

                        ILegalEntity le = leRepo.GetLegalEntityByKey(legalEntityKey);

                        INoteDetail nd = noteRepo.CreateEmptyNoteDetail();
                        nd.InsertedDate = noteDetail.InsertedDate;
                        nd.LegalEntity = le;
                        nd.Note = note;
                        nd.NoteText = noteDetail.NoteText;
                        nd.Tag = noteDetail.Tag;
                        nd.WorkflowState = noteDetail.WorkflowState;
                        noteRepo.SaveNoteDetail(nd);

                        if (DomainValidationFailed)
                        {
                            HandleDomainMessages(validateRequest);
                            txn.VoteRollBack();
                        }

                        txn.VoteCommit();
                        return true;
                    }
                    catch (DomainValidationException)
                    {
                        HandleDomainMessages(validateRequest);
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="debtCounsellingKey"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public List<Proposal> GetProposals(int debtCounsellingKey, string username, string password)
        {
            string leKey = string.Empty;
            List<Proposal> proposals = new List<Proposal>();
            ServiceMessage validateRequest = ValidRequest(username, password, out leKey);

            IDebtCounselling dc = debtCounsellingRepository.GetDebtCounsellingByKey(debtCounsellingKey);

            foreach (IProposal p in dc.Proposals)
            {
                proposals.Add(AutoMapper.Mapper.Map<IProposal,Proposal>(p));
            }
            return proposals;
        }
    }
}
