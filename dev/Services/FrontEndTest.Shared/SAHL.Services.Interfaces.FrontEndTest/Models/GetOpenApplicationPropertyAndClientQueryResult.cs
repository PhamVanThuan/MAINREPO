namespace SAHL.Services.Interfaces.FrontEndTest.Models
{
    public class GetOpenApplicationPropertyAndClientQueryResult
    {
        public int PropertyKey { get; set; }

        public string IDNumber { get; set; }

        public GetOpenApplicationPropertyAndClientQueryResult(int propertyKey, string IDNumber)
        {
            this.PropertyKey = propertyKey;
            this.IDNumber = IDNumber;
        }
    }
}