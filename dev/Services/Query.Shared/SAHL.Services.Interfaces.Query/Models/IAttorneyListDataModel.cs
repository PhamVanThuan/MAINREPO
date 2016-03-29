using System;

namespace SAHL.Services.Interfaces.Query.Models
{
    public interface IAttorneyListDataModel
    {
        Guid Id { get; set; }
        string Name { get; set; }
        bool? IsLitigationAttorney { get; set; }
        bool? IsRegistrationAttorney { get; set; }
        bool? IsPanelAttorney { get; set; }
        string AttorneyContact { get; set; }
    }
}