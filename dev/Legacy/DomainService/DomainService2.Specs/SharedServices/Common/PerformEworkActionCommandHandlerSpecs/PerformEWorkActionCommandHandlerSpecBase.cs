using DomainService2.SharedServices.Common;
using EWorkConnector;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainService2.Specs.SharedServices.Common.PerformEworkActionCommandHandlerSpecs
{
    [Subject(typeof(PerformEWorkActionCommandHandler))]
    public class PerformEWorkActionCommandHandlerSpecBase : DomainServiceSpec<PerformEWorkActionCommand, PerformEWorkActionCommandHandler>
    {
        protected const string X2LOGIN = "X2";

        protected const string FOLDERID = "00000000000001111";
        protected const int GENERICKEY = 1;
        protected const string ASSIGNEDUSER = "";
        protected const string CURRENTSTAGE = "";

        protected static PerformEWorkActionCommand command;
        protected static PerformEWorkActionCommandHandler handler;
        protected static IeWork eWorkEngine;
        protected static IDebtCounsellingRepository debtCounsellingRepo;
    }
}
