using System;
using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        ///   This will retrieve a record from the 2am Correspondence table
        /// </summary>
        /// <param name = "genericKey">The GenericKey to use i.e.Account/Offer</param>
        /// <param name = "reportName">The name of the report (ReportStatement.Description)</param>
        /// <param name = "sendMethod">The method used to send the report (CorrespondenceMedium.Description)</param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.Correspondence> GetCorrespondenceRecordsByGenericKeyAndReportStatement(int genericKey, string reportName, string sendMethod)
        {
            string query =
                string.Format(
                    @"select c.*, le.legalEntityKey, cm.description as CorrespondenceMedium, rm.description as ReportDescription,
                                    substring(outputfile, charindex('{0}',outputfile, 0), len(outputfile)) as IDMGuid,c.DestinationValue
                                    from [2am].dbo.correspondence c with (nolock)
                                    join [2am].dbo.correspondencemedium cm with (nolock)
                                    on c.correspondencemediumkey=cm.correspondencemediumkey
                                    join [2am].dbo.reportstatement rm with (nolock) on c.reportstatementkey=rm.reportstatementkey
                                    left join [2am].dbo.legalEntity le with (nolock) on c.legalEntityKey = le.legalEntityKEy
                                    where generickey= {1} and cm.description = '{2}' and rm.description = '{3}'
                                    and datediff(mi, c.ChangeDate, getdate()) < 5
                                    order by ChangeDate desc", '{', genericKey, sendMethod, reportName);
            return dataContext.Query<Automation.DataModels.Correspondence>(query);
        }

        /// <summary>
        /// Queries ImageIndex..Data
        /// </summary>
        /// <param name="guid">Report guid</param>
        /// <returns></returns>
        public QueryResults GetImageIndexData(string guid)
        {
            string query =
                string.Format(@"select * from [imageindex].dbo.data where GUID like '%{0}%'", guid);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns the IDM guid.
        /// </summary>
        /// <param name="genericKey"></param>
        /// <param name="reportName"></param>
        /// <param name="sendMethod"></param>
        /// <returns></returns>
        public string GetCorrespondenceGUID(string genericKey, string reportName, string sendMethod)
        {
            string query =
                string.Format(@"select
                                substring(outputfile, charindex('{0}',outputfile, 0), len(outputfile)) as idmguid
                                from [2am].dbo.correspondence c
                                join [2am].dbo.correspondencemedium cm with (nolock)
                                on c.correspondencemediumkey=cm.correspondencemediumkey
                                join [2am].dbo.reportstatement rm with (nolock) on c.reportstatementkey=rm.reportstatementkey
                                where generickey={1}
                                and cm.description = '{2}'
                                and rm.description = '{3}'
                                order by ChangeDate desc", "{", genericKey, sendMethod, reportName);
            SQLStatement statement = new SQLStatement { StatementString = query };
            var r = dataContext.ExecuteSQLQuery(statement);
            return r.Rows(0).Column("idmguid").GetValueAs<string>();
        }

        public IEnumerable<Automation.DataModels.CorrespondenceTemplate> GetCorrespondenceTemplate()
        {
            return dataContext.Query<Automation.DataModels.CorrespondenceTemplate>(string.Format(@"Select * from [2am].dbo.CorrespondenceTemplate"));
        }

        public void UpdateDefaultEmailAddress(int correspondenceTemplateKey, string emailAddress)
        {
            dataContext.Execute(String.Format(@"update dbo.correspondencetemplate set defaultemail = '{0}' where correspondencetemplatekey = {1} ", emailAddress, correspondenceTemplateKey)); ;
        }
    }
}