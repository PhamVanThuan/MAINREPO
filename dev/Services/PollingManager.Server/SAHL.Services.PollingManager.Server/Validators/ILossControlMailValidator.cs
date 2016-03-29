using SAHL.Core.Exchange;
using System;
namespace SAHL.Services.PollingManager.Validators
{
    public interface ILossControlMailValidator
    {
        void CheckHasOnlyOneValidAttachment(IMailMessage emailMessage);
        System.Collections.Generic.List<string> Errors { get; }
        bool IsValid { get; }
        bool ValidateMail(IMailMessage emailMessage);
    }
}
