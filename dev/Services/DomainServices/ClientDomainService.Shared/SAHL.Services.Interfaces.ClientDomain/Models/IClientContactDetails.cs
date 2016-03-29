using System;
using System.Linq;

namespace SAHL.Services.Interfaces.ClientDomain.Models
{
    public interface IClientContactDetails
    {
        string HomePhoneCode { get; }

        string HomePhone { get; }

        string WorkPhoneCode { get; }

        string WorkPhone { get; }

        string FaxCode { get; }

        string FaxNumber { get; }

        string Cellphone { get; }

        string EmailAddress { get; }
    }
}