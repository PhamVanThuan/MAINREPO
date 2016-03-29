using ActionMailerNext.Standalone;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;

namespace SAHL.Services.Communications.Managers.Email
{
    public interface IEmailManager
    {
        RazorEmailResult GenerateEmail(string templateName, IEmailModel model);

        void DeliverEmail(RazorEmailResult result);
    }
}