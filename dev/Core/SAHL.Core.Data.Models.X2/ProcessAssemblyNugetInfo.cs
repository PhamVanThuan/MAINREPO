using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class ProcessAssemblyNugetInfoDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ProcessAssemblyNugetInfoDataModel(int processID, string packageName, string packageVersion)
        {
            this.ProcessID = processID;
            this.PackageName = packageName;
            this.PackageVersion = packageVersion;
		
        }
		[JsonConstructor]
        public ProcessAssemblyNugetInfoDataModel(int iD, int processID, string packageName, string packageVersion)
        {
            this.ID = iD;
            this.ProcessID = processID;
            this.PackageName = packageName;
            this.PackageVersion = packageVersion;
		
        }		

        public int ID { get; set; }

        public int ProcessID { get; set; }

        public string PackageName { get; set; }

        public string PackageVersion { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}