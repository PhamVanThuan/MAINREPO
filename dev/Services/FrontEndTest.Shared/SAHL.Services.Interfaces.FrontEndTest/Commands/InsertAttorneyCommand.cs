using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.Interfaces.FrontEndTest.Commands
{
    public class InsertAttorneyCommand : ServiceCommand, IFrontEndTestCommand
    {
        public AttorneyDataModel Attorney { get; protected set; }
        public Guid AttorneyId { get; protected set; }

        public InsertAttorneyCommand(AttorneyDataModel attorney, Guid attorneyId)
        {
            this.Attorney = attorney;
            this.AttorneyId = attorneyId;
        }
    }
}
