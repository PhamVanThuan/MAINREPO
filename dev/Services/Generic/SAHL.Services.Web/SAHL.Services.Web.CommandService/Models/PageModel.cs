using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace SAHL.Services.Web.CommandService.Models
{
    public class PageModel
    {
        public PageModel(IEnumerable<string> commands, int pageNumber,int total)
        {
            this.Commands       = commands;
            this.PageNumber     = pageNumber;
            this.TotalPageCount = total;
        }

        public IEnumerable<string> Commands { get; protected set; }
        public int TotalPageCount { get; protected set; }
        public int PageNumber { get; protected set; }
    }
}