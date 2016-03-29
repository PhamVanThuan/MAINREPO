using SAHL.Core.IoC;
using SAHL.Core.Testing;
using StructureMap.Graph;
using System;
using System.Collections.Generic;

namespace SAHL.Core.Testing.Ioc.Registration
{
    public class LegacyEventConvention : IRegistrationConvention
    {
        // these are valid events that do not have queries.
        private readonly IList<string> exclusions = new List<string>() { "BusinessDayCompletedLegacyEvent", "BusinessMonthCompletedLegacyEvent", "ApplicationLoanAgreementAmountUpdatedLegacyEvent" };

        public void Process(Type type, StructureMap.Configuration.DSL.Registry registry)
        {
            if (type.Namespace == null)
            {
                return;
            }
            if (type.GetInterface("ILegacyEvent") != null && type.IsClass && type.Name != "LegacyEvent" && !exclusions.Contains(type.Name))
            {
                registry.For(type).Use(type);
            }
        }
    }
}