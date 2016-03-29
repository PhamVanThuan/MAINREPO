using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using SAHL.Tools.Workflow.Common.Database.Properties;
using SAHL.Tools.Workflow.Common.Database.Templates;
using SAHL.Tools.Workflow.Common.Persistance;
using SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore;
using dbElements = SAHL.Tools.Workflow.Common.Database.WorkflowElements;
using xmlElements = SAHL.Tools.Workflow.Common.WorkflowElements;
using recalculator = SAHL.Tools.Workflow.Common.Database.SecurityRecalculating;
using System.Xml;
using NuGet;
using System.Reflection;

namespace SAHL.Tools.Workflow.Common.Database.Publishing
{
    public class Publisher
    {
        public const string TargetEnvironmentConnection = "TargetEnvironmentFailure";

        private PrePublishChecker checker;
        private ProcessFromXmlGenerator processFromXmlGenerator;

        public Publisher(PrePublishChecker checker, ProcessFromXmlGenerator processFromXmlGenerator)
        {
            this.checker = checker;
            this.processFromXmlGenerator = processFromXmlGenerator;
        }

        public IEnumerable<PublisherError> PerfromPrePublishChecks(ProcessOption processOption)
        {
            // load the process to check
            using (FileStream fs = new FileStream(processOption.PathToProcessFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkflowPersistanceStore store = new XmlStreamPersistanceStore(fs);

                xmlElements.Process process = store.LoadProcess();

                return this.checker.CheckProcess(process);
            }
        }

        public void SaveProcessToDB(ISession session, dbElements.Process dbProcess)
        {
            session.Save(dbProcess);
        }

        public void MigrateWorkflow(ISession session,int newWorkflowID,int timeout)
        {
            ISQLQuery query = session.CreateSQLQuery(Resources.MapStates);
            query.SetParameter("NewWorkFlowID", newWorkflowID);
            query.SetTimeout(timeout);
            query.ExecuteUpdate();
        }

        public void CreateMigrationIndexes(ISession session, int timeout)
        {
            ISQLQuery query = session.CreateSQLQuery(Resources.createindexes);
            query.SetTimeout(timeout);
            query.ExecuteUpdate();
        }

        public void DropMigrationIndexes(ISession session, int timeout)
        {
            ISQLQuery query = session.CreateSQLQuery(Resources.dropindexes);
            query.SetTimeout(timeout);
            query.ExecuteUpdate();
        }

        private void CreateCustomVariables(ISession session, xmlElements.Workflow workflow)
        {
            CustomX2Data data = new CustomX2Data(workflow);
            string sqlQuery = data.TransformText();
            ISQLQuery query = session.CreateSQLQuery(sqlQuery);
            query.ExecuteUpdate();
        }

        private static ISessionFactory InitialiseActiveRecord(string connectionString)
        {
            NHibernateInitialiser ARInit = new NHibernateInitialiser(connectionString);
            return ARInit.InitialiseNHibernate();
        }

        public void PublishProcess(string processPath, string binaryPath, string connectionString)
        {
            int noOFinstancesRecalculated = 0;
            PublishProcess(processPath, binaryPath, connectionString, out noOFinstancesRecalculated);
        }
        public void PublishProcess(string processPath, string binaryPath, string connectionString, out int noOFinstancesRecalculated)
        {
            string mapName = Path.GetFileNameWithoutExtension(processPath);
            string directory = Path.GetDirectoryName(processPath);
            string dllFileName = mapName.Replace(" ", "");
            string dllFullPath = Path.Combine(binaryPath, dllFileName) + ".dll";
            if (!File.Exists(dllFullPath))
            {
                dllFullPath = Path.Combine(directory, dllFileName) + ".dll";
                if (!File.Exists(dllFullPath))
                {
                    throw new Exception("Can't find compiled workflow");
                }
            }

            ISessionFactory factory = InitialiseActiveRecord(connectionString);

            noOFinstancesRecalculated = 0;

            using (ISession session = factory.OpenSession())
            {
                dbElements.Process dbProcess = null;
                dbElements.Process oldProcess = null;

                using (ITransaction trans = session.BeginTransaction())
                {
                    // if all is well load up the process
                    xmlElements.Process xmlProcess = null;
                    using (FileStream fs = new FileStream(processPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        IWorkflowPersistanceStore store = new XmlStreamPersistanceStore(fs);

                        xmlProcess = store.LoadProcess();
                    }

                    // convert the process to a dbprocess
                    List<dbElements.SecurityGroup> fixedSecurityGroups = new List<dbElements.SecurityGroup>();

                    fixedSecurityGroups = session.Query<dbElements.SecurityGroup>().Where(x => x.Process == null).ToList();

                    dbProcess = this.processFromXmlGenerator.GenerateFromXml(xmlProcess, fixedSecurityGroups);

                    // get the previous process if there is one
                    oldProcess = session.Query<dbElements.Process>().Where(y => y.Name == dbProcess.Name).OrderByDescending(x => x.Id).FirstOrDefault();

                    // add the designer data
                    byte[] processFileData = null;
                    this.processFromXmlGenerator.GetFileData(ref processFileData, processPath);
                    dbProcess.DesignerData = processFileData;

                    // get the maps config file and save it in the process table
                    string configFileName = Path.Combine(directory, mapName + ".config");
                    XmlDocument configFile = new XmlDocument();
                    configFile.Load(configFileName);
                    dbProcess.ConfigFile = configFile.InnerXml;

                    // add the process assembly to the ProcessAssembly db table
                    string primaryAssemblyPath = dllFullPath;
                    string dllName = Path.GetFileName(primaryAssemblyPath);
                    string buildDir = Path.GetDirectoryName(primaryAssemblyPath);
                    this.processFromXmlGenerator.GetFileData(ref processFileData, dllFullPath);
                    dbElements.ProcessAssembly primaryAssembly = new dbElements.ProcessAssembly()
                    {
                        Process = dbProcess,
                        Dllname = dllName,
                        Dlldata = processFileData
                    };
                    dbProcess.ProcessAssemblies.Add(primaryAssembly);

                    // get a list of nuget packages from the x2p file
                    IDictionary<string, SemanticVersion> nugetPackages = Compiler.Compiler.GetNuGetPackagesFromMap(processPath);

                    // add the nuget package info into the ProcessAssemblyNuGetInfo db table
                    foreach (var nugetPackage in nugetPackages)
                    {
                        dbElements.ProcessAssemblyNuGetInfo nugetAssemblyInfo = new dbElements.ProcessAssemblyNuGetInfo()
                        {
                            Process = dbProcess,
                            PackageName = nugetPackage.Key,
                            PackageVersion = nugetPackage.Value.ToString()
                        };
                        dbProcess.ProcessAssemblyNuGetInfos.Add(nugetAssemblyInfo);
                    }

                    // save the new process
                    this.SaveProcessToDB(session, dbProcess);

                    // create custom variables
                    foreach (xmlElements.Workflow workflow in xmlProcess.Workflows)
                    {
                        CreateCustomVariables(session, workflow);
                    }

                    IList<dbElements.WorkFlow> newWorkflows = (session.Query<dbElements.Process>().OrderByDescending(x => x.Id).FirstOrDefault()).WorkFlows;

                    // migrate the process
                    foreach (dbElements.WorkFlow w in newWorkflows)
                    {
                        MigrateWorkflow(session, w.Id, 36000);
                    }

                    trans.Commit();
                }
            }
        }
    }
}