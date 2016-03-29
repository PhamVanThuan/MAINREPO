using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class ProcessDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public ProcessDataModel(int? processAncestorID, string name, string version, byte[] designerData, DateTime createDate, string mapVersion, string configFile, string viewableOnUserInterfaceVersion, bool isLegacy)
        {
            this.ProcessAncestorID = processAncestorID;
            this.Name = name;
            this.Version = version;
            this.DesignerData = designerData;
            this.CreateDate = createDate;
            this.MapVersion = mapVersion;
            this.ConfigFile = configFile;
            this.ViewableOnUserInterfaceVersion = viewableOnUserInterfaceVersion;
            this.IsLegacy = isLegacy;
		
        }
		[JsonConstructor]
        public ProcessDataModel(int iD, int? processAncestorID, string name, string version, byte[] designerData, DateTime createDate, string mapVersion, string configFile, string viewableOnUserInterfaceVersion, bool isLegacy)
        {
            this.ID = iD;
            this.ProcessAncestorID = processAncestorID;
            this.Name = name;
            this.Version = version;
            this.DesignerData = designerData;
            this.CreateDate = createDate;
            this.MapVersion = mapVersion;
            this.ConfigFile = configFile;
            this.ViewableOnUserInterfaceVersion = viewableOnUserInterfaceVersion;
            this.IsLegacy = isLegacy;
		
        }		

        public int ID { get; set; }

        public int? ProcessAncestorID { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public byte[] DesignerData { get; set; }

        public DateTime CreateDate { get; set; }

        public string MapVersion { get; set; }

        public string ConfigFile { get; set; }

        public string ViewableOnUserInterfaceVersion { get; set; }

        public bool IsLegacy { get; set; }

        public void SetKey(int key)
        {
            this.ID =  key;
        }
    }
}