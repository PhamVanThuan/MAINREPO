using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using SAHL.Tools.Workflow.Common.AppDomainManagement;
using SAHL.Tools.Workflow.Common.Persistance;
using SAHL.Tools.Workflow.Common.Persistance.XmlPersistanceStore;
using SAHL.Tools.Workflow.Common.WorkflowElements;
using System;

namespace SAHL.Tools.Workflow.Common.ReferenceChecking
{
    public class ReferenceChecker
    {
        public void RemoveAndReAddReferences(Process process, Options options)
        {
            // we need to remove all the references here and re add them from our nuget packages folder
            string workflowMap = options.WorkflowMapPath;
            if (File.Exists(workflowMap))
            {
                string rootDirectory = options.RootPath;

                // setup the referenced assembly directories
                string binariesPath = options.BinariesPath;

                using (DomainManager dm = new DomainManager(rootDirectory))
                {
                    // get a list of all the assemblies located in the NuGet binaries folder
                    IDictionary<string, string> references = GetAssembliesFromNuGetFolders(binariesPath);

                    // load the referenced dll's into the app domain
                    foreach (var assemblyRef in references)
	                {
                        dm.ReflectionLoadAssembly(assemblyRef.Value);
                    }

                    // get the references xml node and clear all the references out
                    XDocument doc = null;
                    using (FileStream fs = new FileStream(workflowMap, FileMode.Open, FileAccess.ReadWrite))
                    {
                        doc = XDocument.Load(fs);
                    }
                    XElement processXml = doc.Element("ProcessName");
                    XElement referencesXml = processXml.Element("References");
                    if (referencesXml != null)
                    {
                        referencesXml.RemoveNodes();


                        IOrderedEnumerable<KeyValuePair<string, string>> references2 = dm.AssemblyReflectionCache.OrderBy(x => x.Key);

                        // add each reference to the xml
                        foreach (KeyValuePair<string, string> kvp in references2)
                        {
                            string dllName = kvp.Key.Substring(0, kvp.Key.IndexOf(",")) + ".dll";
                            XElement referenceXml = new XElement("Reference",
                                new XAttribute("FullName", kvp.Key),
                                new XAttribute("Path", kvp.Value.Replace(options.BinariesPath, String.Empty).Trim('\\')),
                                new XAttribute("Name", dllName),
                                new XAttribute("Version", GetVersionFromAssemblyFullName(kvp.Key))
                                );
                            referencesXml.Add(referenceXml);
                        }
                    }

                    // save the xml file
                    doc.Save(workflowMap);
                }
            }
        }

        public IDictionary<string, string> GetAssembliesFromNuGetFolders(string nugetPackagePath)
        {
            IDictionary<string, string> references = new Dictionary<string, string>();

            string[] directories = Directory.GetDirectories(nugetPackagePath, "*", SearchOption.TopDirectoryOnly);

            for (int i = 0; i < directories.Length; i++)
            {
                string folderToGetDllsFrom = directories[i];

                // look for a lib folder and anysub folders with 'net'
				if(!Directory.Exists(directories[i] + "\\lib"))
				{
					continue;
				}
                List<string> subFolders = new List<string>(Directory.GetDirectories(directories[i] + "\\lib", "net*"));

                var orderedSub = subFolders.OrderByDescending(x => x);
               
                // look for the higest version and only extract that
                if (orderedSub.Count() > 0)
                {
                    folderToGetDllsFrom = orderedSub.First();
                }
                // get the files and add to our collection
                string[] files = Directory.GetFiles(folderToGetDllsFrom, "*.dll", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    // get the filename
                    string fileName = Path.GetFileName(file);

                    // load the dll into the collection if it doesnt already exist
                    if (!references.ContainsKey(fileName))
                        references.Add(fileName, file);

                }
            }

            return references;
        }

        public void CheckAndUpdateReferences(Options options)
        {
            Process process = GetProcessFromFile(options.WorkflowMapPath);
            RemoveAndReAddReferences(process, options);
        }

        private Process GetProcessFromFile(string pathToWorkflowFile)
        {
            using (FileStream fs = new FileStream(pathToWorkflowFile, FileMode.Open, FileAccess.Read))
            {
                IWorkflowPersistanceStore store = new XmlStreamPersistanceStore(fs);

                Process process = store.LoadProcess();
                return process;
            }
        }

        private string GetVersionFromAssemblyFullName(string assemblyName)
        {
            int start = assemblyName.IndexOf("Version=") + 8;
            int end = assemblyName.IndexOf(",", start);
            return assemblyName.Substring(start, end - start);
        }
    }
}