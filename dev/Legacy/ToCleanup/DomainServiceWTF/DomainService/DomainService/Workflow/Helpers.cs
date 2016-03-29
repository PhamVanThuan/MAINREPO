using System;
using System.Collections.Generic;
using System.Text;
using SAHL.X2.Framework.DataAccess;
using System.Data;
using SAHL.Common.Factories;
using System.Data.SqlClient;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.X2.Common.DataAccess;
using Castle.ActiveRecord;
using SAHL.X2.Common;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
//using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.DAO;

namespace DomainService.Workflow
{
    internal class Helpers
    {
        /// <summary>
        /// Will recurse through all groups that fall under a given orgnaisationstructure and
        /// check if the aduser passed in is a member of any groups or subgroups or subgroups (recurse)
        /// </summary>
        /// <param name="OrgStructureKey">OrgStructureKey of the org structure you want to use as the top level
        /// parent</param>
        /// <param name="ADUserName">ADUserName to check membership</param>
        /// <param name="Conn"></param>
        /// <returns>True / False</returns>
        public bool CheckIfUserIsPartOfOrgStructure(int OrgStructureKey, string ADUserName, IActiveDataTransaction Tran)
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("with tmpOS (OrganisationStructureKey)");
            sb.AppendLine("AS");
            sb.AppendLine("(");
            sb.AppendLine("select OS.OrganisationStructureKey");
            sb.AppendLine("from [2am].dbo.OrganisationStructure OS");
            sb.AppendFormat("where OS.ParentKey in ({0}) ", OrgStructureKey);
            sb.AppendLine();
            sb.AppendLine("UNION ALL	");
            sb.AppendLine("select OS.OrganisationStructureKey");
            sb.AppendLine("from [2am].dbo.OrganisationStructure OS");
            sb.AppendLine("join");
            sb.AppendLine("tmpOS");
            sb.AppendLine("on");
            sb.AppendLine("tmpOS.OrganisationStructureKey=OS.ParentKey");
            sb.AppendLine(")");
            sb.AppendLine("select tmpOS.*");
            sb.AppendLine(", OS.Description, OS.ParentKey");
            sb.AppendLine(", A.AdUSerKey, A.ADUserName  ");
            sb.AppendLine("from tmpOS");
            sb.AppendLine("inner join");
            sb.AppendLine("	[2am].dbo.OrganisationStructure OS");
            sb.AppendLine("on");
            sb.AppendLine("OS.OrganisationStructureKey=tmpOS.OrganisationStructureKey");
            sb.AppendLine("inner join");
            sb.AppendLine("	[2am].dbo.UserOrganisationStructure UOS");
            sb.AppendLine("on");
            sb.AppendLine("UOS.OrganisationStructureKey=OS.OrganisationStructureKey");
            sb.AppendLine("inner join");
            sb.AppendLine("	[2am].dbo.ADUser A");
            sb.AppendLine("on");
            sb.AppendLine("A.ADUserKey = UOS.ADUserKey");
            sb.AppendLine("where");
            sb.AppendFormat("A.ADUserName='{0}'", ADUserName);
            //WorkerHelper.FillFromQuery(dt, sb.ToString(), Tran.Context, null);
            DataSet ds = ExecuteQueryOnCastleTran(sb.ToString(), typeof(Instance_DAO));
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                return (dt.Rows.Count > 0) ? true : false;
            }
            return false;
        }

        internal static void SetX2DataRow(long InstanceID, IDictionary<string, object> X2Data, IActiveDataTransaction Tran)
        {
            Instance_DAO instance = Instance_DAO.Find(InstanceID);

            string Query = "UPDATE X2.X2DATA.{0} SET ";
            string Where = " WHERE InstanceID = {1} ";

            foreach (KeyValuePair<string, object> KP in X2Data)
            {
                Query += (KP.Key + " = " + KP.Value+ ", ");
            }
            Query = Query.Substring(0, Query.Length - 2);
            Query += Where;
            Query = String.Format(Query, instance.WorkFlow.StorageTable, InstanceID);
            ExecuteNonQueryOnCastleTran(Query, typeof(Instance_DAO));
        }

        internal static IDictionary<string, object> GetX2DataRow(long InstanceID)
        {
            Instance_DAO instance = Instance_DAO.Find(InstanceID);

            string query = string.Format("SELECT * FROM X2DATA.{0} (nolock) WHERE InstanceID = {1}", instance.WorkFlow.StorageTable, InstanceID);
            DataTable DT = new DataTable();

            DataSet ds = ExecuteQueryOnCastleTran(query, typeof(Instance_DAO));
            if (ds.Tables.Count > 0)
            {
                DT = ds.Tables[0];
            }
            //WorkerHelper.FillFromQuery(DT, query, Tran.Context, new ParameterCollection());

            Dictionary<string, object> dict = new Dictionary<string, object>();

            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Columns.Count; i++)
                {
                    dict.Add(DT.Columns[i].ColumnName, DT.Rows[0].ItemArray[i]);
                }
            }

            return dict;
        }

        internal static DataSet ExecuteQuery(string Query, IActiveDataTransaction Tran)
        {
            DataSet ds = new DataSet();
            WorkerHelper.FillFromQuery(ds, "DATA", Query, Tran.Context, new ParameterCollection());
            return ds;
        }

        internal bool CheckLegalEntityQualifies(IAccountLifePolicy accountLifePolicy, ILegalEntityNaturalPerson legalEntityNaturalPerson, IActiveDataTransaction Tran)
        {
            ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();

            // 1. check the person is alive
            if (legalEntityNaturalPerson.LegalEntityStatus.Key == (int)SAHL.Common.Globals.LegalEntityStatuses.Deceased)
            {
                //spc.DomainMessages.Add(new Error("Assured Life cannot be Deceased", "Assured Life cannot be Deceased"));
                //args.Cancel = true;
                return false;
            }

            // 2. check the age between 18 & 65
            int minAge = Convert.ToInt32(lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.AssuredLifeMinAge].ControlNumeric);
            int maxAge = Convert.ToInt32(lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.AssuredLifeMaxAge].ControlNumeric);
            if (legalEntityNaturalPerson.AgeNextBirthday < minAge || legalEntityNaturalPerson.AgeNextBirthday > maxAge)
            {
                //spc.DomainMessages.Add(new Error("Assured Life must be between the ages of " + minAge + " and " + maxAge, "Assured Life must be between the ages of " + minAge + " and " + maxAge));
                //args.Cancel = true;
                return false;
            }

            // 3. check the group exposure (cannot have more than 2 life policies)
            int maxPolicies = Convert.ToInt32(lookupRepo.Controls.ObjectDictionary[SAHL.Common.Constants.ControlTable.Life.GroupExposureMaxPolicies].ControlNumeric);
            string query = SAHL.Common.DataAccess.UIStatementRepository.GetStatement("COMMON", "LifeIsOverExposed");
            query = query.Replace("@LegalentityKey", legalEntityNaturalPerson.Key.ToString());
            
            DataSet ds = ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO));
            
            // Get the Return Values
            bool overExposed = Convert.ToBoolean(ds.Tables[0].Rows[0][0]);
            if (overExposed == true)
            {
                //spc.DomainMessages.Add(new Error("The selected Legal Entity is already covered on " + maxPolicies + " Life Policies.", "The selected Legal Entity is already covered on  " + maxPolicies + " Life Policies."));
                //args.Cancel = true;
                return false;
            }

            // 4. check the person doesnt already play a role on the account
            foreach (IRole r in legalEntityNaturalPerson.Roles)
            {
                if (r.Account.Key == accountLifePolicy.Key)
                {
                    //spc.DomainMessages.Add(new Error("The selected Legal entity is already an Assured Life on this Policy.", "The selected Legal entity is already an Assured Life on this Policy."));
                    //args.Cancel = true;
                    return false;
                }
            }

            return true;
        }

        //public static int ExecuteNonQueryOnCastleTran(string SQL, Type Ty)
        //{
        //    return ExecuteNonQueryOnCastleTran(SQL, Ty, null);
        //}

        //public static DataSet ExecuteQueryOnCastleTran(string SQL, Type T)
        //{
        //    return ExecuteQueryOnCastleTran(SQL, T, null);
        //}

        public static int ExecuteNonQueryOnCastleTran(string query, Type Ty)//, ParameterCollection parameters)
        {
            IDbCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            //if (null != parameters)
            //{
            //    for (int i = 0; i < parameters.Count; i++)
            //    {
            //        SqlParameter param = parameters[i];
            //        cmd.Parameters.Add(param);
            //    }
            //}
            NHibernateDelegate d = new NHibernateDelegate(ProcessNonQuery);
            return Convert.ToInt32(ActiveRecordMediator.Execute(Ty, d, cmd));
        }

        public static DataSet ExecuteQueryOnCastleTran(string query, Type Ty)//, ParameterCollection parameters)
        {
            IDbCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            //if (null != parameters)
            //{
            //    for (int i = 0; i < parameters.Count; i++)
            //    {
            //        SqlParameter param = parameters[i];
            //        cmd.Parameters.Add(param);
            //    }
            //}
            NHibernateDelegate d = new NHibernateDelegate(ProcessQuery);
            return (DataSet)ActiveRecordMediator.Execute(Ty, d, cmd);
        }

        protected static object ProcessNonQuery(NHibernate.ISession session, object data)
        {
            try
            {
                IDbConnection conn = session.Connection;
                IDbCommand cmd = data as IDbCommand;
                cmd.Connection = conn;
                session.Transaction.Enlist(cmd);
                return cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 1205)
                {
                    string BP = "Deadlock here";
                }
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            return -1;
        }

        protected static object ProcessQuery(NHibernate.ISession session, object data)
        {
            try
            {
                IDbConnection conn = session.Connection;
                IDbCommand cmd = data as IDbCommand;
                cmd.Connection = conn;
                session.Transaction.Enlist(cmd);
                DataSet ds = new DataSet();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd.CommandText, conn.ConnectionString))
                {
                    da.Fill(ds, "DATA");
                }
                return ds;
            }
            catch (Exception ex)
            {
                LogPlugin.LogError("ProcessQuery failed: {0}{1}{2}", data, Environment.NewLine, ex.ToString());
            }
            return new DataSet();
        }

    }
}
