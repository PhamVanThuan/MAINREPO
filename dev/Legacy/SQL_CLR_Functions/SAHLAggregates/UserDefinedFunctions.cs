using Microsoft.SqlServer.Server;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;

public partial class UserDefinedFunctions
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlGuid CombGuid()
    {
        byte[] destinationArray = Guid.NewGuid().ToByteArray();
        DateTime time = new DateTime(0x76c, 1, 1);
        DateTime now = DateTime.Now;
        TimeSpan span = new TimeSpan(now.Ticks - time.Ticks);
        TimeSpan timeOfDay = now.TimeOfDay;
        byte[] bytes = BitConverter.GetBytes(span.Days);
        byte[] array = BitConverter.GetBytes((long)(timeOfDay.TotalMilliseconds / 3.333333));
        Array.Reverse(bytes);
        Array.Reverse(array);
        Array.Copy(bytes, bytes.Length - 2, destinationArray, destinationArray.Length - 6, 2);
        Array.Copy(array, array.Length - 4, destinationArray, destinationArray.Length - 4, 4);
        return new SqlGuid(destinationArray);
    }

    [SqlFunction(DataAccess = DataAccessKind.Read, IsDeterministic = true)]
    [return: SqlFacet(MaxSize = -1)]
    public static SqlString CapitecApplicantsToJson([SqlFacet(IsNullable = false)] int applicationNumber)
    {
        StringBuilder json = new StringBuilder();
        int applicantCount = 1;
        DataTable table = new DataTable();
        string selectStatement = string.Format("select sal.Name Salutation, p.FirstName, p.Surname, p.IdentityNumber " +
                                                "from [Capitec].[dbo].[Application] ap (nolock) " +
                                                "join [Capitec].[dbo].[ApplicationApplicant] apa (nolock) on apa.ApplicationId = ap.Id " +
                                                "join [Capitec].[dbo].[Applicant] apl (nolock) on apl.Id = apa.ApplicantId " +
                                                "join [Capitec].[dbo].[Person] p (nolock) on p.Id = apl.PersonID " +
                                                "join [Capitec].[dbo].[SalutationEnum] sal (nolock) on sal.Id = p.SalutationEnumId " +
                                                "where ap.ApplicationNumber = {0}", applicationNumber);

        using (SqlConnection conn = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(selectStatement, conn);
            da.Fill(table);

            json.AppendFormat("{{\"noofapplicants\":\"{0}\"", table.Rows.Count);

            //if (table.Rows.Count > 0)
                json.Append(",\"applicants\":[");

            foreach (DataRow row in table.Rows)
            {
                json.Append("{");
                json.AppendFormat("\"salutation\":\"{0}\",", row["Salutation"]);
                json.AppendFormat("\"firstname\":\"{0}\",", row["Firstname"]);
                json.AppendFormat("\"surname\":\"{0}\",", row["Surname"]);
                json.AppendFormat("\"identitynumber\":\"{0}\"", row["IdentityNumber"]);
                json.Append("}");

                if (table.Rows.Count > applicantCount)
                    json.Append(",");

                applicantCount++;
            }
            //if (table.Rows.Count > 0)
                json.Append("]");
            json.Append("}");
        }
        return (SqlString)json.ToString();
    }
}