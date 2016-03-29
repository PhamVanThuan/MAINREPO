using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SAHL.Tools.EnumGenerator
{
    public class EnumGenerator
    {
        public IEnumerable<EnumData> GenerateEnums(string connectionString, string outputDirectory, string database, IEnumerable<string> enumsToGenerate)
        {
            List<EnumData> enumDataList = new List<EnumData>();
            using (SqlConnection connection = new SqlConnection(connectionString.Replace("[","").Replace("]","")))
            {
                connection.Open();
                foreach(var enumtoGen in enumsToGenerate)
                {
                    string[] enumParts = enumtoGen.Split(new char[] {','});

                    string schema = enumParts[0];
                    string table = enumParts[1];
                    string keyColumn = enumParts[2];
                    string descColumn = enumParts[3];
                    string where = "";
                    string enumName = "";
                    if(enumParts.Length> 4)
                    {
                        where = enumParts[4];
                    }
                    if(enumParts.Length > 5)
                    {
                        enumName = enumParts[5];
                    }
                    else{
                        enumName = table;
                    }

                    string query = string.Format("select {0} as [Key], {1} as [Desc] from [{2}].{3}.{4}", keyColumn, descColumn, database, schema, table);
                    if(!string.IsNullOrEmpty(where))
                    {
                        query += string.Format(" {0}", where);
                    }
                    using(SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        List<EnumValue> values = new List<EnumValue>();
                        using (SqlDataReader enumDataReader = cmd.ExecuteReader())
                        {
                            if (enumDataReader != null)
                            {
                                while (enumDataReader.Read())
                                {
                                    string enumDataKey = enumDataReader[0].ToString();
                                    string enumDataValue = enumDataReader[1].ToString();

                                    values.Add(new EnumValue(enumDataKey, enumDataValue));
                                }
                            }
                        }
                        if (values.Count > 0)
                        {
                            enumDataList.Add(new EnumData(enumName, values.Select(x=>x.Value)));
                        }
                    }
                }
                connection.Close();
            }

            return enumDataList;
        }
    }

    public class EnumData
    {
        public EnumData(string enumName, IEnumerable<string> values)
        {
            this.EnumName = enumName;
            this.EnumValues = values;
        }

        public string EnumName { get; protected set; }

        public IEnumerable<string> EnumValues { get; protected set; }
    }

    public class EnumValue
    {
        public EnumValue(string name,string value)
        {
            string part1 = this.Sanitise(name);
            string part2 = this.Sanitise(value);

            this.Value = string.Format("{0} = {1}", part2, part1);
        }


        public string Value {get; protected set;}

        private string Sanitise(string stringToSanitise)
        {
            string result = stringToSanitise.Trim().Replace(" ", "");
            result = result.Replace("/", "_");
            result = result.Replace("-", "_");
            result = result.Replace("–", "_");
            result = result.Replace("(", "_");
            result = result.Replace(")", "");
            result = result.Replace("'", "");
            result = result.Replace("+", "");
            result = result.Replace("&", "_and_");
            result = result.Replace(".", "");
            result = result.Replace(",", "_");
            if (result.StartsWith("_"))
            {
                result.Remove(0, 1);
            }
            return result;
        }
    }
}