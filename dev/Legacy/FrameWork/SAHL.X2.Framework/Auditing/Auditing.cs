using System.Data;
using System.IO;

namespace SAHL.X2.Framework.Auditing
{
    public class AuditService
    {
        private static string GetDiffGram(DataRow p_DataRow)
        {
            DataSet dataSet = new DataSet("AuditDataSet");

            Stream schema = new MemoryStream();
            p_DataRow.Table.WriteXmlSchema(schema, false);
            DataTable dataTable = new DataTable(p_DataRow.Table.TableName);
            schema.Position = 0;
            dataTable.ReadXmlSchema(schema);
            schema.Close();

            dataSet.Tables.Add(dataTable);
            dataTable.ImportRow(p_DataRow);
            Stream stream = new MemoryStream();
            dataSet.WriteXml(stream, XmlWriteMode.DiffGram);
            stream.Position = 0;

            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            stream.Close();

            string str = System.Text.Encoding.ASCII.GetString(bytes);

            return str;
        }
    }
}