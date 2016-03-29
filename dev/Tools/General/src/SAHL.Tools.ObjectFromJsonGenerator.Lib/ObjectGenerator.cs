using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Abstractions;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using SAHL.Tools.ObjectFromJsonGenerator.Lib.Templates;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SAHL.Tools.ObjectFromJsonGenerator.Lib
{
    public class ObjectGenerator : SAHL.Tools.ObjectFromJsonGenerator.Lib.IObjectGenerator
    {
        public class SetMap
        {
            public string Data { get; set; }
            public int Version { get; set; }
            public bool IsPublished { get; set; }
            public DateTime PublishDate { get; set; }
        };

        public class SetMapType
        {
            public string mapType { get;protected set; }
            public SetMap setMap { get;protected set; }

            public SetMapType(string mapType,SetMap setMap)
            {
                this.mapType = mapType;
                this.setMap = setMap;
            }
        };

        

        public ObjectGenerator()
        {
        }

        private string GetText<T>(string inputData, string namespaceName, string version, string otherData = null, string otherDataVersion = null, string enumVersion = null, string msgVersion = null) where T : GeneratorObject
        {
            T generator;
            if ((otherData!=null)&&(enumVersion == null))
                generator = (T)Activator.CreateInstance(typeof(T), inputData, namespaceName, version, otherData, otherDataVersion);
            else if (enumVersion != null)
                generator = (T)Activator.CreateInstance(typeof(T), inputData, namespaceName, version, otherData, otherDataVersion, enumVersion, msgVersion);
            else                
                generator = (T)Activator.CreateInstance(typeof(T), inputData, namespaceName, version);
            generator.ParseJson();
            return generator.TransformText();
        }

        public void GenerateObjects(string connectionString, string outputPath, string MsgSetVersion, string VarSetVersion, string EnumSetVersion, string defaultNamespace, string buildMode = "Debug")
        {
            string enumSave = "";
            string extraData = null;
            outputPath = Path.Combine(outputPath, "Globals");
            IEnumerable<SetMap> msgRaw = null;
            string[] tables = { "EnumerationSet", "VariableSet", "MessageSet" };
            string[] requiredVersions = { EnumSetVersion, VarSetVersion, MsgSetVersion };
            
            List<SetMapType> mappings = new List<SetMapType>();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                for (int i = 0; i < 3; ++i)
                {
                    string query = "";
                    if (buildMode.Equals("Debug", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (requiredVersions[i] != "*")
                        {
                            query = String.Format(DBQueries.SpecificVersion, tables[i]);
                            msgRaw = connection.Query<SetMap>(query, new { ver = Int32.Parse(requiredVersions[i]) });
                        }
                        else
                        {
                            query = String.Format(DBQueries.All, tables[i]);
                            msgRaw = connection.Query<SetMap>(query);
                        }

                        foreach (SetMap map in msgRaw)
                        {
                            mappings.Add(new SetMapType(tables[i], map));
                        }
                    }
                    else if (buildMode.Equals("Release", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (requiredVersions[i] != "*")
                        {
                            query = String.Format(DBQueries.SpecificPublishedVersion, tables[i]);
                            msgRaw = connection.Query<SetMap>(query, new { ver = Int32.Parse(requiredVersions[i]) });
                        }
                        else
                        {
                            query = String.Format(DBQueries.AllPublished, tables[i]);
                            msgRaw = connection.Query<SetMap>(query);
                        }

                        foreach (SetMap map in msgRaw)
                        {
                            mappings.Add(new SetMapType(tables[i], map));
                        }
                    }
                }
            }

            IOrderedEnumerable<SetMapType> orderedMaps = mappings.OrderBy(x => x.setMap.PublishDate).ThenBy(x=>x.mapType);
            Save(orderedMaps.ToList(), defaultNamespace, outputPath);
            SaveLatest(orderedMaps.ToList(), defaultNamespace, outputPath);
        }

        public Dictionary<string, string> GenerateLatestDebugClasses(string connectionString, string MsgSetVersion, string VarSetVersion, string EnumSetVersion)
        {
            string defaultNamespace = "SAHL.DecisionTree.Shared";
            Dictionary<string, string> generatedClasses = new Dictionary<string, string>();
            string enumSave = "";
            string extraData = null;            
            IEnumerable<SetMap> msgRaw = null;
            string[] tables = { "EnumerationSet", "VariableSet", "MessageSet" };
            string[] requiredVersions = { EnumSetVersion, VarSetVersion, MsgSetVersion };
            string[] actualVersions = new string[3];
            List<SetMapType> mappings = new List<SetMapType>();

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                for (int i = 0; i < 3; ++i)
                {
                    string query = "";

                    if (requiredVersions[i] != "*")
                    {
                        query = String.Format(DBQueries.SpecificVersion, tables[i]);
                        msgRaw = connection.Query<SetMap>(query, new { ver = Int32.Parse(requiredVersions[i]) });                        
                    }
                    else
                    {
                        query = String.Format(DBQueries.LatestVersion, tables[i]);
                        msgRaw = connection.Query<SetMap>(query);                        
                    }
                    foreach (SetMap map in msgRaw)
                    {
                        mappings.Add(new SetMapType(tables[i], map));
                        actualVersions[i] = map.Version.ToString();
                    }
                    
                }
            }

            IOrderedEnumerable<SetMapType> orderedMaps = mappings.OrderBy(x => x.setMap.PublishDate).ThenBy(x => x.mapType);
            List<SetMapType> maps = orderedMaps.ToList();
           
            Dictionary<string, Type> generators = new Dictionary<string, Type>()
            {
                {"EnumerationSet",typeof(EnumObject)},
                {"MessageSet",typeof(MessageObject)},
                {"VariableSet",typeof(VariableObject)}
            };

            SetMapType lastEnumerationSetMap = null;

            for (int i = 0; i < maps.Count(); i++)
            {
                string enumData = null;
                string enumDataVersion = null;
                SetMapType mapType = maps[i];
                if (mapType.mapType == "EnumerationSet")
                {
                    lastEnumerationSetMap = mapType;
                }
                else if (mapType.mapType == "VariableSet")
                {
                    enumData = mapType.setMap.Data.ToString();
                    enumDataVersion = mapType.setMap.Version.ToString();
                }                
                
                MethodInfo method = typeof(ObjectGenerator).GetMethod("GetText", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo generic = method.MakeGenericMethod(generators[mapType.mapType]);

                string generatedData = "";
                if (mapType.mapType == "VariableSet")
                {
                    generatedData = (string)generic.Invoke(this, new object[] { mapType.setMap.Data.ToString(), defaultNamespace, mapType.setMap.Version.ToString(), enumData, enumDataVersion, actualVersions[0], actualVersions[2] });//, actualVersions[0], actualVersions[2]
                }
                else
                {
                    generatedData = (string)generic.Invoke(this, new object[] { mapType.setMap.Data.ToString(), defaultNamespace, mapType.setMap.Version.ToString(), enumData, enumDataVersion, null, null });
                }

                generatedClasses.Add(mapType.mapType, generatedData);
            }

            return generatedClasses;
        }

        private void Save(List<SetMapType> maps,string defaultNamespace,string outputPath)
        {
            Dictionary<string, string> fileNames = new Dictionary<string, string>()
            {
                {"EnumerationSet","Enumerations_{0}.cs"},
                {"MessageSet","Messages_{0}.cs"},
                {"VariableSet","Variables_{0}.cs"}
            };
            Dictionary<string, Type> generators = new Dictionary<string, Type>()
            {
                {"EnumerationSet",typeof(EnumObject)},
                {"MessageSet",typeof(MessageObject)},
                {"VariableSet",typeof(VariableObject)}
            };
            
            SetMapType lastEnumerationSetMap = null;

            for (int i = 0; i < maps.Count(); i++)
            {
                string enumData = null;
                string enumDataVersion = null;
                SetMapType mapType = maps[i];
                if (mapType.mapType == "EnumerationSet")
                {
                    lastEnumerationSetMap = mapType;
                }
                else if (mapType.mapType == "VariableSet")
                {
                    enumData = mapType.setMap.Data.ToString();
                    enumDataVersion = mapType.setMap.Version.ToString();
                }

                string fileName = string.Format(fileNames[mapType.mapType],mapType.setMap.Version);
                if(!(mapType.setMap.IsPublished && DoesFileExist(outputPath,fileName)))
                {
                    MethodInfo method = typeof(ObjectGenerator).GetMethod("GetText", BindingFlags.NonPublic | BindingFlags.Instance);
                    MethodInfo generic = method.MakeGenericMethod(generators[mapType.mapType]);

                    string generatedData = (string)generic.Invoke(this, new object[] { mapType.setMap.Data.ToString(), defaultNamespace, mapType.setMap.Version.ToString(), enumData, enumDataVersion, null, null });

                    SaveFile(outputPath, fileName, generatedData);
                }
            }
        }

        private void SaveLatest(List<SetMapType> maps,string defaultNamespace,string outputPath)
        {
            Dictionary<string, string> fileNames = new Dictionary<string, string>()
            {
                {"EnumerationSet","Enumerations.cs"},
                {"MessageSet","Messages.cs"},
                {"VariableSet","Variables.cs"}
            };
            Dictionary<string, Type> generators = new Dictionary<string, Type>()
            {
                {"EnumerationSet",typeof(EnumObject)},
                {"MessageSet",typeof(MessageObject)},
                {"VariableSet",typeof(VariableObject)}
            };
            List<SetMapType> latestPublishedSet = new List<SetMapType>();
            SetMapType lastEnumerationSetMap = null;
            foreach (KeyValuePair<string, Type> kvp in generators)
            {
                var latest = maps.Where(x => x.mapType == kvp.Key).OrderByDescending(o => o.setMap.Version).First();

                string enumData = null;
                string enumDataVersion = null;
                if (latest.mapType == "EnumerationSet")
                {
                    lastEnumerationSetMap = latest;
                }
                else if (latest.mapType == "VariableSet")
                {
                    enumData = lastEnumerationSetMap.setMap.Data.ToString();
                    enumDataVersion = lastEnumerationSetMap.setMap.Version.ToString();
                }
                
                string fileName = fileNames[kvp.Key];
                MethodInfo method = typeof(ObjectGenerator).GetMethod("GetText", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo generic = method.MakeGenericMethod(kvp.Value);
                string generatedData = (string)generic.Invoke(this, new object[] { latest.setMap.Data.ToString(), defaultNamespace,null, enumData, enumDataVersion, null, null });
                SaveFile(outputPath, fileName, generatedData);
            }
        }

        string testString = "{\"variables\":{\"groups\":[{\"id\":1,\"name\":\"Credit\",\"variables\":[{\"id\":1,\"name\":\"Loan Amount Max\",\"usage\":\"global\",\"type\":\"float\"},{\"id\":2,\"name\":\"Employment Type\",\"usage\":\"global\",\"type\":\"enumeration\",\"enumeration_group_id\":\"G0\"},{\"id\":8,\"name\":\"Loss Control\",\"usage\":\"global\",\"type\":\"enumeration\",\"enumeration_group_id\":\"G0\"},{\"id\":9,\"name\":\"Interest Only\",\"usage\":\"global\",\"type\":\"enumeration\",\"enumeration_group_id\":\"G0\"}],\"groups\":[{\"id\":5,\"name\":\"Further Lending\"}]},{\"id\":2,\"name\":\"Loss Control\",\"variables\":[{\"id\":3,\"name\":\"Max Attorney Fees\",\"usage\":\"global\",\"type\":\"float\"},{\"id\":4,\"name\":\"FAdj\",\"usage\":\"global\",\"type\":\"enumeration\",\"enumeration_group_id\":\"G2\"}],\"groups\":[{\"id\":3,\"name\":\"Debt Counselling\",\"variables\":[{\"id\":5,\"name\":\"Max Collections\",\"usage\":\"global\",\"type\":\"int\"},{\"id\":6,\"name\":\"Finance\",\"usage\":\"global\",\"type\":\"enumeration\",\"enumeration_group_id\":\"G2\"}]},{\"id\":4,\"name\":\"Litigation\",\"variables\":[{\"id\":7,\"name\":\"Default Magistrates Court\",\"usage\":\"global\",\"type\":\"string\"}]}]}],\"values\":[{\"variableId\":1,\"value\":\"2500000\"},{\"variableId\":2,\"value\":\"G0_E2\"},{\"variableId\":8,\"value\":\"G0_E1\"},{\"variableId\":9,\"value\":\"G0_E0\"},{\"variableId\":3,\"value\":\"78000\"},{\"variableId\":4,\"value\":\"G2_E0\"},{\"variableId\":5,\"value\":\"1\"},{\"variableId\":6,\"value\":\"G2_E0\"},{\"variableId\":7,\"value\":\"Durban\"}]}}";

        private void SaveFile(string location, string fileName,string content)
        {
            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }
            string filePath = Path.Combine(location, fileName);
            File.WriteAllText(filePath, content);
        }

        private bool DoesFileExist(string location, string fileName)
        {
            FileInfo info = new FileInfo(Path.Combine(location, fileName));
            return info.Exists;
        }

        public static string StripBadChars(string inString)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9_-]");
            return rgx.Replace(inString, string.Empty);
        }

        public static string ToPascalCase(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return "";
            }
            original = ObjectGenerator.StripBadChars(original);
            return string.Format("{0}{1}", original.Substring(0, 1).ToUpper(), original.Substring(1, original.Length - 1));
        }

        public static string toCamelCase(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                throw new ArgumentNullException("original");
            }
            original = ObjectGenerator.StripBadChars(original);
            return string.Format("{0}{1}", original.Substring(0, 1).ToLower(), original.Substring(1, original.Length - 1));
        }
    }
}
