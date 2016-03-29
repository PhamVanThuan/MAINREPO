using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetPropertyForAccountQuery : ServiceQuery<PropertyDataModel>, IFrontEndTestQuery, ISqlServiceQuery<PropertyDataModel>
    {
        [Required]
        public int AccountKey { get; protected set; }

        public GetPropertyForAccountQuery(int accountKey)
        {
            this.AccountKey = accountKey;
        }
    }
}