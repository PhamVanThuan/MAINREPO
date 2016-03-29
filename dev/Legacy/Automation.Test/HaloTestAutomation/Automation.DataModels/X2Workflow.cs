using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class X2Workflow : IDataModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string StorageTable { get; set; }

        public string StorageKey { get; set; }

        public GenericKeyTypeEnum GenericKeyTypeKey { get; set; }

        public DateTime CreateDate { get; set; }
    }
}