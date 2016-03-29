namespace Automation.DataModels
{
    public class LegalEntityLogin
    {
        public int LegalEntityLoginKey { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public int GeneralStatusKey { get; set; }

        public int LegalEntityKey { get; set; }

        public int AttorneyKey { get; set; }
    }
}