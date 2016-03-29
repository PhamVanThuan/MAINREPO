using System;
using SAHL.Core.Data;

namespace SAHL.Testing.Common.Models
{
    public class CorrespondenceDataModel : IDataModel
    {
        public int Id { get; set; }
        public string CorrespondenceType { get; set; }

        public string CorrespondenceReason { get; set; }

        public string CorrespondenceMedium { get; set; }

        public DateTime Date { get; set; }

        public string UserName { get; set; }

        public string MemoText { get; set; }

        public int GenericKey { get; set; }

        public int GenericKeyTypeKey { get; set; }
    }
}