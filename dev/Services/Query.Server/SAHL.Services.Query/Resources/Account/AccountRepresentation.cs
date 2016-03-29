using System.Diagnostics;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SAHL.Services.Interfaces.Query.Models;
using SAHL.Services.Query.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Services.Query.Metadata;
using StructureMap;
using WebApi.Hal;

namespace SAHL.Services.Query.Resources.Account
{
    public class AccountRepresentation : Representation, IRepresentation
    {
        private readonly ILinkResolver linkResolver;

        public AccountRepresentation(ILinkResolver linkResolver)
        {
            this.linkResolver = linkResolver;
        }

        public int Id { get; set; }
        public int? AccountStatusKey { get; set; }
        public DateTime? ChangeDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public decimal? FixedPayment { get; set; }
        public DateTime? OpenDate { get; set; }
        public int? ParentAccountKey { get; set; }
        public int? SPVKey { get; set; }
      
        public override string Rel
        {
            get { return this.linkResolver.GetRel(this); }
        }

        public override string Href
        {
            get
            {
                return this.linkResolver.GetHref(this, new { id = this.Id });
            }
        }
    }
}
