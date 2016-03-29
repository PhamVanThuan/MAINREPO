using System;
using System.Collections.Generic;
using System.Text;
using WatiN.Core;
using WatiN.Core.Logging;
using NUnit.Framework;
using BuildingBlocks;
using System.Xml;

namespace ApplicationCaptureTests
{
    //[TestFixture, RequiresSTA]
    public partial class BatchLeadCreation
    {
        Browser browser;
      

        private static class OfferLeads
        {
            public enum LeadType
            {
                BranchConsultantLead = 0,
                BranchAdminLead,
                InternetLead,
                EstateAgent
            }
            public struct OfferLeadData
            {
                public string _idnumber;
                public string _offerkey;
                public string _offersourcekey;
                public string _OrigConsultant;
                public string _LegalEntityFullName;
            }
            private static ImportCSV csvCreatedOffers;
            private static SQLQuerying.DataHelper._2AM db2AM;
            public static void PersistOfferLeadData(OfferLeadData LeadData, LeadType Type, int Index)
            {
                if (Type == LeadType.BranchAdminLead)
                    csvCreatedOffers = new ImportCSV(@"DataFiles\BranchAdminLeads.csv", ',');
                if (Type == LeadType.BranchConsultantLead)
                    csvCreatedOffers = new ImportCSV(@"DataFiles\BranchConsultantLeads.csv", ',');
                if (Type == LeadType.EstateAgent)
                    csvCreatedOffers = new ImportCSV(@"DataFiles\EstateAgentLeads.csv", ',');
                if (Type == LeadType.InternetLead)
                    csvCreatedOffers = new ImportCSV(@"DataFiles\InternetLeads.csv", ',');
                csvCreatedOffers.AddValues("OfferKey", Index, LeadData._offerkey);
                csvCreatedOffers.AddValues("OrigConsultant", Index, LeadData._OrigConsultant);
                if (Type == LeadType.BranchAdminLead)
                    csvCreatedOffers.OutputFile(@"DataFiles\BranchAdminLeads.csv");
                if (Type == LeadType.BranchConsultantLead)
                    csvCreatedOffers.OutputFile(@"DataFiles\BranchConsultantLeads.csv");
                if (Type == LeadType.EstateAgent)
                    csvCreatedOffers.OutputFile(@"DataFiles\EstateAgentLeads.csv");
                if (Type == LeadType.InternetLead)
                    csvCreatedOffers.OutputFile(@"DataFiles\InternetLeads.csv");
            }
        }

        
    }
}
