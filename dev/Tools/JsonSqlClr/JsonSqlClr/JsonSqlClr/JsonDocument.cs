using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Text;

public partial class JsonDocumentService
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlString JsonDocument(Guid id, string name, string description, int version, string documentFormatVersion, string documentType, string data)
    {
        StringBuilder json = new StringBuilder();
        try
        {
            json.Append("{");
            json.AppendFormat("{0},", JsonString("id", id));
            json.AppendFormat("{0},", JsonString("name", name.Replace(@"\",@"\\")));
            json.AppendFormat("{0},", JsonString("description", description));
            json.AppendFormat("{0},", JsonString("version", version));
            json.AppendFormat("{0},", JsonString("documentFormatVersion", documentFormatVersion));
            json.AppendFormat("{0},", JsonString("documentType", documentType));
            json.AppendFormat("\"document\":{0}",data);
            json.Append("}");

        }
        catch (Exception)
        {

            throw;
        }
        // Put your code here
        return new SqlString(json.ToString());
    }

    private static string JsonString(string propertyName, int value)
    {
        return string.Format("\"{0}\":{1}", propertyName, value);
    }
    private static string JsonString(string propertyName, object value)
    {
        return string.Format("\"{0}\":\"{1}\"", propertyName, value);
    }
}
