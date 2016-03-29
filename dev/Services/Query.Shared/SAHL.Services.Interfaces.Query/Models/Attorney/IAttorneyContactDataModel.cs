using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using SAHL.Core.Data;

namespace SAHL.Services.Interfaces.Query.Models
{

    public interface IAttorneyContactDataModel : IDataModel
    {
        int Id { get; set; }
        string AttorneyId { get; set; }
        string FirstNames { get; set; }
        string Surname { get; set; }
        string HomePhoneCode { get; set; }
        string HomePhoneNumber { get; set; }
        string WorkPhoneCode { get; set; }
        string WorkPhoneNumber { get; set; }
        string EmailAddress { get; set; }   
    }

}