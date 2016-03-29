using SAHL.Core.Data.Models._2AM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Extensions;

namespace SAHL.Services.ClientDomain.Utils
{
    public class LegalEntitySanitiser
    {
        public static void Sanitise(LegalEntityDataModel legalEntity)
        {
            legalEntity.FirstNames = string.IsNullOrEmpty(legalEntity.FirstNames) ? legalEntity.FirstNames : legalEntity.FirstNames.ConvertToCamelCase();
            legalEntity.Surname = string.IsNullOrEmpty(legalEntity.Surname) ? legalEntity.Surname : legalEntity.Surname.ConvertToCamelCase();
            legalEntity.PreferredName = string.IsNullOrEmpty(legalEntity.PreferredName) ? legalEntity.PreferredName : legalEntity.PreferredName.ConvertToCamelCase();

            legalEntity.RegisteredName = string.IsNullOrEmpty(legalEntity.RegisteredName) ? legalEntity.RegisteredName : legalEntity.RegisteredName.ConvertToCamelCase();
            legalEntity.TradingName = string.IsNullOrEmpty(legalEntity.TradingName) ? legalEntity.TradingName : legalEntity.TradingName.ConvertToCamelCase();
        }
    }
}
