namespace SAHL.Services.Interfaces.FrontEndTest.Models.Cuttlefish
{
    public class GetLogMessageParamsForLogMessageQueryResult
    {
        public string ParameterValue { get; set; }

        public string ParameterKey { get; set; }

        public GetLogMessageParamsForLogMessageQueryResult(string parameterValue, string parameterKey)
        {
            this.ParameterValue = parameterValue;
            this.ParameterKey = parameterKey;
        }
    }
}