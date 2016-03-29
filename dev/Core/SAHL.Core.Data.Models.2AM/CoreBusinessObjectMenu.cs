using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class CoreBusinessObjectMenuDataModel :  IDataModel
    {
        public CoreBusinessObjectMenuDataModel(int coreBusinessObjectKey, int? parentKey, string description, string nodeType, string uRL, string statementNameKey, int sequence, string menuIcon, int featureKey, bool hasOriginationSource, bool isRemovable, int expandLevel, bool includeParentHeaderIcons, int? genericKeyTypeKey)
        {
            this.CoreBusinessObjectKey = coreBusinessObjectKey;
            this.ParentKey = parentKey;
            this.Description = description;
            this.NodeType = nodeType;
            this.URL = uRL;
            this.StatementNameKey = statementNameKey;
            this.Sequence = sequence;
            this.MenuIcon = menuIcon;
            this.FeatureKey = featureKey;
            this.HasOriginationSource = hasOriginationSource;
            this.IsRemovable = isRemovable;
            this.ExpandLevel = expandLevel;
            this.IncludeParentHeaderIcons = includeParentHeaderIcons;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }		

        public int CoreBusinessObjectKey { get; set; }

        public int? ParentKey { get; set; }

        public string Description { get; set; }

        public string NodeType { get; set; }

        public string URL { get; set; }

        public string StatementNameKey { get; set; }

        public int Sequence { get; set; }

        public string MenuIcon { get; set; }

        public int FeatureKey { get; set; }

        public bool HasOriginationSource { get; set; }

        public bool IsRemovable { get; set; }

        public int ExpandLevel { get; set; }

        public bool IncludeParentHeaderIcons { get; set; }

        public int? GenericKeyTypeKey { get; set; }
    }
}