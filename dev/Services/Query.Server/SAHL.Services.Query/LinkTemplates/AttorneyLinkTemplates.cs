using System;
using WebApi.Hal;

namespace SAHL.Services.Query.LinkTemplates
{
    public static class AttorneyLinkTemplates
    {
        public static Link GetAttorney(Guid id)
        {
            return new Link("Attorney", "~/api/attorneys/" + id);
        }

        public static Link SearchAttorneys { get { return new Link("page", "~/api/attorneys{?searchTerm,page}"); } }
        public static Link GetAttorneyStatus { get { return LookupLinkTemplate.GetLookupType("generalstatus"); } }
        public static Link GetAttorneyDeedsOffice { get { return LookupLinkTemplate.GetLookupType("deedsoffice"); } }

    }
}