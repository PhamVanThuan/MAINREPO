using System;

namespace SAHL.Common.BusinessModel.Interfaces.DataTransferObjects
{
    public interface ISendCorrespondenceRequest
    {
        int LoanNumber { get; set; }

        int GenericKey { get; set; }

        int GenericKeyTypeKey { get; set; }

        string ADUserName { get; set; }

        string DataStorName { get; set; }

        bool EmailProcessedPDFtoConsultant { get; set; }

        SAHL.Common.Globals.CorrespondenceTemplates CorrespondenceTemplate { get; set; }

        int LegalEntityKey { get; set; }

        string ReportName { get; set; }

        bool SendUserConfirmationEmail { get; set; }

        string WorkflowName { get; set; }

        bool BatchPrint { get; set; }

        bool AllowPreview { get; set; }

        bool UpdateConditions { get; set; }

        SAHL.Common.Globals.CorrespondenceMediums SelectedCorrespondenceMedium { get; set; }

        string ReportGenericValue { get; set; }

        int OriginationSourceProductKey { get; set; }

        bool CombineDocumentsIfEmailing { get; set; }
    }
}
