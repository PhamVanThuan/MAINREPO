using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using SAHL.Services.Interfaces.Communications.Enums;

namespace SAHL.Services.Interfaces.Communications.EmailTemplates
{
    public class InvoiceEmailTemplate : IEmailTemplate<IEmailModel>
    {
        public string TemplateName { get; protected set; }

        public IEmailModel EmailModel { get; protected set; }

        public InvoiceEmailTemplate(InvoiceTemplateType templateName, IEmailModel emailModel)
        {
            this.EmailModel = emailModel;
            this.TemplateName = templateName.ToString();
        }
    }
}