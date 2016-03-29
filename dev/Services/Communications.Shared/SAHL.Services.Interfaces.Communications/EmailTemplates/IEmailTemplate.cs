using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;

namespace SAHL.Services.Interfaces.Communications.EmailTemplates
{
    public interface IEmailTemplate<T> where T : IEmailModel
    {
        string TemplateName { get; }

        T EmailModel { get; }
    }
}