using SAHL.Services.Interfaces.Communications.EmailTemplates;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;

namespace SAHL.Services.Communications.Specs.Fakes
{
    public class TestEmailTemplate : IEmailTemplate<IEmailModel>
    {
        public string TemplateName { get; protected set; }

        public IEmailModel EmailModel { get; protected set; }

        public TestEmailTemplate(string templateName, IEmailModel emailModel)
        {
            this.TemplateName = templateName;
            this.EmailModel = emailModel;
        }
    }
}