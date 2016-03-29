using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class UiWsMenuDataModel :  IDataModel
    {
        public UiWsMenuDataModel(string menuKey, string name, string link, bool active, string parent, int displayOrder)
        {
            this.MenuKey = menuKey;
            this.Name = name;
            this.Link = link;
            this.Active = active;
            this.Parent = parent;
            this.DisplayOrder = displayOrder;
		
        }		

        public string MenuKey { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public bool Active { get; set; }

        public string Parent { get; set; }

        public int DisplayOrder { get; set; }
    }
}