using SAHL.Core.Services;
using SAHL.Services.Interfaces.ITC.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ITC.Queries
{
    public class GetCapitecITCProfileQuery : ServiceQuery<GetITCProfileQueryResult>, IITCServiceQuery
    {
        [Required]
        public Guid ItcID { get; set; }

        public GetCapitecITCProfileQuery(Guid itcID)
        {
            this.ItcID = itcID;
        }
    }
}