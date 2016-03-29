using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Globals;

namespace DomainService2.Workflow.PersonalLoan
{
    public class EmailCorrespondenceReportToApplicationMailingAddressCommand : StandardDomainServiceCommand
    {
        public EmailCorrespondenceReportToApplicationMailingAddressCommand(int genericKey, string adusername, string reportName, CorrespondenceTemplates correspondenceTemplate)
        {
            this.GenericKey = genericKey;
            this.ADUserName = adusername;
            this.ReportName = reportName;
            this.CorrespondenceTemplate = correspondenceTemplate;
        }

        public int GenericKey { get; set; }

        public string ADUserName { get; set; }

        public string ReportName { get; set; }

        public CorrespondenceTemplates CorrespondenceTemplate { get; set; }

        public SAHL.Common.Globals.CorrespondenceMediums CorrespondenceMedium { get; set; }
    }
}