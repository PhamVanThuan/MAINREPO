using System;
using System.Data;
using System.Configuration;
using System.Web;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common;
using SAHL.Common.Security;
using System.Security.Principal;
using SAHL.Common.DataAccess;
using System.Data.SqlClient;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.DAO;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Common.BusinessModel.Rules.LegalEntityLogin
{
    /// <summary>
    /// Legal Entity Login Duplicate Username
    /// </summary>
    [RuleDBTag("LegalEntityLoginDuplicateUsername",
    "Ensures that there are no duplicate Legal Entity Login Username's",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.LegalEntityLogin.LegalEntityLoginDuplicateUsername")]
    [RuleInfo]
    public class LegalEntityLoginDuplicateUsername : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            ILegalEntityRepository legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();

            ILegalEntityLogin legalEntityLogin = Parameters[0] as ILegalEntityLogin;

            ILegalEntityLogin foundLegalEntityLogin = legalEntityRepository.GetLegalEntityLogin(legalEntityLogin.Username);

            if (foundLegalEntityLogin != null)
            {
                string errorMessage = "A user with that username already exists.";
                AddMessage(errorMessage, errorMessage, Messages);
                return 0;
            }

            return 1;
        }
    }
}
