using SAHL.Core.Data;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.FrontEndTest.Models;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetAlphaHousingApplicationQuery : ServiceQuery<GetAlphaHousingApplicationQueryResult>, IFrontEndTestQuery, ISqlServiceQuery<GetAlphaHousingApplicationQueryResult>
    {
        public GetAlphaHousingApplicationQuery(int SPVKey, double LTV, bool isAccepted)
        {
            this.SPVKey = SPVKey;
            this.LTV = LTV;
            this.IsAccepted = IsAccepted;
        }

        [Required]
        public int SPVKey { get; protected set; }

        [Required]
        public double LTV { get; protected set; }

        [Required]
        public bool IsAccepted { get; protected set; }
    }
}