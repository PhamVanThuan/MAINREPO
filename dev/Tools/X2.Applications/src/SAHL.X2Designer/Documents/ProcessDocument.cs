using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Xml;
using Northwoods.Go;
using SAHL.X2Designer.Items;
using NuGet;
using System.Linq;

namespace SAHL.X2Designer.Documents
{
    /// <summary>
    /// Summary description for ProcessDocument.
    /// </summary>

    [Serializable]
    public class ProcessDocument : GoDocument
    {
        public List<WorkFlow> m_WorkFlows;

        [NonSerialized]
        public WorkFlow m_CurrentWorkFlow;
        private static Hashtable myDocuments = new Hashtable();
        private String myLocation = "";
        private RoleItemCollection m_GlobalRoles;
        private List<ReferenceItem> _References;
        private List<string> m_UsingStatements;
        private List<string> m_UsedUsingStatements = new List<string>();

        private int buildCount;

        private List<BusinessStageItem> m_BusinessStageItems = new List<BusinessStageItem>();

        private string m_businessStagesConnectionString = "";
        private string _mapVersion = "";

        private bool isLegacy;
        private bool haloV3Viewable;

        private SAHL.X2Designer.AppDomainManagement.AppDomainManager processAppDomainManager;

        private List<NuGetPackageItem> m_NuGetPackages = new List<NuGetPackageItem>();

        public ProcessDocument()
        {
            processAppDomainManager = new AppDomainManagement.AppDomainManager();

            this.MaintainsPartID = true;
            m_WorkFlows = new List<WorkFlow>();
            m_GlobalRoles = new RoleItemCollection();
            RolesCollectionItem mItem = new RolesCollectionItem();
            _References = new List<ReferenceItem>();
            m_UsingStatements = new List<string>();

            foreach (var masterUsingStatement in Properties.Settings.Default.MasterUsingStatements)
            {
                m_UsedUsingStatements.Add(masterUsingStatement);
            }

            m_UsingStatements.AddRange(m_UsedUsingStatements);
        }

        #region Properties

        public SAHL.X2Designer.AppDomainManagement.AppDomainManager ProcessAppDomainManager
        {
            get
            {
                return this.processAppDomainManager;
            }
        }

        public WorkFlow[] WorkFlows
        {
            get { return m_WorkFlows.ToArray(); }
        }

        public WorkFlow CurrentWorkFlow
        {
            get { return m_CurrentWorkFlow; }
        }


        public int BuildCount
        {
            get
            {
                return buildCount;
            }
            set
            {
                buildCount = value;
            }
        }

        public List<RolesCollectionItem> Roles
        {
            get
            {
                return m_GlobalRoles;
            }
        }

        public List<ReferenceItem> References
        {
            get
            {
                _References.TrimExcess();
                return _References;
            }
        }

        public List<BusinessStageItem> BusinessStages
        {
            get
            {
                return m_BusinessStageItems;
            }
            set
            {
                m_BusinessStageItems = value;
            }
        }

        public List<NuGetPackageItem> NuGetPackages
        {
            get
            {
                return m_NuGetPackages;
            }
            set
            {
                m_NuGetPackages = value;
            }
        }

        public List<string> UsingStatements
        {
            get
            {
                return m_UsingStatements;
            }
        }

        public List<string> UsedUsingStatements
        {
            get
            {
                m_UsedUsingStatements.TrimExcess();
                return m_UsedUsingStatements;
            }
        }

        public string BusinessStageConnectionString
        {
            get
            {
                return m_businessStagesConnectionString;
            }
            set
            {
                m_businessStagesConnectionString = value;
            }
        }

        public string MapVersion
        {
            get
            {
                return _mapVersion;
            }
            set
            {
                _mapVersion = value;
            }
        }

        public bool IsLegacy
        {
            get
            {
                return isLegacy;
            }
            set
            {
                isLegacy = value;
            }
        }

        public bool HaloV3Viewable
        {
            get
            {
                return haloV3Viewable;
            }
            set
            {
                haloV3Viewable = value;
            }
        }

        #endregion Properties

        #region Misc Functions
        public void RemoveUsedUsingStatement(string Statement)
        {
            if (m_UsedUsingStatements.Contains(Statement))
                m_UsedUsingStatements.Remove(Statement);
        }

