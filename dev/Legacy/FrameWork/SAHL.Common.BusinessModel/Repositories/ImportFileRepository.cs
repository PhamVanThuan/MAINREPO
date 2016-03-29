using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Utils;
using SAHL.Common.Factories;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using Castle.ActiveRecord.Queries;
using Castle.ActiveRecord;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    [FactoryType(typeof(IImportFileRepository))]
    public class ImportFileRepository : AbstractRepositoryBase, IImportFileRepository
    {
		/// <summary>
		/// Gets the Import History sorted by Import Date
		/// </summary>
        public IReadOnlyEventList<IImportFile> GetImportHistory()
        {
			try
			{
				string HQL = "from ImportFile_DAO i where i.FileType = 'Actual' order by i.DateImported desc";
				SimpleQuery<ImportFile_DAO> q = new SimpleQuery<ImportFile_DAO>(HQL);
				q.SetQueryRange(5);
				ImportFile_DAO[] res = q.Execute();
				IEventList<IImportFile> list = new DAOEventList<ImportFile_DAO, IImportFile, ImportFile>(res);
				return new ReadOnlyEventList<IImportFile>(list);
			}
			catch
			{
				return null;
			}
        }

		/// <summary>
		/// Gets the results of an import by the file key
		/// </summary>
		/// <param name="fileKey"></param>
        public IReadOnlyEventList<IImportLegalEntity> GetImportResultsByFileKey(int fileKey)
        {
			try
			{
				string HQL = "from ImportLegalEntity_DAO L where ErrorMsg is not null and L.ImportApplication.ImportFile.Key = ?";
				SimpleQuery<ImportLegalEntity_DAO> query = new SimpleQuery<ImportLegalEntity_DAO>(HQL, fileKey);
				ImportLegalEntity_DAO[] res = query.Execute();
				IEventList<IImportLegalEntity> list = new DAOEventList<ImportLegalEntity_DAO, IImportLegalEntity, ImportLegalEntity>(res);
				return new ReadOnlyEventList<IImportLegalEntity>(list);
			}
			catch
			{
				return null;
			}
        }

		/// <summary>
		/// Gets the RCSUploadExportPath from the Control Repository
		/// </summary>
        public string GetRCSUploadExportPath()
        {
            try
            {
                string uploadExportPath = "";

                IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                IControl ctrl = ctrlRepo.GetControlByDescription("RCSUploadExportPath");

                if (ctrl == null || string.IsNullOrEmpty(ctrl.ControlText))
                    throw new ArgumentNullException("There is no value for 'RCSUploadExportPath' in the control table");

                uploadExportPath = ctrl.ControlText.ToString();

                if (!string.IsNullOrEmpty(uploadExportPath))
                {
                    return uploadExportPath;
                }
                else
                {
                    return "";
                }
            }

            catch
            {
                return "";
            }
        }

		/// <summary>
		/// Deletes all the files in RCSUploadExportPath directory
		/// </summary>
		/// <param name="uploadExportPath"></param>
        public bool ClearRCSUploadExportPath(string uploadExportPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(uploadExportPath))
                {

                    if (IOUtils.DirectoryExists(uploadExportPath))
                    {
                        string[] fileList = IOUtils.DirectoryGetFiles(uploadExportPath);
                        foreach (string oldfile in fileList)
                        {
                            try
                            {
                                IOUtils.FileDelete(oldfile);
                            }
                            catch
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }

            catch
            {
                return false;
            }
        }

		/// <summary>
		/// Converts the uploaded CSV file into a Data Table and validates it against the XSD Abstraction
		/// </summary>
		/// <param name="uploadPath"></param>
		/// <param name="uploadFile"></param>
		/// <param name="DT"></param>
		/// <param name="xsdAbs"></param>
		/// <param name="missingColumns"></param>
		/// <param name="extraColumns"></param>
		/// <param name="invalidData"></param>
        public void LoadRCS_CSVFile(string uploadPath, string uploadFile, ref DataTable DT, ref IXsdAbstraction xsdAbs, ref List<string> missingColumns, ref List<string> extraColumns, ref Dictionary<string, List<string>> invalidData)
        {
            if (uploadPath.Length > 0)
            {
                DT = new DataTable();
                DT = IOUtils.GetCsvFile(uploadPath + uploadFile, Convert.ToChar(","));
                if (DT != null)
                {
                    IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                    IControl ctrl = ctrlRepo.GetControlByDescription("RCSUploadXSDFile");

                    if (ctrl == null || ctrl.ControlText.Length == 0)
                        throw new ArgumentNullException("There is no value for 'RCSUploadXSDFile' in the control table");

                    string xsdFile = ctrl.ControlText.ToString();
                    if (!IOUtils.FileExists(xsdFile))
                        throw new Exception("XSD file [" + xsdFile + "] does not exist ");

                    xsdAbs = CsvXml.CreateXsdAbstraction(xsdFile);

                    CsvXml.WriteXmlFileFromDataTable(DT, CsvXml.CreateXsdAbstraction(xsdFile), uploadPath, ref missingColumns, ref extraColumns, ref invalidData, false);
                }
            }
            else
                throw new Exception("Upload path does not exist");
        }

		/// <summary>
		/// Converts a populated Data Table into and XML file and validates it against the XSD Abstraction
		/// </summary>
		/// <param name="DT"></param>
		/// <param name="missingColumns"></param>
		/// <param name="extraColumns"></param>
		/// <param name="invalidData"></param>
        public string GenerateXML(DataTable DT, ref List<string> missingColumns, ref List<string> extraColumns, ref Dictionary<string, List<string>> invalidData)
        {
            if (DT == null)
                return "";

            string xmlFileName = "";

            try
            {
                IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
                IControl ctrl = ctrlRepo.GetControlByDescription("RCSUploadXSDFile");

                if (ctrl == null || ctrl.ControlText.Length == 0)
                    throw new ArgumentNullException("There is no value for 'RCSUploadXSDFile' in the control table");

                string xsdFile = ctrl.ControlText.ToString();

                if (!IOUtils.FileExists(xsdFile))
                    throw new Exception("XSD file [" + xsdFile + "] does not exist ");

                ctrl = ctrlRepo.GetControlByDescription("RCSUploadExportPath");

                if (ctrl == null || ctrl.ControlText.Length == 0)
                    throw new ArgumentNullException("There is no value for 'RCSUploadExportPath' in the control table");

                xmlFileName = ctrl.ControlText.ToString();

                if (!IOUtils.DirectoryExists(xmlFileName))
                    throw new Exception("Export directory [" + xmlFileName + "] does not exist ");

                xmlFileName = xmlFileName + "\\" + DateTime.Now.ToString("ddMMyyyy") + ".xml";

                CsvXml.WriteXmlFileFromDataTable(DT, CsvXml.CreateXsdAbstraction(xsdFile), xmlFileName, ref missingColumns, ref extraColumns, ref invalidData, true);
            }
            catch
            {
                xmlFileName = "";
            }
            return xmlFileName;
        }

        /// <summary>
        /// 
        /// </summary>
        public IImportFile CreateEmptyImportFile()
        {
			return base.CreateEmpty<IImportFile, ImportFile_DAO>();
			//return new ImportFile(new ImportFile_DAO());
        }

		/// <summary>
		/// Writes away data from an XML file to the Import tables
		/// </summary>
		/// <param name="xmlFileName"></param>
		/// <param name="importFile"></param>
		public int ImportDataFromXML(string xmlFileName, IImportFile importFile)
		{
			int returnValue = 1, fileKey = 0, timeOut = 240, importStatusKey = (int)ImportStatuses.Pending;
			string query;
			ImportFile_DAO importFileDAO = (ImportFile_DAO)(importFile as IDAOObject).GetDAOObject();
			TransactionScope transScope = new TransactionScope();
			IDbConnection con = Helper.GetSQLDBConnection();
			ParameterCollection parameters = new ParameterCollection();
			SqlParameter paramReturn = Helper.AddParameter(parameters, "@return_value", SqlDbType.Int, ParameterDirection.Output, null);

			IControlRepository ctrlRepo = RepositoryFactory.GetRepository<IControlRepository>();
			IControl ctrl = ctrlRepo.GetControlByDescription("RCSUploadCommandTimeout");
			if (ctrl != null)
				if (ctrl.ControlNumeric.HasValue)
					timeOut = Convert.ToInt32(ctrl.ControlNumeric.Value);

			using (transScope)
			{
				try
				{
					importFileDAO.SaveAndFlush();
					fileKey = importFileDAO.Key;
				}
				catch
				{
					returnValue = -1;
				}
				finally
				{
					if (returnValue != -1)
					{
						transScope.VoteCommit();
					}
					else
					{
						transScope.VoteRollBack();
					}
				}
			}

			transScope = new TransactionScope();
			using (transScope)
			{
				try
				{
					if (fileKey != 0)
					{
						query = UIStatementRepository.GetStatement("COMMON", "ExecuteRCSImportXML_Data");
						Helper.AddIntParameter(parameters, "@FileKey", fileKey);
						Helper.AddIntParameter(parameters, "@ImportStatusKey", importStatusKey);
						Helper.ExecuteNonQuery(con, query, parameters, timeOut);
						if (Convert.ToInt32(paramReturn.Value) == 1)
						{
							returnValue = 1;
						}
						else
						{
							returnValue = -1;
						}
					}
					else
					{
						returnValue = -1;
					}
				}
				catch
				{
					returnValue = -1;
				}
				finally
				{
					if (returnValue != -1)
					{
						transScope.VoteCommit();
					}
					else
					{
						transScope.VoteRollBack();
					}
				}
			}

			transScope = new TransactionScope();
			using (transScope)
			{
				try
				{
					if (fileKey != 0 && returnValue != -1)
					{
						query = UIStatementRepository.GetStatement("COMMON", "ExecuteRCSImport");
						parameters = new ParameterCollection();
						Helper.AddIntParameter(parameters, "@InputFileKey", fileKey);
						paramReturn = Helper.AddParameter(parameters, "@return_value", SqlDbType.Int, ParameterDirection.Output, null);
						Helper.ExecuteNonQuery(con, query, parameters, timeOut);
						if (Convert.ToInt32(paramReturn.Value) == 0)
						{
							returnValue = fileKey;
						}
						else
						{
							returnValue = -1;
						}
					}
				}
				catch
				{
					returnValue = -1;
				}
				finally
				{
					if (returnValue != -1)
					{
						transScope.VoteCommit();
					}
					else
					{
						transScope.VoteRollBack();
					}
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Exposes the XsdAbstraction.SimpleNodes
		/// </summary>
		/// <param name="InvalidType"></param>
		/// <param name="xsdAbs"></param>
        public ISimpleNode GetSimpleNode(string InvalidType, IXsdAbstraction xsdAbs)
        {
            try
            {
                return xsdAbs.SimpleNodes.Find(delegate(ISimpleNode p) { return (p.Name == InvalidType); });
            }

            catch
            {
                return null;
            }
        }

        /// <summary>
        /// This class contains static functions for use when working with XSD.
        /// </summary>
        public class XsdAbstraction : SAHL.Common.BusinessModel.Interfaces.IXsdAbstraction
        {
			/// <summary>
			/// Defines the simple nodes
			/// </summary>
			internal class SimpleNode : SAHL.Common.BusinessModel.Interfaces.ISimpleNode
            {
                string m_name = "";
                string m_type = "";
                private List<string> m_enumerations = new List<string>();

                public string Name
                {
                    get { return m_name; }
                    set { m_name = value; }
                }

                public string Type
                {
                    get { return m_type; }
                    set { m_type = value; }
                }

                public List<string> Enumerations
                {
                    get { return m_enumerations; }
                }

                public SimpleNode()
                {

                }

                public SimpleNode(string name, string type)
                {
                    m_name = name;
                    m_type = type;
                }
            }

			/// <summary>
			/// Defines the element nodes
			/// </summary>
			internal class ElementNode
            {
                string m_name = "";
                string m_type = "";
                int m_maxLength = 0;
                bool m_required = false;
                string m_nodeType = "";
                List<ElementNode> m_elements = new List<ElementNode>();

                public string Name
                {
                    get { return m_name; }
                    set { m_name = value; }
                }

                public string Type
                {
                    get { return m_type; }
                    set { m_type = value; }
                }

                public List<ElementNode> Elements
                {
                    get { return m_elements; }
                    //set { m_elements = value; }
                }

                public string NodeType
                {
                    get { return m_nodeType; }
                    set { m_nodeType = value; }
                }

                public int MaxLength
                {
                    get { return m_maxLength; }
                    set { m_maxLength = value; }
                }

                public bool IsRequired
                {
                    get { return m_required; }
                    set { m_required = value; }
                }

                public ElementNode()
                {
                }

                public ElementNode(string name, string type)
                {
                    Name = name;
                    Type = type;
                }
            }

            List<string> m_simpleTypes = new List<string>();
            List<string> m_complexTypes = new List<string>();
            List<string> m_elements = new List<string>();

            public List<string> Elements
            {
                get { return m_elements; }
            }

            public List<string> SimpleTypes
            {
                get { return m_simpleTypes; }
            }

            public List<string> ComplexTypes
            {
                get { return m_complexTypes; }
            }

            List<ISimpleNode> m_simpleNodes = new List<ISimpleNode>();
            List<XmlNode> m_complexNodes = new List<XmlNode>();
            List<ElementNode> m_elementNodes = new List<ElementNode>();

            public List<ISimpleNode> SimpleNodes
            {
                get { return  m_simpleNodes; }
            }

            public List<XmlNode> ComplexNodes
            {
                get { return m_complexNodes; }
            }

            internal List<ElementNode> ElementNodes
            {
                get { return m_elementNodes; }
            }

            public XsdAbstraction()
            {
            }
        }

        /// <summary>
        /// Exposes all functions that are needed for the CSV and XML data
        /// </summary>
        internal static class CsvXml
        {
            /// <summary>
            /// Removes characters from a string
            /// </summary>
            /// <param name="victim"></param>
            private static string RemoveXS(string victim)
            {
                int idx = victim.IndexOf(":");
                //if (idx > -1)
                    victim = victim.Remove(0, idx + 1);
                return victim;
            }

            /// <summary>
            /// Creates an element node
            /// </summary>
            /// <param name="xsdAbs"></param>
            /// <param name="node"></param>
            private static XsdAbstraction.ElementNode CreateElementNode(XsdAbstraction xsdAbs, XmlNode node)
            {
                string name = node.Attributes["name"].InnerText;
                string type = null;
                string nodetype = null;
                int length = 0;
                bool required = true;

                for (int i = 0; i < node.Attributes.Count; i++)
                {
                    XmlAttribute xa = node.Attributes[i];

                    if (xa.Name == "minOccurs")
                    {
                        int x = int.Parse(xa.Value);

                        if (x == 0)
                        {
                            required = false;
                            break;
                        }
                    }
                }

                if (node.HasChildNodes)//simpletype
                {
                    XmlNode simpleType = node.FirstChild;
                    XmlNode restriction = simpleType.FirstChild;
                    type = restriction.Attributes["base"].InnerText;
                    nodetype = "base";

                    XmlNode maxLength = restriction.FirstChild;
                    string len = maxLength.Attributes["value"].InnerText;
                    length = int.Parse(len);
                }
                else
                {
                    type = node.Attributes["type"].InnerText;
                }

                type = RemoveXS(type);

                XsdAbstraction.ElementNode enode = new XsdAbstraction.ElementNode(name, type);
                enode.MaxLength = length;
                enode.IsRequired = required;

                if (nodetype == null)
                {
                    int idx = xsdAbs.ComplexTypes.IndexOf(type);

                    if (idx > -1)
                    {
                        nodetype = "complex";
                        XmlNode cnode = xsdAbs.ComplexNodes[idx];

                        if (cnode != null && cnode.HasChildNodes && cnode.FirstChild.HasChildNodes)
                        {
                            for (int i = 0; i < cnode.FirstChild.ChildNodes.Count; i++)
                            {
                                XsdAbstraction.ElementNode elNode = CreateElementNode(xsdAbs, cnode.FirstChild.ChildNodes[i]);
                                enode.Elements.Add(elNode);
                                xsdAbs.Elements.Add(elNode.Name);
                            }
                        }
                    }
                    else if (xsdAbs.SimpleTypes.Contains(type))
                    {
                        nodetype = "simple";
                    }
                }

                enode.NodeType = nodetype;

                return enode;
            }

            /// <summary>
            /// Creates the XSD Abstraction
            /// </summary>
            /// <param name="xsdFilePath"></param>
            internal static XsdAbstraction CreateXsdAbstraction(string xsdFilePath)
            {
                if (!IOUtils.FileExists(xsdFilePath))
                    return null;

                XmlDocument xsd = new XmlDocument();
                xsd.Load(xsdFilePath);

                XmlNodeList nodes = null;

                //get xsd nodes
                for (int i = 0; i < xsd.ChildNodes.Count; i++)
                {
                    if (xsd.ChildNodes[i].Name.ToLower() == "xs:schema" && xsd.ChildNodes[i].HasChildNodes)
                    {
                        nodes = xsd.ChildNodes[i].ChildNodes;
                        break;
                    }
                }

                XsdAbstraction xsdAbs = new XsdAbstraction();

                //get types
                for (int i = 0; i < nodes.Count; i++)
                {
                    XmlNode node = nodes[i];
                    string name = node.Attributes["name"].InnerText;
                    string type = "";

                    switch (node.Name.ToLower())
                    {
                        case "xs:complextype":
                            xsdAbs.ComplexTypes.Add(name);
                            xsdAbs.ComplexNodes.Add(node);
                            break;

                        case "xs:simpletype":
                            XmlNode restriction = node.FirstChild;
                            type = restriction.Attributes["base"].InnerText;
                            XmlNodeList enums = restriction.ChildNodes;

                            XsdAbstraction.SimpleNode snode = new XsdAbstraction.SimpleNode(name, type);

                            for (int k = 0; k < enums.Count; k++)
                            {
                                XmlNode e = enums[k];
                                snode.Enumerations.Add(e.Attributes["value"].InnerText);
                            }

                            xsdAbs.SimpleTypes.Add(snode.Name);
                            xsdAbs.SimpleNodes.Add(snode);
                            break;

                        default: break;
                    }//switch
                }

                //ok we've got lists of simple and complex types, now lets get the elements
                //for each element, mark it as base, simple, or complex. If complex, process again
                for (int i = 0; i < nodes.Count; i++)
                {
                    XmlNode node = nodes[i];

                    if (node.Name == "xs:element")
                    {
                        XsdAbstraction.ElementNode enode = CreateElementNode(xsdAbs, node);
                        xsdAbs.ElementNodes.Add(enode);
                        xsdAbs.Elements.Add(enode.Name);
                    }
                }

                return xsdAbs;
            }

            /// <summary>
            /// Checks that all required columns defined in the XSD exist in the populated Data Table 
            /// </summary>
            /// <param name="DT"></param>
            /// <param name="nodes"></param>
            /// <param name="missing"></param>
            internal static void MissingRequiredColumns(DataTable DT, IList<XsdAbstraction.ElementNode> nodes, ref List<string> missing)
            {
                if (DT == null || nodes == null)
                    return;

                if (missing == null)
                    missing = new List<string>();

                for (int i = 0; i < nodes.Count; i++)
                {
                    XsdAbstraction.ElementNode enode = nodes[i];

                    if (enode.NodeType == "complex")
                    {
                        MissingRequiredColumns(DT, enode.Elements, ref missing);
                    }
                    else
                    {
                        if (!DT.Columns.Contains(enode.Name))
                        {
                            string s = enode.Name;

                            if (enode.IsRequired)
                                s += " (required)";

                            missing.Add(s);
                        }
                    }
                }
            }

            /// <summary>
            /// Checks for any extra columns in the populated Data Table that are not defined in the XSD
            /// </summary>
            /// <param name="DT"></param>
            /// <param name="xsdAbs"></param>
            /// <param name="extra"></param>
            internal static void ExtraColumns(DataTable DT, XsdAbstraction xsdAbs, ref List<string> extra)
            {
                if (DT == null || xsdAbs == null)
                    return;

                if (extra == null)
                    extra = new List<string>();

                for (int i = 0; i < DT.Columns.Count; i++)
                {
                    if (!xsdAbs.Elements.Contains(DT.Columns[i].ColumnName))
                    {
                        extra.Add(DT.Columns[i].ColumnName);
                    }
                }
            }

            /// <summary>
            /// Writes the XML nodes from the populated Data Table and validates for missing, extra and against the XSD. Recursive.
            /// </summary>
            /// <param name="XT"></param>
            /// <param name="DT"></param>
            /// <param name="Row"></param>
            /// <param name="xsdAbs"></param>
            /// <param name="enode"></param>
            /// <param name="invalidData"></param>
            /// <param name="forReal"></param>
            private static bool WriteElement(XmlTextWriter XT, DataTable DT, DataRow Row, XsdAbstraction xsdAbs, XsdAbstraction.ElementNode enode, ref Dictionary<string, List<string>> invalidData, bool forReal)
            {
                bool AtLeastOneItemWasWritten = false;

                if (enode.Elements.Count > 0) //it's just a container
                {
                    if (forReal)
                        XT.WriteStartElement(enode.Name);

                    for (int k = 0; k < enode.Elements.Count; k++)
                    {
                        if (WriteElement(XT, DT, Row, xsdAbs, enode.Elements[k], ref invalidData, forReal))
                            AtLeastOneItemWasWritten = true;
                    }

                    if (forReal)
                        XT.WriteEndElement();
                }
                else //it's a final element
                {
                    object value = null;

                    if (DT.Columns.Contains(enode.Name))
                        value = Row[enode.Name]; ;

                    string val = "";

                    if (value != null)
                        val = Convert.ToString(value).Trim();

                    string type = enode.Type;
                    int idx = xsdAbs.SimpleTypes.IndexOf(enode.Type);

                    if (idx > -1)
                    {
                        ISimpleNode snode = xsdAbs.SimpleNodes[idx];
                        //check the corresponding row value to make sure it is one of the enums
                        //val = Convert.ToString(value);

                        bool containsUnknown = false;
                        foreach (string temp in snode.Enumerations)
                        {
                            if (temp.ToUpper() == "UNKNOWN")
                                containsUnknown = true;
                        }
                        if (string.IsNullOrEmpty(val) && containsUnknown)
                        {

                            val = "Unknown";
                        }
                        else
                        {
                            bool containsVal = false;
                            if (!string.IsNullOrEmpty(val))
                            {
                                foreach (string tempVal in snode.Enumerations)
                                {
                                    if (tempVal.ToUpper() == val.ToUpper())
                                        containsVal = true;
                                }
                                if (!containsVal)
                                {
                                    if (!invalidData.ContainsKey(enode.Name))
                                    {
                                        invalidData.Add(enode.Name, new List<string>());
                                    }

                                    //if (val == "")
                                    //    val = "NO DATA VALUE";

                                    if (!invalidData[enode.Name].Contains(val))
                                    {
                                        invalidData[enode.Name].Add(val);
                                    }

                                    return AtLeastOneItemWasWritten;
                                }
                            }
                        }
                    }

                    if (forReal && !string.IsNullOrEmpty(val))
                    {
                        if (enode.MaxLength > 0 && val.Length > enode.MaxLength)
                        {
                            val = val.Remove(enode.MaxLength - 1);
                        }

                        if (type.ToLower() == "datetime")
                        {
                            DateTime date = DateTime.MinValue;
                            if (DateTime.TryParseExact(val, "dd/MM/yyyy", null, DateTimeStyles.None, out date))
                                val = date.ToString("s");// String.Format("{DDDD:0}-{DD:1}-{DD:2}T{DD:3}:{DD:4}:{DD:5}", date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
                            else if (DateTime.TryParseExact(val, "dd-MM-yyyy", null, DateTimeStyles.None, out date))
                                val = date.ToString("s");
                            else if (DateTime.TryParseExact(val, "yyyy/MM/dd", null, DateTimeStyles.None, out date))
                                val = date.ToString("s");
                            else if (DateTime.TryParseExact(val, "yyyy-MM-dd", null, DateTimeStyles.None, out date))
                                val = date.ToString("s");
                            else
                            {
                                //record error
                                if (!invalidData.ContainsKey(enode.Name))
                                {
                                    invalidData.Add(enode.Name, new List<string>());
                                }

                                if (!string.IsNullOrEmpty(val))
                                {
                                    if (!invalidData[enode.Name].Contains("Bad date format"))
                                    {
                                        invalidData[enode.Name].Add("Bad date format");
                                    }
                                }
                                else
                                {
                                    if (!invalidData[enode.Name].Contains(val))
                                    {
                                        invalidData[enode.Name].Add(val);
                                    }
                                }
                            }
                        }
                        else if (enode.Name.ToLower() == "idnumber")
                        {
                            bool nonDigit = false;
                            foreach (char c in val.ToCharArray())
                            {
                                if (!char.IsDigit(c))
                                {
                                    nonDigit = true;
                                    break;
                                }
                            }

                            if (nonDigit)
                            {
                                if (!invalidData.ContainsKey(enode.Name))
                                {
                                    invalidData.Add(enode.Name, new List<string>());
                                }

                                if (!invalidData[enode.Name].Contains("IDNumber contains alpha"))
                                {
                                    invalidData[enode.Name].Add("IDNumber contains alpha");
                                }
                            }
                        }

                        XT.WriteElementString(enode.Name, val);
                        AtLeastOneItemWasWritten = true;
                    }
                }

                return AtLeastOneItemWasWritten;
            }

            /// <summary>
            /// Writes the XML file
            /// </summary>
            /// <param name="DT"></param>
            /// <param name="xsdAbs"></param>
            /// <param name="xmlFilePath"></param>
            /// <param name="missingColumns"></param>
            /// <param name="extraColumns"></param>
            /// <param name="invalidData"></param>
            /// <param name="forReal"></param>
            internal static int WriteXmlFileFromDataTable(DataTable DT, XsdAbstraction xsdAbs, string xmlFilePath, ref List<string> missingColumns, ref List<string> extraColumns, ref Dictionary<string, List<string>> invalidData, bool forReal)
            {
                if (DT == null || xsdAbs == null || xsdAbs.ElementNodes.Count == 0)
                    return 0;

                int count = -1;

                if (missingColumns != null)
                    missingColumns.Clear();
                else
                    missingColumns = new List<string>();

                CsvXml.MissingRequiredColumns(DT, xsdAbs.ElementNodes, ref missingColumns);

                if (extraColumns != null)
                    extraColumns.Clear();
                else
                    extraColumns = new List<string>();

                CsvXml.ExtraColumns(DT, xsdAbs, ref extraColumns);

                if (invalidData != null)
                    invalidData.Clear();
                else
                    invalidData = new Dictionary<string, List<string>>();

                XmlTextWriter XT = null;

                if (forReal)
                {
                    XT = new XmlTextWriter(xmlFilePath, Encoding.Unicode);
                    XT.WriteStartDocument(); // + Start
                }

                IList<XsdAbstraction.ElementNode> elements = xsdAbs.ElementNodes;

                if (elements.Count == 1)
                {
                    if (forReal)
                        XT.WriteStartElement(elements[0].Name);
                    elements = elements[0].Elements;
                }
                else if (forReal)
                    XT.WriteStartElement("Root");

                for (int r = 0; r < DT.Rows.Count; r++)
                {
                    DataRow Row = DT.Rows[r];

                    for (int i = 0; i < elements.Count; i++)
                    {
                        XsdAbstraction.ElementNode enode = elements[i];

                        if (WriteElement(XT, DT, Row, xsdAbs, enode, ref invalidData, forReal))
                            count++;
                    }
                }

                if (forReal)
                {
                    XT.WriteEndElement(); // -root
                    XT.WriteEndDocument(); // - Start
                    XT.Flush();
                    XT.Close();
                }

                return count;
            }

            ///// <summary>
            ///// Used for logging and error handeling - Currently not used!!!
            ///// </summary>
            //    public static void WriteLogFile(string logFilePath, List<string> missingColumns, List<string> extraColumns, Dictionary<string, List<string>> invalidData)
            //    {
            //        try
            //        {
            //            using (StreamWriter sw = File.CreateText(logFilePath))
            //            {
            //                sw.WriteLine(DateTime.Now);
            //                sw.WriteLine();

            //                if (missingColumns.Count > 0)
            //                {
            //                    sw.WriteLine("----------------------------------------------------");
            //                    sw.WriteLine("The following columns are missing from the csv file:");
            //                    sw.WriteLine("----------------------------------------------------");

            //                    foreach (string s in missingColumns)
            //                        sw.WriteLine(s);
            //                }
            //                else
            //                {
            //                    sw.WriteLine("All xsd elements are present as columns in the csv file.");
            //                }
            //                sw.WriteLine();
            //                if (extraColumns.Count > 0)
            //                {
            //                    sw.WriteLine("--------------------------------------------------------------");
            //                    sw.WriteLine("The following columns of the csv file are not in the xsd file:");
            //                    sw.WriteLine("--------------------------------------------------------------");

            //                    foreach (string s in extraColumns)
            //                        sw.WriteLine(s);
            //                }
            //                else
            //                {
            //                    sw.WriteLine("The csv file contains no extra columns that are not in the xsd.");
            //                }
            //                sw.WriteLine();
            //                if (invalidData.Count > 0)
            //                {
            //                    sw.WriteLine("----------------------------------------------------------");
            //                    sw.WriteLine("The following fields of the csv file contain invalid data:");
            //                    sw.WriteLine("----------------------------------------------------------");

            //                    foreach (string key in invalidData.Keys)
            //                    {
            //                        string log = "Column: " + key + "\tData values: ";
            //                        //sw.WriteLine("Column : " + key);

            //                        List<string> values = invalidData[key];

            //                        foreach (string s in values)
            //                        {
            //                            if (s == "")
            //                                log += s + "(blank), ";
            //                            else
            //                                log += s + ", ";
            //                        }

            //                        sw.WriteLine(log.Remove(log.Length - 2, 2));
            //                    }
            //                }
            //                else
            //                {
            //                    sw.WriteLine("No invalid data detected.");
            //                }
            //                sw.WriteLine();
            //                sw.WriteLine("LOG PROCESS COMPLETE");
            //                sw.Close();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            throw (new Exception("Error writing Log File:\n" + ex.Message));
            //        }
            //    }
        }
    }
}
