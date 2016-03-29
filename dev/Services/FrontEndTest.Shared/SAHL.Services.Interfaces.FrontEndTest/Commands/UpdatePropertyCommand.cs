using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class UpdatePropertyCommand : ServiceCommand, IFrontEndTestCommand
    {
        public PropertyDataModel Property { get; protected set; }

        public UpdatePropertyCommand(PropertyDataModel property)
        {
            this.Property = property;
        }
    }
}