        public void AddUsedUsingStatement(string Statement)
        {
            if (!m_UsedUsingStatements.Contains(Statement))
                m_UsedUsingStatements.Add(Statement);
        }

        public void AddUsingStatement(string Statement)
        {
            if (!m_UsingStatements.Contains(Statement) && (!string.IsNullOrEmpty(Statement)))
                m_UsingStatements.Add(Statement);
        }

        public void AddUsingStatments(List<string> Statements)
        {
            for (int i = 0; i < Statements.Count; i++)
            {
                if (!m_UsingStatements.Contains(Statements[i]) && (!string.IsNullOrEmpty(Statements[i])))
                    m_UsingStatements.Add(Statements[i]);
            }
        }

        public WorkFlow GetWorkFlowForItem(BaseItem Item)
        {
            for (int i = 0; i < m_WorkFlows.Count; i++)
            {
                if (m_WorkFlows[i].Contains(Item))
                    return m_WorkFlows[i];
            }

            return null;
        }

        public void SelectWorkFlow(WorkFlow WorkFlow)
        {
            int WorkFlowIndex = -1;
            for (int i = 0; i < m_WorkFlows.Count; i++)
            {
                if (m_WorkFlows[i].Equals(WorkFlow))
                {
                    WorkFlowIndex = i;
                }
                m_WorkFlows[i].Collapse();
            }

            if (WorkFlowIndex != -1)
            {
                m_WorkFlows[WorkFlowIndex].Expand();
            }
            m_CurrentWorkFlow = WorkFlow;
        }

        public void CreateWorkFlow(string openMethod, string Name)
        {
            this.StartTransaction();

            Debug.WriteLine("Start Create WorkFlow");
            WorkFlow m_WorkFlowItem;
            if (Name == null)
            {
                m_WorkFlowItem = new WorkFlow("WorkFlow" + m_WorkFlows.Count + 1, new PointF(WorkFlow.WorkFlowLeft + (150 * (m_WorkFlows.Count)), WorkFlow.WorkFlowCollapsedTop));
            }
            else
            {
                m_WorkFlowItem = new WorkFlow(Name, new PointF(WorkFlow.WorkFlowLeft + (150 * (m_WorkFlows.Count)), WorkFlow.WorkFlowCollapsedTop));
            }

            this.Add(m_WorkFlowItem);
            if (m_WorkFlows.Count == 0)
            {
                m_WorkFlows.Add(m_WorkFlowItem);
            }

            if (openMethod == "new")
            {
                m_WorkFlowItem.CreateInvisibleAnchorNode(new Point(5, 5));
                m_WorkFlowItem.CreateClapperBoard(new PointF(20, 20));

                if (MainForm.App.m_BrowserView != null)
                {
                    MainForm.App.m_BrowserView.SetupBrowser();
                }
            }
            else
            {
                PointF WorkFlowPos = m_WorkFlowItem.Position;
            }
            m_WorkFlowItem.Label.Width = 125;

            SelectWorkFlow(m_WorkFlowItem);

            this.FinishTransaction("Created New WorkFlow");
        }

		public void AddNuGetPackage(string packageName, string version, string dependsOn, string dependsOnVersion)
		{
			m_NuGetPackages.Add(new NuGetPackageItem(packageName, version, dependsOn, dependsOnVersion));
		}

        public void AddReference(ReferenceItem ri)
        {
            if (!_References.Contains(ri))
                _References.Add(ri);
        }

        /// <summary>
        /// Gets a list of refernces from the remote domain and merges any new references into the
        /// main list contained in the procedure document.
        /// </summary>
        /// <param name="References"></param>
        private void AddReferences(List<ReferenceItem> References)
        {
            if (null == References) return;

            // add this to the TOTAL list of references
            for (int i = 0; i < References.Count; i++)
            {
                bool FoundMatch = false;
                for (int j = 0; j < _References.Count; j++)
                {
                    if (References[i].FullName == _References[j].FullName)
                    {
                        FoundMatch = true;
                        break;
                    }
                }

                if (!FoundMatch)
                    _References.Add(References[i]);
            }
        }

      
        public void DeleteWorkFlow(WorkFlow workFlowToDelete)
        {
            for (int x = 0; x < m_WorkFlows.Count; x++)
            {
                if (m_WorkFlows[x] == workFlowToDelete)
                {
                    m_WorkFlows.RemoveAt(x);
                }
            }
        }

