using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Services.ApplicationDomain.Managers.Affordability.Statements
{
  public class DoesAffordabilityRequireDescriptionStatement : ISqlStatement<bool>
    {
      public AffordabilityType AffordabilityType { get; protected set; }

        public DoesAffordabilityRequireDescriptionStatement(AffordabilityType affordabilityType)
        {
            this.AffordabilityType = affordabilityType;
        }

        public string GetStatement()
        {
            return @"SELECT TOP 01 DescriptionRequired FROM [2AM].[dbo].[AffordabilityType] WHERE AffordabilityTypeKey = @AffordabilityType";
        }

    }
}
