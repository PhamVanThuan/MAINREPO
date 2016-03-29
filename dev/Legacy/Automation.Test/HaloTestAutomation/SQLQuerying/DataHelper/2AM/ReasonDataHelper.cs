using Common.Enums;
using System.Collections.Generic;
using Automation.DataAccess.DataHelper._2AM.Contracts;
using Automation.DataAccess.Interfaces;

namespace Automation.DataAccess.DataHelper
{
    public class ReasonDataHelper : IReasonDataHelper
    {
        private IDataContext dataContext;
        public ReasonDataHelper(IDataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        /// <summary>
        ///   This returns a set of reasons when provided with the reason type
        /// </summary>
        /// <param name = "ReasonType">ReasonType.Description</param>
        /// <returns></returns>
        public QueryResults GetActiveReasonsByReasonType(string ReasonType)
        {
            string query = string.Format(@"select rdesc.description, EnforceComment, AllowComment, rdesc.ReasonDescriptionKey
                            from [2am].dbo.reasontype rt with (nolock)
                            join [2am].dbo.reasontypegroup rtg with (nolock)  on rt.reasontypegroupkey=rtg.reasontypegroupkey
                            join [2am].dbo.reasondefinition rd with (nolock)  on rt.reasontypekey=rd.reasontypekey
                            join [2am].dbo.reasondescription rdesc with (nolock)  on rd.reasondescriptionkey=rdesc.reasondescriptionkey
                            where rt.description='{0}' and rd.generalstatuskey=1
                            order by reasondefinitionkey asc", ReasonType);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///   Retrieves the Reasons linked to a Generic Key when provided with the Generic Key value and the Generic Key Type
        /// </summary>
        /// <param name = "genericKey">GenericKey</param>
        /// <param name = "genericKeyType">GenericKeyType (OfferKey=2, OfferInformationKey=5, AccountKey=1)</param>
        /// <returns>r.GenericKey, ReasonDescription.Description as ReasonDescription, ReasonType.Description as ReasonTypeDescription,
        ///   GenericKeyTypeKey
        /// </returns>
        public QueryResults GetReasonsByGenericKeyAndGenericKeyType(int genericKey, GenericKeyTypeEnum genericKeyType)
        {
            string query = string.Format(@"select r.GenericKey, rde.description as ReasonDescription,
                           rt.description as ReasonTypeDescription, rt.GenericKeyTypeKey, r.Comment
                           from [2am].dbo.reason r with (nolock)
                           join [2am].dbo.reasondefinition rd with (nolock) on r.reasondefinitionkey=rd.reasondefinitionkey
                           join [2am].dbo.reasondescription rde with (nolock) on rd.reasondescriptionkey=rde.reasondescriptionkey
                           join [2am].dbo.reasontype rt with (nolock) on rd.reasontypekey=rt.reasontypekey
                           where generickey={0} and GenericKeyTypeKey = {1}", genericKey, (int)genericKeyType);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// This will remove all of the reasons of a particular type against a generic key
        /// </summary>
        /// <param name="genericKey">genericKey</param>
        /// <param name="genericKeyType">genericKeyType i.e. debtCounsellingKey/OfferKey</param>
        /// <param name="reasonTypeKey">reasonTypeKey</param>
        public void RemoveReasonsAgainstGenericKeyByReasonType(int genericKey, GenericKeyTypeEnum genericKeyType, ReasonTypeEnum reasonTypeKey)
        {
            string q =
                string.Format(@"delete r
                                from [2am].dbo.Reason r
                                join [2am].dbo.ReasonDefinition rd
                                on r.reasonDefinitionKey=rd.reasonDefinitionKey
                                join [2am].dbo.ReasonType rt on rd.reasonTypeKey=rt.reasonTypeKey
                                where r.GenericKey={0}
                                and GenericKeyTypeKey={1}
                                and rt.reasonTypeKey={2}", genericKey, (int)genericKeyType, (int)reasonTypeKey);
            SQLStatement s = new SQLStatement { StatementString = q };
            dataContext.ExecuteNonSQLQuery(s);
        }

        /// <summary>
        /// Inserts a reason against a GenericKey if the reason type matches the GenericKeyType provided. i.e. It will not allow reason types to be associated to an
        /// incorrect genericKey.
        /// </summary>
        /// <param name="genericKey">genericKey</param>
        /// <param name="reasonDescription">reasonDescription</param>
        /// <param name="reasonType">reasonType</param>
        /// <param name="genericKeyType">genericKeyType i.e. debtCounsellingKey/OfferKey</param>
        public void InsertReason(int genericKey, string reasonDescription, ReasonTypeEnum reasonType, GenericKeyTypeEnum genericKeyType)
        {
            string q =
                string.Format(@"insert into [2am].dbo.Reason (
                                ReasonDefinitionKey, GenericKey, Comment, StageTransitionKey
                                )
                                select reasonDefinitionKey, {0}, '',  NULL
                                from [2am].dbo.reasondescription rd
                                join [2am].dbo.reasondefinition rde on rd.reasondescriptionKey=rde.reasondescriptionKey
                                join [2am].dbo.reasonType rt on rde.reasontypekey=rt.reasontypekey and rt.generickeytypekey={3}
                                where rd.description = '{1}' and rde.reasonTypeKey={2}", genericKey, reasonDescription, (int)reasonType, (int)genericKeyType);
            SQLStatement s = new SQLStatement { StatementString = q };
            bool b = dataContext.ExecuteNonSQLQuery(s);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reasonDefinition"></param>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public QueryResults GetNotificationReasonsForLegalEntity(int reasonDefinition, int accountKey)
        {
            string q =
                string.Format(@"select le.FirstNames + ' ' + le.Surname as Displayname,
                                case    when re.ReasonKey is null then 0
                                else 1  end as NotificationExists
                                from [2am].dbo.[Role] r (nolock)
                                join [2am].dbo.LegalEntity le (nolock) on
                                r.LegalEntityKey = le.LegalEntityKey
                                left join [2am].dbo.Reason re (nolock)
                                on le.LegalEntityKey = re.GenericKey
                                and re.ReasonDefinitionKey = {0}
                                where r.RoleTypeKey in (2, 3)
                                and r.GeneralStatusKey = 1
                                and le.LegalEntityTypeKey = 2
                                and le.LegalEntityStatusKey != 3
                                and r.AccountKey = {1} order by 1", reasonDefinition, accountKey);
            SQLStatement s = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(s);
        }

        /// <summary>
        /// Returns a reason definition when provided with the description and type
        /// </summary>
        /// <param name="reasonDescription"></param>
        /// <param name="reasonType"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.ReasonDefinition> GetReasonDefinition(string reasonDescription, ReasonTypeEnum reasonType)
        {
            var query =
                string.Format(@"select rd.ReasonDefinitionKey, rt.ReasonTypeKey, rt.Description as ReasonTypeDescription,
                                rtg.ReasonTypeGroupKey, EnforceComment, rd.GeneralStatusKey,
                                GenericKeyTypeKey
                                from [2am].dbo.reasondefinition rd
                                join [2am].dbo.reasonType rt on rd.reasonTypeKey=rt.reasonTypeKey
                                join [2am].dbo.reasonDescription rdesc on rd.reasonDescriptionKey=rdesc.reasonDescriptionKey
                                join [2am].dbo.reasonTypeGroup rtg on rt.reasonTypeGroupKey=rtg.reasonTypeGroupKey
                                where rdesc.description = '{0}' and rt.reasonTypeKey = {1}", reasonDescription, (int)reasonType);
            return dataContext.Query<Automation.DataModels.ReasonDefinition>(query);
        }

        public void SetStageTransitionOnLatestReason(int genericKeyStageTransition, int genericKeyReason, StageDefinitionStageDefinitionGroupEnum sdsdgKey)
        {
            string sql = string.Format(@"select top 1 stageTransitionKey from [2am].dbo.StageTransition where genericKey = {0} and stageDefinitionStageDefinitionGroupKey = {1}
                                        order by stageTransitionKey desc", genericKeyStageTransition, (int)sdsdgKey);
            var statement = new SQLStatement { StatementString = sql };
            var results = dataContext.ExecuteSQLQuery(statement);
            int stageTransitionKey = results.Rows(0).Column("stageTransitionKey").GetValueAs<int>();
            sql = string.Format(@"  update [2am].dbo.Reason set stageTransitionKey = {0}
                                    where reasonKey = (select top 1 ReasonKey from [2am].dbo.Reason where genericKey = {1} order by ReasonKey desc)", stageTransitionKey, genericKeyReason);
            statement = new SQLStatement { StatementString = sql };
            dataContext.ExecuteNonSQLQuery(statement);
        }
    }
}