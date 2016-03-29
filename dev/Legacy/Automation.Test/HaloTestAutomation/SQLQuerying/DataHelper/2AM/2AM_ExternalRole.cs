using Common.Enums;
using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Get all external roles by generickey and type indicating whether the fullname should be retrieved or not.
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.ExternalRole> GetExternalRoles(int genericKey, GenericKeyTypeEnum genericKeyType, bool isfullLegalName = false)
        {
            int getfullLegalName = isfullLegalName == false ? 1 : 0;
            string query =
                string.Format(@"select *, dbo.legalentitylegalname(er.legalentitykey,{0}) as LegalEntityLegalName, er.legalentitykey as LEKey, le.EmailAddress, le.IDNumber as IDNumber
								from [2am].dbo.externalRole er
								join [2am].dbo.legalEntity le on er.legalEntityKey=le.legalEntityKey
								where er.genericKey = {1} and er.genericKeyTypeKey = {2}", getfullLegalName, genericKey, (int)genericKeyType);
            return dataContext.Query<Automation.DataModels.ExternalRole>(query);
        }

        /// <summary>
        /// Get all external roles by legalentity indicating whether the fullname should be retrieved or not.
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.ExternalRole> GetExternalRoles(int legalEntityKey, bool isfullLegalName = false)
        {
            int getfullLegalName = isfullLegalName == false ? 1 : 0;
            string query =
                string.Format(@"select *, dbo.legalentitylegalname(er.legalentitykey,{0}) as LegalEntityLegalName, er.legalentitykey as LEKey, le.EmailAddress
								from [2am].dbo.externalRole er
								join [2am].dbo.legalEntity le on er.legalEntityKey=le.legalEntityKey
								where le.legalentitykey = {1}
                                order by er.genericKey desc", getfullLegalName, legalEntityKey);
            return dataContext.Query<Automation.DataModels.ExternalRole>(query);
        }

        /// <summary>
        /// Removes all of the external roles of the type against the genericKey
        /// </summary>
        /// <param name="genericKey">genericKey</param>
        /// <param name="genericKeyType">genericKeyType</param>
        /// <param name="externalRoleType">externalRoleType</param>
        public void DeleteExternalRole(int genericKey, GenericKeyTypeEnum genericKeyType, ExternalRoleTypeEnum externalRoleType)
        {
            var q =
                string.Format(@"delete from [2am].dbo.ExternalRole where GenericKey={0} and GenericKeyTypeKey={1}
								and externalRoleTypeKey={2}", genericKey, (int)genericKeyType, (int)externalRoleType);
            SQLStatement s = new SQLStatement { StatementString = q };
            dataContext.ExecuteNonSQLQuery(s);
        }

        /// <summary>
        /// Gets the correspondence details of the external role on a case
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <param name="externalRoleType">externalRoleType</param>
        /// <returns></returns>
        public QueryResults GetExternalRoleCorrespondenceDetails(int debtCounsellingKey, ExternalRoleTypeEnum externalRoleType)
        {
            string query = string.Format(@"select le.LegalEntityKey, dbo.LegalEntityLegalName(le.legalentitykey, 1) as LegalEntityLegalName, le.FaxCode, le.FaxNumber, le.EmailAddress,
												[2AM].[dbo].[fGetFormattedAddressDelimited] ( lea.addresskey, 0)
										   from [2am].dbo.externalrole er
												join [2am].dbo.legalentity le on er.legalentitykey = le.legalentitykey
												left join [2am].dbo.legalentityaddress lea on le.legalentitykey = lea.legalentitykey
										   where generickey = {0}
												and er.generickeytypekey = 27
												and er.externalroletypekey = {1}
												and er.generalstatuskey = 1", debtCounsellingKey, (int)externalRoleType);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Inserts an External Role Record
        /// </summary>
        /// <param name="genericKey">GenericKey</param>
        /// <param name="genericKeyTypeKey">genericKeyTypeKey</param>
        /// <param name="legalEntityKey">legalEntityKey</param>
        /// <param name="externalRoleTypeKey">externalRoleTypeKey</param>
        /// <param name="status">GeneralStatusKey</param>
        public void InsertExternalRole(int genericKey, GenericKeyTypeEnum genericKeyTypeKey, int legalEntityKey, ExternalRoleTypeEnum externalRoleTypeKey,
                GeneralStatusEnum status)
        {
            string query =
                string.Format(@"insert into [2am].dbo.ExternalRole
								(GenericKey, GenericKeyTypeKey, LegalEntityKey, ExternalRoleTypeKey, GeneralStatusKey, ChangeDate)
								values
								({0},{1},{2},{3},{4},getdate())", genericKey, (int)genericKeyTypeKey, legalEntityKey, (int)externalRoleTypeKey,
                                                                    (int)status);
            SQLStatement s = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(s);
        }
    }
}