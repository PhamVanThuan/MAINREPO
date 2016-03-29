using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Transactions;
using SAHL.Core;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Logging;
using SAHL.Core.X2;
using SAHL.Core.X2.AppDomain;
using SAHL.X2Engine2.Node.Providers;
using SAHL.X2Engine2.Providers;

namespace SAHL.X2Engine2.Node.AppDomain
{
    [Serializable]
    public class ProcessInstantiator : IProcessInstantiator
    {
        private IWorkflowDataProvider workflowDataProvider;
        private IAppDomainProcessProxyCache appDomainProcessProxyCache;
        private IAppDomainFactory appDomainFactory;
        private IAppDomainProcessProxyFactory appDomainProcessProxyFactory;
        private IAppDomainFileManager appDomainFileManager;
        private INugetRetriever nugetRetriever;
        private List<IParticipatesInThreadLocalStorage> participants;
        private IFileSystem fileSystem;
        ILogger logger;
        ILoggerSource loggerSource;
        private static object syncObj = new object();

        public ProcessInstantiator(IWorkflowDataProvider workflowDataProvider, IAppDomainProcessProxyCache appDomainProcessProxyCache, INugetRetriever nugetRetriever, IAppDomainFileManager appDomainFileManager, IX2NodeConfigurationProvider x2NodeConfigurationProvider, IAppDomainFactory appDomainFactory, IAppDomainProcessProxyFactory appDomainProcessProxyFactory,
            IFileSystem fileSystem, ILogger logger, ILoggerSource loggerSource)
        {
            this.workflowDataProvider = workflowDataProvider;
            this.appDomainProcessProxyCache = appDomainProcessProxyCache;
            this.nugetRetriever = nugetRetriever;
            this.appDomainFileManager = appDomainFileManager;
            this.appDomainFactory = appDomainFactory;
            this.appDomainProcessProxyFactory = appDomainProcessProxyFactory;
            this.fileSystem = fileSystem;
            this.logger = logger;
            this.loggerSource = loggerSource;
            participants = new List<IParticipatesInThreadLocalStorage>();
            participants.Add(logger);
        }

        public Core.X2.IX2Process GetProcess(int processId)
        {
            IAppDomainProcessProxy proxy = null;
            ProcessDataModel processDataModel = workflowDataProvider.GetProcessById(processId);
            if (appDomainProcessProxyCache.ContainsProxy(processId))
            {
                proxy = appDomainProcessProxyCache.GetProxy(processId);
                Transaction currentTransaction = Transaction.Current;
                proxy.SetTransaction(currentTransaction);
                return GetProcess(proxy, processDataModel.Name);
            }
            else
            {
                lock (syncObj)
                {
                    if (appDomainProcessProxyCache.ContainsProxy(processId))
                    {
                        proxy = appDomainProcessProxyCache.GetProxy(processId);
                        return GetProcess(proxy, processDataModel.Name);
                    }

                    IEnumerable<ProcessAssemblyNugetInfoDataModel> nugetPackages = workflowDataProvider.GetProcessAssemblyNuGetInfoByProcessId(processId);
                    IEnumerable<ProcessAssemblyDataModel> assemblies = workflowDataProvider.GetProcessAssemblies(processId);
                    string directory = "dotnetX2Processes\\" + processDataModel.ID;
                    string directoryFullPath = fileSystem.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, directory);
                    List<string> FilesToLoadInNewDomain = new List<string>();
                    string configFileName = fileSystem.Path.Combine(directoryFullPath, processDataModel.Name + ".config");
                    if (!fileSystem.File.Exists(configFileName))
                    {
                        appDomainFileManager.WriteConfigFile(processDataModel.ConfigFile, configFileName);
                    }

                    foreach (var assembly in assemblies)
                    {
                        string DestinationPath = fileSystem.Path.Combine(directoryFullPath, assembly.DLLName);
                        appDomainFileManager.WriteFile(assembly.DLLData, DestinationPath);
                        FilesToLoadInNewDomain.Add(assembly.DLLName);
                    }

                    string nugetPackagePath = fileSystem.Path.Combine(directoryFullPath, "nuget");

                    if (!fileSystem.Directory.Exists(nugetPackagePath))
                        fileSystem.Directory.CreateDirectory(nugetPackagePath);

                    foreach (ProcessAssemblyNugetInfoDataModel package in nugetPackages)
                    {
                        nugetRetriever.InstallPackage(package.PackageName, nugetPackagePath, package.PackageVersion);
                    }

                    System.AppDomain newDomain = appDomainFactory.Create(processDataModel, directory, directoryFullPath);

                    List<string> Files = appDomainFileManager.GetInstalledNuGetFiles(nugetPackagePath);

                    foreach (string file in Files)
                    {
                        string FileName = fileSystem.Path.GetFileName(file);
                        string DestinationPath = fileSystem.Path.Combine(directoryFullPath, FileName);
                        appDomainFileManager.CopyFile(file, DestinationPath);
                        FilesToLoadInNewDomain.Add(FileName);
                    }

                    proxy = appDomainProcessProxyFactory.Create(newDomain);

                    foreach (string file in FilesToLoadInNewDomain)
                    {
                        try
                        {
                            proxy.LoadAssembly(fileSystem.Path.Combine(directoryFullPath, file));
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine(ex.Message);
                        }
                    }

                    var process = GetProcess(proxy, processDataModel.Name); // this will call CreateStartables
                    //proxy.CreateStartables(processDataModel.Name);
                    Transaction currentTransaction = Transaction.Current;
                    proxy.SetTransaction(currentTransaction);
                    //proxy.CreateStartables(processDataModel.Name);

                    appDomainProcessProxyCache.AddProxy(processId, proxy);

                    if (process is IX2TLSLegacyProcess)
                    {
                        return new X2TLSLegacyProcess(process, proxy, logger);
                    }
                    else
                    {
                        return new X2TLSProcess(process, proxy, logger);
                    }
                    //return proxy.GetProcess(processDataModel.Name.Replace(" ", "_"));
                }
            }
        }

        public void UnloadAll()
        {
            throw new NotImplementedException();
        }

        private IX2Process GetProcess(IAppDomainProcessProxy proxy, string processName)
        {
            proxy.ClearThreadLocalStore();

            var result = proxy.GetProcess(processName.Replace(" ", "_"));
			try
			{
				foreach (var participant in participants)
				{
					Type type = participant.GetType();
					proxy.SetThreadLocalStore(participant.GetThreadLocalStore(), type);
				}
			}
			catch (Exception ex)
			{
				logger.LogErrorWithException(this.loggerSource, "X2 User", "SetThreadLocalStore", ex.Message, ex);
			}

            if (result is IX2LegacyProcess)
            {
                return new X2TLSLegacyProcess(result, proxy, logger);
            }
            else
            {
                return new X2TLSProcess(result, proxy, logger);
            }
        }
    }
}