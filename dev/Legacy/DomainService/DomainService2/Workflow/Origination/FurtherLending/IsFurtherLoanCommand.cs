// -----------------------------------------------------------------------
// <copyright file="IsFurtherLoanCommand.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DomainService2.Workflow.Origination.FurtherLending
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class IsFurtherLoanCommand : IDomainServiceCommand
    {
        public IsFurtherLoanCommand(int applicationKey)
        {
            this.ApplicationKey = applicationKey;
        }

        public int ApplicationKey { get; protected set; }
    }
}
