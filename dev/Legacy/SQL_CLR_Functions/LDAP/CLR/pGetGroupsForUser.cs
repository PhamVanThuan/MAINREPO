using Microsoft.SqlServer.Server;
//------------------------------------------------------------------------------
// <copyright file="CSSqlClassFile.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void pGetGroupsForUser(string ADUserName)
    {
        SqlPipe sp = SqlContext.Pipe;
        string WSURL = string.Empty;// "http://n101646/LDAPQuery/LDAPQueryService.asmx";
        string str = "select ControlText from [2am]..control where ControlDescription='AD Webservice URL'";
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = "Context Connection=true";
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = str;
                conn.Open();

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        WSURL = rdr[0].ToString();
                        sp.Send(WSURL);
                    }
                    else
                    {
                        throw new Exception("Unable to get WSURL");
                    }
                    rdr.Close();
                }
                conn.Close();
            }
        }

        LDAP.Proxy.LDAPQueryService.LDAPQueryService svc = new LDAP.Proxy.LDAPQueryService.LDAPQueryService();
        svc.Url = WSURL;

        string[] Groups = svc.GetFeatureAccessForUser(ADUserName);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("select distinct(f.ShortName), f.LongName, fg.ADUserGroup ");
        sb.AppendLine("from featuregroup fg  ");
        sb.AppendLine("join Feature f on fg.FeatureKey=f.FeatureKey ");
        sb.AppendLine("where fg.ADUserGroup in (");
        for (int i = 0; i < Groups.Length; i++)
        {
            sb.AppendFormat("'{0}',", Groups[i].Replace("'", "''"));
        }
        sb.Remove(sb.Length - 1, 1);
        sb.AppendLine(");");
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = "Context Connection=true";
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = sb.ToString();

                conn.Open();

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    SqlContext.Pipe.Send(rdr);
                    rdr.Close();
                }
                conn.Close();
            }
        }
    }
};
