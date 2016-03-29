using ActionMailerNext.Standalone;
using SAHL.Services.Communications.Managers.LiveReplies;
using SAHL.Services.Interfaces.Communications.EmailTemplates.Models;
using System;
using System.IO;
using System.Net.Mail;

namespace SAHL.Services.Communications.Managers.Email
{
    public class EmailManager : RazorMailerBase, IEmailManager
    {
        private ILiveRepliesManager communicationsManager;

        public EmailManager(ILiveRepliesManager communicationsManager)
        {
            this.communicationsManager = communicationsManager;
        }

        public override string ViewPath
        {
            get
            {
                return "EmailTemplates";
            }
        }

        public RazorEmailResult GenerateEmail(string templateName, IEmailModel model)
        {
            MailAttributes.From = new MailAddress(communicationsManager.InternalEmailFromAddress);
            MailAttributes.To.Add(new MailAddress(model.To));
            MailAttributes.Subject = model.Subject;
            MailAttributes.Priority = model.MailPriority;

            if (model.Attachments != null)
            {
                foreach (var attachment in model.Attachments)
                {
                    var file = Convert.FromBase64String(attachment.ContentAsBase64);
                    MailAttributes.Attachments.Add(attachment.AttachmentName, file);
                }
            }


            return Email(templateName, model);
        }

        // This needs an integration test
        public void DeliverEmail(RazorEmailResult result)
        {
            result.Deliver();
        }
    }
}