using Common.Enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Returns a legal entity key and legal entity name for a legal entity linked one level down from the root node description.
        /// </summary>
        /// <param name="rootNodeDescription"></param>
        /// <param name="legalEntityKeyToExclude"></param>
        /// <returns></returns>
        public QueryResults GetLegalEntityFromLEOS(string rootNodeDescription, int legalEntityKeyToExclude = 0)
        {
            var q = string.Empty;
            switch (rootNodeDescription)
            {
                default:
                    q =
                        string.Format(@"select top 1 leos.*, dbo.legalentitylegalname(leos.legalentityKey, 1) as Name
                                from [2am].dbo.legalEntityOrganisationStructure leos with (nolock)
                                join [2am].dbo.organisationStructure os with (nolock)
                                on leos.organisationStructureKey=os.organisationStructureKey
                                join [2am].dbo.organisationStructure os_parent with (nolock)
                                on os.parentKey=os_parent.organisationStructureKey
                                where os_parent.Description='{0}'
                                and leos.legalentityKey <> {1} order by 1 desc", rootNodeDescription, legalEntityKeyToExclude);
                    break;

                case "Debt Counsellors":
                    q = string.Format(@"select top 1 leos.*, dbo.legalentitylegalname(leos.legalentityKey, 1) as Name,
                                        dc.NCRDCRegistrationNumber as RegNumber
                                        from [2am].dbo.legalEntityOrganisationStructure leos with (nolock)
                                        join [2am].dbo.organisationStructure os with (nolock)
                                        on leos.organisationStructureKey=os.organisationStructureKey
                                        join [2am].dbo.organisationStructure os_parent with (nolock)
                                        on os.parentKey=os_parent.organisationStructureKey
                                        join debtcounselling.debtCounsellorDetail dc on leos.legalentitykey=dc.legalentitykey
                                        where os_parent.Description='Debt Counsellors' and len(NCRDCRegistrationNumber) > 0
                                        and leos.legalentityKey <> {0} order by newid()", legalEntityKeyToExclude);
                    break;

                case "Payment Distribution Agencies":
                    q = string.Format(@"select top 1 leos.*, dbo.legalentitylegalname(leos.legalentityKey, 1) as Name
                                from [2am].dbo.legalEntityOrganisationStructure leos with (nolock)
                                join [2am].dbo.organisationStructure os with (nolock)
                                on leos.organisationStructureKey=os.organisationStructureKey
                                join organisationStructure os_parent with (nolock)
                                on os.parentKey=os_parent.organisationStructureKey
                                join legalentity le with (nolock) on leos.legalentitykey = le.legalentitykey
									and tradingName is null
                                where os_parent.Description='{0}'
                                and leos.legalentityKey <> {1} order by 1 desc", rootNodeDescription, legalEntityKeyToExclude);
                    break;
            }

            SQLStatement statement = new SQLStatement { StatementString = q };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="legalentitykey"></param>
        /// <returns></returns>
        public IEnumerable<Automation.DataModels.OrganisationStructure> GetOrganisationStructure(int legalentitykey)
        {
            var query = String.Format(@"select os.*, ot.Description as OrganisationTypeDescription
                                            from [2am].dbo.LegalEntityOrganisationStructure leos
                                            inner join [2am].dbo.OrganisationStructure os on leos.OrganisationStructureKey = os.OrganisationStructureKey
	                                        inner join [2am].dbo.OrganisationType ot on os.OrganisationTypeKey = ot.OrganisationTypeKey
                                            where leos.LegalEntityKey = {0}", legalentitykey);
            return dataContext.Query<Automation.DataModels.OrganisationStructure>(query);
        }

        public void InsertLegalEntityOrganisationStructure(string name, LegalEntityTypeEnum leType, string idNumber_registrationnumber, int parentKey)
        {
            var proc = new SQLStoredProcedure { Name = "test.CreateLegalEntityOrganisationStructure" };
            proc.AddParameter(new SqlParameter("@LENames", name));
            proc.AddParameter(new SqlParameter("@IdNumber_RegistrationNumber", idNumber_registrationnumber));
            proc.AddParameter(new SqlParameter("@LegalEntityType", (int)leType));
            proc.AddParameter(new SqlParameter("@ParentKey", parentKey));
            dataContext.ExecuteStoredProcedure(proc);
        }

        public void DeleteLegalEntityOrganisationStructure(string firstnames_registeredname)
        {
            var proc = new SQLStoredProcedure { Name = "test.LegalEntityOrganisationStructureCleanup" };
            proc.AddParameter(new SqlParameter("@firstnames_registeredname", firstnames_registeredname));
            dataContext.ExecuteStoredProcedure(proc);
        }
    }
}