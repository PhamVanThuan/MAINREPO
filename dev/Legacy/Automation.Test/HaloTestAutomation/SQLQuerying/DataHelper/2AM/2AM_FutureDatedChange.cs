using System.Collections.Generic;
using System.Linq;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Gets all of the Future Dated Changes linked to the identifier reference provided.
        /// </summary>
        /// <param name="identifierReference"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.FutureDatedChange> GetFutureDatedChangeByIdentifierReference(int identifierReference)
        {
            string query = string.Format(@"select *,FutureDatedChangeTypeKey as FutureDatedChangeType
                        from [2am].dbo.FutureDatedChange where identifierReferenceKey = {0}", identifierReference);
            var futureDatedChanges = dataContext.Query<Automation.DataModels.FutureDatedChange>(query);
            foreach (var item in futureDatedChanges)
            {
                item.FutureDatedChangeDetails = this.GetFutureDatedChangeDetail(item.FutureDatedChangeKey);
            }
            return futureDatedChanges;
        }

        /// <summary>
        /// Gets the details of the Future Dated Change from the FutureDatedChangeDetail
        /// </summary>
        /// <param name="futureDatedChangeKey"></param>
        /// <returns></returns>
        public List<Automation.DataModels.FutureDatedChangeDetail> GetFutureDatedChangeDetail(int futureDatedChangeKey)
        {
            string query =
                    string.Format(@"select * from [2am].dbo.FutureDatedChangeDetail where FutureDatedChangeKey = {0}", futureDatedChangeKey);
            return dataContext.Query<Automation.DataModels.FutureDatedChangeDetail>(query).ToList();
        }

        /// <summary>
        /// Removes any future dated changes against the identifier reference
        /// </summary>
        /// <param name="identifierReference"></param>
        public void DeleteFutureDatedChangeByIdentifierReference(int identifierReference)
        {
            string sql = string.Format(@"delete from [2am].dbo.futureDatedChangeDetail
                            where futureDatedChangeKey in (
                            select futureDatedChangeKey
                            from [2am].dbo.futureDatedChange
                            where identifierReferenceKey = {0}
                            and effectiveDate > getdate()
                            )
                            delete from [2am].dbo.futureDatedChange
                            where identifierReferenceKey = {1}
                            and effectiveDate > getdate()", identifierReference, identifierReference);
            var statement = new SQLStatement { StatementString = sql };
            dataContext.ExecuteNonSQLQuery(statement);
        }
    }
}