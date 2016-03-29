using Automation.DataAccess;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IAttorneyService
    {
        string GetFirstDeedsOfficeNameWithActiveAttorneys();

        IEnumerable<Automation.DataModels.DeedsOffice> GetDeedsOffices();

        string GetActiveAttorneyNameByDeedsOffice(string deedsOffice);

        void SetRegistrationIndbyAttorneykey(int attorneykey);

        Automation.DataModels.Attorney GetAttorney(bool litigationAttorney, bool registrationAttorney);

        QueryResultsRow GetAttorneyByKey(int attorneyKey);

        Automation.DataModels.Attorney GetAttorneyByLegalEntityKey(int legalEntityKey);

        Automation.DataModels.AttorneyContacts GetAttorneyContactRecord(string firstnames, string surname);

        Automation.DataModels.Attorney GetAttorneyRecord(string registeredname);

        List<Automation.DataModels.Attorney> GetAttorneys();

        Dictionary<int, string> GetLitigationAttorney();

        IEnumerable<Automation.DataModels.AttorneyInvoice> GetAttorneyInvoices(int accountkey);
    }
}