using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;

namespace SAHL.Services.Interfaces.Communications.EmailTemplates
{
    public class ComcorpComparisonEmailTemplate : IEmailTemplate<IEmailModel>
    {
        public string TemplateName
        {
            get
            {
                return "ComcorpComparisonEmailTemplate";
            }
        }

        public IEmailModel EmailModel { get; protected set; }

        public ComcorpComparisonEmailTemplate(ComcorpComparisonEmailModel emailModel)
        {
            this.EmailModel = emailModel;
        }
    }
}