        #endregion Misc Functions

        #region Save Functions

        public String Location
        {
            get { return myLocation; }
            set
            {
                String old = myLocation;
                if (old != value)
                {
                    RemoveDocument(old);
                    myLocation = value;
                    AddDocument(value, this);
                    RaiseChanged(ChangedLocation, 0, null, 0, old, NullRect, 0, value, NullRect);
                }
            }
        }

        internal static void RemoveDocument(String location)
        {
            myDocuments.Remove(location);
        }

        internal static void AddDocument(String location, ProcessDocument doc)
        {
            myDocuments[location] = doc;
        }

        public const int ChangedLocation = LastHint + 23;

        #endregion Save Functions
    }

    [Serializable]
    public class ReferenceItem : MarshalByRefObject
    {
        public ReferenceItem()
        {

        }

        public ReferenceItem(XmlNode xn)
        {
            if (null != xn.Attributes["FullName"])
            {
                m_FullName = xn.Attributes["FullName"].Value;
                this.m_Name = FullName.ToString().Substring(0, m_FullName.IndexOf(","));
                int startVer = m_FullName.IndexOf("Version") + 8;
                int endVer = m_FullName.IndexOf(",", startVer);
                string ver = m_FullName.Substring(startVer, endVer - startVer);
                this.m_Version = ver;
            }
            else
            {
                m_Name = xn.Attributes["Name"].Value;
                m_Version = "0.0.0";
            }

            string FilePath = "";
            if (null != xn.Attributes["Path"])
            {
                _SavePath = xn.Attributes["Path"].Value;
                if (FilePath != "")
                    m_Path = string.Format("{0}\\{1}", FilePath, _SavePath);
                else
                    m_Path = string.Format("{0}\\{1}", BuildLocation, _SavePath);
            }
        }

        public static string ReturnVersion(string FullName)
        {
            int startVer = FullName.IndexOf("Version") + 8;
            int endVer = FullName.IndexOf(",", startVer);
            string ver = FullName.Substring(startVer, endVer - startVer);
            return ver;
        }

        public ReferenceItem(string FullName, string Path)
        {
            this.m_Path = Path;
            this.m_FullName = FullName;
            this.m_Name = FullName.ToString().Substring(0, m_FullName.IndexOf(","));
            this.m_Version = ReturnVersion(FullName);
            this._SavePath = Name + ".dll";
        }

        private string m_Name = "";
        private string m_FullName = "";
        private string m_Path = "";
        private string BuildLocation = "";
        private string _SavePath = "";
        private string m_Version = "";

        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        public string FullName
        {
            get
            {
                return m_FullName;
            }
            set
            {
                m_FullName = value;
            }
        }

        public string Version
        {
            get
            {
                return m_Version;
            }
            set
            {
                m_Version = value;
            }
        }

        public string SavePath
        {
            get
            {
                return _SavePath;
            }
        }

        public string FullPath
        {
            get
            {
                return m_Path;
            }
            set
            {
                m_Path = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class NuGetPackageItem
    {
        public string PackageID { get; set; }
        public string PackageVersion { get; set; }
		public string DependsOn { get; set; }
		public string DependsOnVersion { get; set; }
		public NuGetPackageItem(string packageId, string packageVersion, string dependsOn, string dependsOnVersion)
		{
			this.PackageID = packageId;
			this.PackageVersion = packageVersion;
			this.DependsOn = dependsOn;
			this.DependsOnVersion = dependsOnVersion;
		}
    }

    public class BusinessStageItem
    {
        private string m_SDSDGKey;
        private string m_DefinitionGroupDescription;
        private string m_DefinitionDescription;

        public string SDSDGKey
        {
            get
            {
                return m_SDSDGKey;
            }
            set
            {
                m_SDSDGKey = value;
            }
        }

        public string DefinitionGroupDescription
        {
            get
            {
                return m_DefinitionGroupDescription;
            }
            set
            {
                m_DefinitionGroupDescription = value;
            }
        }

        public string DefinitionDescription
        {
            get
            {
                return m_DefinitionDescription;
            }
            set
            {
                m_DefinitionDescription = value;
            }
        }
    }
}