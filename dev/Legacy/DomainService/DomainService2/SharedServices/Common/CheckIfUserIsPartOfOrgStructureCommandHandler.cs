using System.Data;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class CheckIfUserIsPartOfOrgStructureCommandHandler : IHandlesDomainServiceCommand<CheckIfUserIsPartOfOrgStructureCommand>
    {
        private ICastleTransactionsService castleTransactionsService;

        public CheckIfUserIsPartOfOrgStructureCommandHandler(ICastleTransactionsService castleTransactionsService)
        {
            this.castleTransactionsService = castleTransactionsService;
        }

        public void Handle(IDomainMessageCollection messages, CheckIfUserIsPartOfOrgStructureCommand command)
        {
            string query = string.Format(@"with tmpOS (OrganisationStructureKey)
                                            AS(
	                                            select OS.OrganisationStructureKey
                                                from [2am].dbo.OrganisationStructure OS (NOLOCK)
	                                            where OS.ParentKey in ({0})
	                                            UNION ALL
	                                            select OS.OrganisationStructureKey
	                                            from [2am].dbo.OrganisationStructure OS (NOLOCK)
	                                            join tmpOS
	                                            on tmpOS.OrganisationStructureKey=OS.ParentKey
                                            )
                                            select tmpOS.*, OS.Description, OS.ParentKey
                                            from tmpOS
                                            inner join [2am].dbo.OrganisationStructure OS (NOLOCK)
                                            on OS.OrganisationStructureKey=tmpOS.OrganisationStructureKey
                                            inner join [2am].dbo.UserOrganisationStructure UOS (NOLOCK)
                                            on UOS.OrganisationStructureKey=OS.OrganisationStructureKey
                                            inner join [2am].dbo.ADUser A (NOLOCK)
                                            on A.ADUserKey = UOS.ADUserKey
                                            where A.ADUserName='{1}'", (int)command.OrganisationStructure, command.ADUserName);
            command.Result = false;
            DataSet ds = castleTransactionsService.ExecuteQueryOnCastleTran(query, SAHL.Common.Globals.Databases.X2, null);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    command.Result = (ds.Tables[0].Rows.Count > 0) ? true : false;
                }
            }
        }
    }
}