namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// This will get the clientemail records queued for sending an email or sms in the sahldb.dbo.clientemail
        /// </summary>
        /// <param name="accountkey"></param>
        /// <returns></returns>
        public QueryResults GetClientCommunication(Common.Enums.ContentTypeEnum contentType, int accountkey)
        {
            var sql =
                 string.Format(@"select top 10 * from sahldb.dbo.clientemail
                                 where loannumber = {0} and contenttypekey = {1}", accountkey, (int)contentType);
            SQLStatement statement = new SQLStatement { StatementString = sql };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="emailSubject"></param>
        /// <returns></returns>
        public QueryResults GetClientEmailByToAddressAndSubject(string emailTo, string emailSubject, string date)
        {
            var sql =
                string.Format(@"select top 1 * from [sahldb].dbo.ClientEmail where EmailTo='{0}' and EmailSubject='{1}'
                                and emailInsertDate >= '{2}' order by 1 desc",
                                    emailTo, emailSubject, date);
            SQLStatement statement = new SQLStatement { StatementString = sql };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Selects the first client email record that matches the parameters provided.
        /// </summary>
        /// <param name="emailTo">ClientEmail.emailTo column</param>
        /// <param name="emailSubject">ClientEmail.emailSubject column/param>
        /// <param name="date">ClientEmail.date column</param>
        /// <param name="genericKey">ClientEmail.genericKey column</param>
        /// <param name="emailBody">ClientEmail.emailBody column</param>
        /// <returns></returns>
        public QueryResults GetClientEmail(string emailTo, string emailSubject, string date, int genericKey, string emailBody)
        {
            var sql =
                string.Format(@"select top 1 * from [sahldb].dbo.ClientEmail where EmailTo='{0}' and EmailSubject='{1}'
                                and emailInsertDate >= '{2}' and LoanNumber = {3} and emailBody='{4}' order by 1 desc",
                                    emailTo, emailSubject, date, genericKey, emailBody);
            SQLStatement statement = new SQLStatement { StatementString = sql };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="emailTo"></param>
        /// <param name="emailSubject"></param>
        /// <returns></returns>
        public QueryResults GetClientEmail(string emailTo)
        {
            var query =
                string.Format(@"select top 1 * from [sahldb].dbo.ClientEmail where EmailTo='{0}' order by 1 desc", emailTo);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        public QueryResults GetClientEmailSMS(string emailBody, string cellPhone, int genericKey)
        {
            var query =
                string.Format(@"SELECT * FROM [2am].dbo.ClientEmail
                    WHERE EmailBody = '{0}' AND EmailAttachment1 = 'SMS' AND Cellphone = '{1}' AND AccountKey = {2}
                    ", emailBody, cellPhone, genericKey);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }
    }
}