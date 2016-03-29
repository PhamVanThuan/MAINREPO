using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;

namespace SAHL.Services.Interfaces.Communications.EmailTemplates
{
    public class StandardEmailTemplate : IEmailTemplate<IEmailModel>
    {
        public string TemplateName
        {
            get
            {
                return "StandardEmailTemplate";
            }
        }

        public IEmailModel EmailModel { get; protected set; }

        public StandardEmailTemplate(IEmailModel emailModel)
        {
            this.EmailModel = emailModel;
        }
    }
}