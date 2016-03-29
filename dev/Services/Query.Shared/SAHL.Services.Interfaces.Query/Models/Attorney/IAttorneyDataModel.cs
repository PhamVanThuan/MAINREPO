using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using SAHL.Core.Data;

namespace SAHL.Services.Interfaces.Query.Models
{
    
    public interface IAttorneyDataModel : IDataModel, IQueryDataModel
    {

        Guid Id { get; set; }
        string Name { get; set; }
        bool? IsLitigationAttorney { get; set; }
        bool? IsRegistrationAttorney { get; set; }
        bool? IsPanelAttorney { get; set; }
        string AttorneyContact { get; set; }
        int? GeneralStatusKey { get; set; }
        string GeneralStatus { get; set; }
        int? DeedsOfficeKey { get; set; }
        string DeedsOffice { get; set; }
        decimal? Mandate { get; set; }
        bool? WorkflowEnabled { get; set; }
    }

}