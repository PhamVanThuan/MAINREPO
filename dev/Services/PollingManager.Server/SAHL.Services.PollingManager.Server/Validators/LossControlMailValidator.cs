using SAHL.Core.Exchange;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.PollingManager.Validators
{
    public class LossControlMailValidator : ILossControlMailValidator
    {
        public List<string> Errors { get; private set; }

        public bool IsValid
        {
            get { return Errors.Count == 0; }
        }

        public bool ValidateMail(IMailMessage emailMessage)
        {
            Errors = new List<string>();
            CheckHasOnlyOneValidAttachment(emailMessage);
            CheckForValidSubjectLine(emailMessage.Subject);
            return IsValid;
        }

        public void CheckHasOnlyOneValidAttachment(IMailMessage emailMessage)
        {
            int attachmentCount = GetValidAttachmentCount(emailMessage);
            CheckForNoAttachments(attachmentCount);
            CheckForSingleValidAttachment(attachmentCount);
        }

        private void CheckForValidSubjectLine(string subjectLine)
        {
            if (string.IsNullOrEmpty(subjectLine))
            {
                Errors.Add("The email subject is blank.");
            }
            else
            {
                string[] subjectLineParts = subjectLine.Split("-".ToCharArray());
                if (subjectLineParts.Length < 2)
                {
                    Errors.Add("The email subject is in the incorrect format.");
                }

                if (subjectLineParts.Length > 1)
                {
                    int accountNumber = 0;
                    if (subjectLineParts[0].Trim().Length == 0
                        | subjectLineParts[1].Trim().Length == 0
                        | !int.TryParse(subjectLineParts[0], out accountNumber))
                    {
                        Errors.Add("The email subject is in the incorrect format.");
                    }
                }
            }
        }

        private static int GetValidAttachmentCount(IMailMessage emailMessage)
        {
            return emailMessage.Attachments.Count(x => x.AttachmentType.Equals(".pdf", StringComparison.OrdinalIgnoreCase) | x.AttachmentType.Equals(".tiff", StringComparison.OrdinalIgnoreCase));
        }

        private void CheckForNoAttachments(int attachmentCount)
        {
            if (attachmentCount == 0)
            {
                Errors.Add("The email did not contain an attachment in either PDF or TIFF format.");
            }
        }

        private void CheckForSingleValidAttachment(int attachmentCount)
        {
            if (attachmentCount > 1)
            {
                Errors.Add("The email contained more than one attachment in either PDF or TIFF format.");
            }
        }
    }
}