using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.EventProjection
{
    [Serializable]
    public partial class CorrespondenceDataModel :  IDataModel, IDataModelWithIdentitySeed, IDataModelWithPrimaryKeyId 
    {
        public CorrespondenceDataModel(string correspondenceType, string correspondenceReason, string correspondenceMedium, DateTime date, string userName, string memoText, int genericKey, int genericKeyTypeKey)
        {
            this.CorrespondenceType = correspondenceType;
            this.CorrespondenceReason = correspondenceReason;
            this.CorrespondenceMedium = correspondenceMedium;
            this.Date = date;
            this.UserName = userName;
            this.MemoText = memoText;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }
		[JsonConstructor]
        public CorrespondenceDataModel(int id, string correspondenceType, string correspondenceReason, string correspondenceMedium, DateTime date, string userName, string memoText, int genericKey, int genericKeyTypeKey)
        {
            this.Id = id;
            this.CorrespondenceType = correspondenceType;
            this.CorrespondenceReason = correspondenceReason;
            this.CorrespondenceMedium = correspondenceMedium;
            this.Date = date;
            this.UserName = userName;
            this.MemoText = memoText;
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
		
        }		

        public int Id { get; set; }

        public string CorrespondenceType { get; set; }

        public string CorrespondenceReason { get; set; }

        public string CorrespondenceMedium { get; set; }

        public DateTime Date { get; set; }

        public string UserName { get; set; }

        public string MemoText { get; set; }

        public int GenericKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public void SetKey(int key)
        {
            this.Id =  key;
        }
    }
}