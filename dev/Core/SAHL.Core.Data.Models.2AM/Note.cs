using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models._2AM
{
    [Serializable]
    public partial class NoteDataModel :  IDataModel, IDataModelWithIdentitySeed
    {
        public NoteDataModel(int genericKeyTypeKey, int genericKey, DateTime? diaryDate)
        {
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.DiaryDate = diaryDate;
		
        }
		[JsonConstructor]
        public NoteDataModel(int noteKey, int genericKeyTypeKey, int genericKey, DateTime? diaryDate)
        {
            this.NoteKey = noteKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.GenericKey = genericKey;
            this.DiaryDate = diaryDate;
		
        }		

        public int NoteKey { get; set; }

        public int GenericKeyTypeKey { get; set; }

        public int GenericKey { get; set; }

        public DateTime? DiaryDate { get; set; }

        public void SetKey(int key)
        {
            this.NoteKey =  key;
        }
    }
}