using SAHL.Core.BusinessModel.Validation;
using SAHL.Core.Rules;
using SAHL.Core.SystemMessages;
using SAHL.WCF.Validation.Engine;
using SAHL.WCFServices.ComcorpConnector.Interfaces;
using SAHL.WCFServices.ComcorpConnector.Objects;
using SAHL.WCFServices.ComcorpConnector.Objects.Document;
using SAHL.WCFServices.ComcorpConnector.Server.Rules;
using System;
using System.ServiceModel;
using System.Text;

namespace SAHL.WCFServices.ComcorpConnector.Server
{
    public class SAHLIntegrationService : ISAHLIntegrationService, ISAHLDocumentService
    {
        // Removed the lazy implementation as it was not working. Implemented singleton.
        public ISAHLHandler Handler { get; set; }

        private IDomainRuleManager<Application> domainRuleContextApplication;
        private IDomainRuleManager<Applicant> domainRuleContextApplicant;

        public SAHLIntegrationService(ISAHLHandler handler)
        {
            if (Handler == null)
            {
                Handler = handler;
            }
        }

        #region ISAHLIntegrationService Members

        [ParameterValidator]
        public SAHLResponse Submit(SAHLRequest request)
        {
            SAHLResponse response = new SAHLResponse();
            ISystemMessageCollection messages = SystemMessageCollection.Empty();
            var errorMessages = new StringBuilder();

            // Model Validation (data annotations)
            if (!ModelState.IsValid)
            {
                foreach (var e in ModelState.Errors)
                {
                    errorMessages.Append(string.Format("Model Validation error : {0} {1}\n", e.MemberName, e.Message));
                }
            }
            else
            {
                ValidationUtils validationUtils = new ValidationUtils();

                // Application Model Validation (rules)
                this.domainRuleContextApplication = new DomainRuleManager<Application>();
                this.domainRuleContextApplication.RegisterRule(new PropertyMarketValueValidation(validationUtils));
                this.domainRuleContextApplication.ExecuteRules(messages, request.Application);

                // Applicant Model Validation (rules)
                this.domainRuleContextApplicant = new DomainRuleManager<Applicant>();
                this.domainRuleContextApplicant.RegisterRule(new ApplicantContactDetailsValidation());
                this.domainRuleContextApplicant.RegisterRule(new ApplicantAgeValidation(validationUtils));
                this.domainRuleContextApplicant.RegisterRule(new ApplicantAssetItemValidation(validationUtils));

                // run the applicant rules against the main applicant
                this.domainRuleContextApplicant.ExecuteRules(messages, request.Application.MainApplicant);

                // run the applicant rules against the co applicants
                if (request.Application.CoApplicants != null)
                {
                    foreach (var applicant in request.Application.CoApplicants)
                    {
                        this.domainRuleContextApplicant.ExecuteRules(messages, applicant);
                    }
                }
            }

            if (messages.HasErrors)
            {
                foreach (var message in messages.AllMessages)
                {
                    errorMessages.Append(string.Format("Model Validation error : {0}\n", message.Message));
                }
            }

            // if we have validation errors then throw an exception
            if (!String.IsNullOrEmpty(errorMessages.ToString()))
            {
                response.SAHLReference = errorMessages.ToString();
                ThrowSAHLFaultException(24, errorMessages.ToString(), "Model Validation");
            }

            // Model Validation has passed so lets handle the request
            try
            {
                response = Handler.Handle(request);
            }
            catch (Exception ex)
            {
                string errorMsgs = this.BuildErrorMessage(ex);

                ThrowSAHLFaultException(1, errorMsgs, "Exception");
            }

            return response;
        }

        private string BuildErrorMessage(Exception runtimeException)
        {
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine(runtimeException.Message);
            if (runtimeException.InnerException != null)
            {
                errorMessage.AppendLine(this.BuildErrorMessage(runtimeException.InnerException));
            }

            return errorMessage.ToString();
        }

        #endregion ISAHLIntegrationService Members

        #region ISAHLDocumentService Members

        public SupportingDocumentsReply ProcessApplicationMessage(ImagingApplicationRequest imagingApplicationRequest)
        {
            var response = new SupportingDocumentsReply();
            try
            {
                response = Handler.Handle(imagingApplicationRequest);
            }
            catch (Exception ex)
            {
                ThrowSAHLDocumentFaultException(2, ex.ToString(), ex.Message);
            }
            return response;
        }

        public SupportingDocumentsReply ProcessMainApplicantMessage(ImagingMainApplicantRequest imagingMainApplicantRequest)
        {
            var response = new SupportingDocumentsReply();
            try
            {
                response = Handler.Handle(imagingMainApplicantRequest);
            }
            catch (Exception ex)
            {
                ThrowSAHLDocumentFaultException(2, ex.ToString(), ex.Message);
            }
            return response;
        }

        public SupportingDocumentsReply ProcessCoApplicantMessage(ImagingCoApplicantRequest imagingCoApplicantRequest)
        {
            var response = new SupportingDocumentsReply();
            try
            {
                response = Handler.Handle(imagingCoApplicantRequest);
            }
            catch (Exception ex)
            {
                ThrowSAHLDocumentFaultException(2, ex.ToString(), ex.Message);
            }
            return response;
        }

        #endregion ISAHLDocumentService Members

        public ModelState ModelState { get; set; }

        private void ThrowSAHLFaultException(int code, string message, string reason)
        {
            SAHLFault fault = new SAHLFault();
            FaultReason faultReason = new FaultReason(reason);

            fault.FaultCode = code;
            fault.FaultDescription = message;
            throw new FaultException<SAHLFault>(fault, faultReason);
        }

        private void ThrowSAHLDocumentFaultException(int code, string message, string reason)
        {
            SAHLDocumentFault fault = new SAHLDocumentFault();
            FaultReason faultReason = new FaultReason(reason);

            fault.FaultCode = code;
            fault.FaultDescription = message;
            throw new FaultException<SAHLDocumentFault>(fault, faultReason);
        }
    }
}