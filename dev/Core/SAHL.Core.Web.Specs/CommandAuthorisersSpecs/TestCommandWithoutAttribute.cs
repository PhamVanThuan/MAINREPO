using SAHL.Core.Services;
using System;

namespace SAHL.Core.Web.Specs.CommandAuthorisersSpecs
{
    public class TestCommandWithoutAttribute : IServiceCommand
    {
        public Guid Id
        {
            get;
            set;
        }
    }
}