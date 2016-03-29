using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SAHL.Test;
using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.Collections;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class ReportStatementTest : TestBase
    {
        [Test]
        public void GetReportStatementByReportGroupKey()
        {
            using (new SessionScope())
            {
                IDomainMessageCollection messages = new DomainMessageCollection();

                ParameterCollection parameters = new ParameterCollection();
                IDbConnection con = Helper.GetSQLDBConnection();
                DataTable DT = new DataTable();
                string query = "Select * from [2AM].[dbo].[ReportStatement] (nolock) where ReportGroupKey in (Select top 1 ReportGroupKey from [2AM].[dbo].[ReportStatement] (nolock) where ReportGroupKey is not null)";
                Helper.FillFromQuery(DT, query, con, parameters);

                Assert.That(DT.Columns.Count == 11, "The number of columns on the ReportStatement table has changed since this test was implemented.");
                Assert.That(DT.Rows.Count > 0, "The DB contains no ReportStatement records with non-null ReportGroupKeys");

                IReadOnlyEventList<IReportStatement> list = ReportStatement.GetReportStatementByReportGroupKey(messages, (int)DT.Rows[0][7]);

                Assert.AreEqual(DT.Rows.Count, list.Count, "The number of rows returned by the method is incorrect.");

                for (int i = 0; i < list.Count; i++)
                {
                    Assert.AreEqual((int)DT.Rows[i][0], list[i].Key);
                    if (list[i].OriginationSourceProduct != null) Assert.AreEqual((int)DT.Rows[i][1], list[i].OriginationSourceProduct.Key);
                    if (list[i].ReportName != null) Assert.AreEqual((string)DT.Rows[i][2], list[i].ReportName);
                    if (list[i].Description != null) Assert.AreEqual((string)DT.Rows[i][3], list[i].Description);
                    if (list[i].StatementName != null) Assert.AreEqual((string)DT.Rows[i][4], list[i].StatementName);
                    if (list[i].GroupBy != null) Assert.AreEqual((string)DT.Rows[i][5], list[i].GroupBy);
                    if (list[i].OrderBy != null) Assert.AreEqual((string)DT.Rows[i][6], list[i].OrderBy);
                    if (list[i].ReportGroup != null) Assert.AreEqual((int)DT.Rows[i][7], list[i].ReportGroup.Key);
                    if (list[i].Feature != null) Assert.AreEqual((int)DT.Rows[i][8], list[i].Feature.Key);
                    if (list[i].ReportType != null) Assert.AreEqual((int)DT.Rows[i][9], list[i].ReportType.Key);
                    if (list[i].ReportOutputPath != null) Assert.AreEqual((string)DT.Rows[i][10], list[i].ReportOutputPath);
                }
            }
        }

        //[Test]
        //public void GetParameterValidValues()
        //{
        //    IDomainMessageCollection messages = new DomainMessageCollection();

        //    ParameterCollection parameters = new ParameterCollection();
        //    IDbConnection con = Helper.GetSQLDBConnection();
        //    DataTable DT = new DataTable();
        //    string query = "Select top 1 rs.ReportStatementKey, rs.ReportTypeKey, rp.ReportParameterKey, rp.StatementName from [2AM].[dbo].[ReportStatement] rs (nolock) "
        //        + "join [2AM].[dbo].[ReportParameter] rp (nolock) on rp.ReportStatementKey = rs.ReportStatementKey "
        //        + "join [2AM].[dbo].[uiStatement] ui (nolock) on ui.StatementName = rp.StatementName";

        //    Helper.FillFromQuery(DT, query, con, parameters);

        //    Assert.That(DT.Rows.Count == 1, "No data found");

        //    int rsKey = (int)DT.Rows[0][0];
        //    int rtKey = (int)DT.Rows[0][1];
        //    int rpKey = (int)DT.Rows[0][2];
        //    string statementName = DT.Rows[0][3].ToString();

        //    DT = new DataTable(); 
            
        //    query = String.Format("Select Statement from [2AM].[dbo].[uiStatement] ui (nolock) where StatementName = '{0}' and Version = (Select max(Version) from [2AM].[dbo].[UIStatement] (nolock) where StatementName = '{0}')", statementName);
        //    Helper.FillFromQuery(DT, query, con, parameters);

        //    query = DT.Rows[0][0].ToString();

        //    DT = new DataTable();
        //    Helper.FillFromQuery(DT, query, con, parameters);

        //    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
        //    ReportStatement rs = BMTM.GetMappedType<ReportStatement>(ReportStatement_DAO.Find(rsKey));
        //    ReportParameter rp = BMTM.GetMappedType<ReportParameter>(ReportParameter_DAO.Find(rpKey));

        //    Dictionary<string, object> list = rs.GetParameterValidValues(rp);

        //    Assert.That(DT.Rows.Count == list.Count);

        //    int idx = 0;

        //    foreach (string key in list.Keys)
        //    {
        //        string s = "";
        //        if (DT.Columns.Count > 1)
        //            s = DT.Rows[idx][1].ToString();
        //        else
        //            s = DT.Rows[idx][0].ToString();

        //        string i = DT.Rows[idx][0].ToString();
        //        Assert.That(key == s);
        //        Assert.That(list[key].ToString() == i);
        //        idx++;

        //    }
        // }
    }
}
