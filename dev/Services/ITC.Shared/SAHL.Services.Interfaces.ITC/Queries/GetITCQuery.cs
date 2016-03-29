using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using SAHL.Services.Interfaces.ITC.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAHL.Services.Interfaces.ITC.Queries
{
    public class GetITCQuery : ServiceQuery<ITCRequestDataModel>, IITCServiceQuery, ISqlServiceQuery<ITCRequestDataModel>
    {
        [Required]
        public Guid ItcID { get; set; }

        public GetITCQuery(Guid itcID)
        {
            this.ItcID = itcID;
        }
    }
}