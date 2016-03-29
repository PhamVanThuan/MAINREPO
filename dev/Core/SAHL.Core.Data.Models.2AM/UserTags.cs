using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class UserTagsDataModel :  IDataModel
    {
        public UserTagsDataModel(string caption, string aDUsername, string backColour, string foreColour, DateTime createDate)
        {
            this.caption = caption;
            this.ADUsername = aDUsername;
            this.BackColour = backColour;
            this.ForeColour = foreColour;
            this.CreateDate = createDate;
		
        }
		[JsonConstructor]
        public UserTagsDataModel(Guid id, string caption, string aDUsername, string backColour, string foreColour, DateTime createDate)
        {
            this.Id = id;
            this.caption = caption;
            this.ADUsername = aDUsername;
            this.BackColour = backColour;
            this.ForeColour = foreColour;
            this.CreateDate = createDate;
		
        }		

        public Guid Id { get; set; }

        public string caption { get; set; }

        public string ADUsername { get; set; }

        public string BackColour { get; set; }

        public string ForeColour { get; set; }

        public DateTime CreateDate { get; set; }
    }
}