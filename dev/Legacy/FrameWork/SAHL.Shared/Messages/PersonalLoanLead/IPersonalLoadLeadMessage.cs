using System;
namespace SAHL.Shared.Messages.PersonalLoanLead
{
    public interface IPersonalLoanLeadMessage : IBatchMessage
    {
        string IdNumber { get; }
    }
}
