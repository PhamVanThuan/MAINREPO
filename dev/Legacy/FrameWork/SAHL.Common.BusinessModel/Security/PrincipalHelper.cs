using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Security;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Caching;

namespace SAHL.Common.BusinessModel.Security
{
    public class PrincipalHelper
    {
        /// <summary>
        /// Initilaises the user security access.  This method is called when the application initialises, and will 
        /// internally also call RefreshUserOrgStructure
        /// </summary>
        /// <param name="principal"></param>
        public static void InitialisePrincipal(SAHLPrincipal principal)
        {
            // get the ADUser
            SimpleQuery<ADUser_DAO> ASQ = new SimpleQuery<ADUser_DAO>("from ADUser_DAO AD where ADUserName = ?", principal.Identity.Name);
            ADUser_DAO[] ADs = ASQ.Execute();
            if (ADs.Length == 1)
            {
                principal.ADUser = new ADUser(ADs[0]);
            }

            RefreshPrincipalOrgStructure(principal);
        }

        /// <summary>
        /// Refreshes the principal's organisation structure settings which reside in memory.
        /// </summary>
        /// <param name="principal"></param>
        public static void RefreshPrincipalOrgStructure(SAHLPrincipal principal)
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            IDomainMessageCollection dmc = new DomainMessageCollection();
            IEventList<IOrganisationStructureOriginationSource> orgStructOrigSources = spc.UserOriginationSources;

            // clear out the existing collection
            while (orgStructOrigSources.Count > 0)
                orgStructOrigSources.RemoveAt(dmc, 0);

            // filter the organisational structure
            IList<IUserOrganisationStructure> UOS = new List<IUserOrganisationStructure>();
            SimpleQuery<OrganisationStructureOriginationSource_DAO> OSOS = new SimpleQuery<OrganisationStructureOriginationSource_DAO>(QueryLanguage.Sql,
                    @"with OSTopLevels (OrganisationStructureKey, ParentKey)
                    as
                    (
	                    select OS.OrganisationStructureKey, OS.ParentKey
	                    from 
		                    OrganisationStructure OS
	                    inner join
		                    UserOrganisationStructure UOS
	                    on
		                    OS.OrganisationStructureKey = UOS.OrganisationStructureKey
	                    inner join
		                    ADUser A
	                    on
		                    A.ADUserKey = UOS.ADUserKey
	                    where 
		                    A.ADUserName = ?
                    UNION ALL
	                    select OS.OrganisationStructureKey, OS.ParentKey
	                    from 
		                    OrganisationStructure OS
	                    join
		                    OSTopLevels
	                    on
		                    OS.OrganisationStructureKey = OSTopLevels.ParentKey
                    )
                    select distinct OSOS.* from OSTopLevels
                    inner join
	                    OrganisationStructureOriginationSource OSOS
                    on
	                    OSOS.OrganisationStructureKey = OSTopLevels.OrganisationStructureKey", principal.Identity.Name);
            OSOS.AddSqlReturnDefinition(typeof(OrganisationStructureOriginationSource_DAO), "OSOS");
            OrganisationStructureOriginationSource_DAO[] OSOSs = OSOS.Execute();
            for (int i = 0; i < OSOSs.Length; i++)
                orgStructOrigSources.Add(dmc, new OrganisationStructureOriginationSource(OSOSs[i]));
        }

        public static void DisposePrincipal(SAHLPrincipal principal)
        {
        }
    }
}
