using SAHL.Tools.TileModelGenerator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.TileModelGenerator.Templates
{
    public partial class HtmlPageTemplate
    {
        public PageModel Model { get; protected set; }

        public HtmlPageTemplate(PageModel model)
        {
            this.Model = model;
        }
    }
}
