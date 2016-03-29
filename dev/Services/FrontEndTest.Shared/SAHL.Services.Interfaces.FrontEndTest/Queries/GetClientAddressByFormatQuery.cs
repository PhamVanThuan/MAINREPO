using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SAHL.Services.Interfaces.FrontEndTest.Queries
{
    public class GetClientAddressByFormatQuery : ServiceQuery<AddressDataModel>, IFrontEndTestQuery, ISqlServiceQuery<AddressDataModel>
    {
        public GetClientAddressByFormatQuery(int addressFormat)
        {
            this.AddressFormat = addressFormat;
        }

        [Required]
        public int AddressFormat { get; protected set; }
    }
}