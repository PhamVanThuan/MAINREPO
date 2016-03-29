using System;

namespace Automation.DataModels
{
    public class ITC
    {
        public int ITCKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int AccountKey { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ResponseXML { get; set; }

        public string ResponseStatus { get; set; }

        public string UserID { get; set; }

        public string RequestXML { get; set; }
    }
}