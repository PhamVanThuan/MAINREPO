using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.DataTransferObjects;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.DataTransferObjects
{
    public class SendCorrespondenceRequest : ISendCorrespondenceRequest
    {
        public SendCorrespondenceRequest()
        {

        }

        public SendCorrespondenceRequest(int loanNumber, int genericKey, int genericKeyTypeKey, string reportGenericValue, string workflowName, string reportName, string adusername, string dataStorName, CorrespondenceTemplates correspondenceTemplate, int legalEntityKey, SAHL.Common.Globals.CorrespondenceMediums correspondenceMedium)
        {
            this.LoanNumber = loanNumber;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.WorkflowName = workflowName;
            this.ReportName = reportName;
            this.ADUserName = adusername;
            this.DataStorName = dataStorName;
            this.CorrespondenceTemplate = correspondenceTemplate;
            this.OriginationSourceProductKey = 1;
            this.SelectedCorrespondenceMedium = correspondenceMedium;
            this.LegalEntityKey = legalEntityKey;
            this.ReportGenericValue = reportGenericValue;
        }

        public int LoanNumber { get; set; }

        public int GenericKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public string WorkflowName { get; set; }

        public string ReportName { get; set; }

        public string ADUserName { get; set; }

        public int LegalEntityKey { get; set; }

        public string DataStorName { get; set; }

        public bool SendUserConfirmationEmail { get; set; }

        public bool EmailProcessedPDFtoConsultant { get; set; }

        public CorrespondenceTemplates CorrespondenceTemplate { get; set; }

        public bool CombineDocumentsIfEmailing { get; set; }

        public bool BatchPrint { get; set; }

        public bool AllowPreview { get; set; }

        public bool UpdateConditions { get; set; }

        public int OriginationSourceProductKey { get; set; }

        public SAHL.Common.Globals.CorrespondenceMediums SelectedCorrespondenceMedium { get; set; }

        public string ReportGenericValue { get; set; }

    }
}
