// -----------------------------------------------------------------------
// <copyright file="IsFurtherLoanCommandHandler.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DomainService2.Workflow.Origination.FurtherLending
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IsFurtherLoanCommandHandler : IHandlesDomainServiceCommand<IsFurtherLoanCommand>
    {
        private IApplicationRepository appRepo;
        public IsFurtherLoanCommandHandler(IApplicationRepository appRepo)
        {
            this.appRepo = appRepo;
        }

        public void Handle(SAHL.Common.Collections.Interfaces.IDomainMessageCollection messages, IsFurtherLoanCommand command)
        {

            appRepo.GetApplicationByAccountKey(command.ApplicationKey);
            throw new NotImplementedException();
        }
    }
}
