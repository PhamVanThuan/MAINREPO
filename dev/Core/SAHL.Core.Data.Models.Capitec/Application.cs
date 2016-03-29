using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
    [Serializable]
    public partial class ApplicationDataModel :  IDataModel
    {
        public ApplicationDataModel(DateTime applicationDate, Guid applicationPurposeEnumId, int? applicationNumber, Guid userId, Guid? addressId, Guid? applicationStageTypeEnumId, Guid applicationStatusEnumId, DateTime lastStatusChangeDate, string consultantName, string consultantContactNumber, DateTime? captureStartTime, DateTime? captureEndTime, Guid? branchId)
        {
            this.ApplicationDate = applicationDate;
            this.ApplicationPurposeEnumId = applicationPurposeEnumId;
            this.ApplicationNumber = applicationNumber;
            this.UserId = userId;
            this.AddressId = addressId;
            this.ApplicationStageTypeEnumId = applicationStageTypeEnumId;
            this.ApplicationStatusEnumId = applicationStatusEnumId;
            this.LastStatusChangeDate = lastStatusChangeDate;
            this.ConsultantName = consultantName;
            this.ConsultantContactNumber = consultantContactNumber;
            this.CaptureStartTime = captureStartTime;
            this.CaptureEndTime = captureEndTime;
            this.BranchId = branchId;
		
        }

        public ApplicationDataModel(Guid id, DateTime applicationDate, Guid applicationPurposeEnumId, int? applicationNumber, Guid userId, Guid? addressId, Guid? applicationStageTypeEnumId, Guid applicationStatusEnumId, DateTime lastStatusChangeDate, string consultantName, string consultantContactNumber, DateTime? captureStartTime, DateTime? captureEndTime, Guid? branchId)
        {
            this.Id = id;
            this.ApplicationDate = applicationDate;
            this.ApplicationPurposeEnumId = applicationPurposeEnumId;
            this.ApplicationNumber = applicationNumber;
            this.UserId = userId;
            this.AddressId = addressId;
            this.ApplicationStageTypeEnumId = applicationStageTypeEnumId;
            this.ApplicationStatusEnumId = applicationStatusEnumId;
            this.LastStatusChangeDate = lastStatusChangeDate;
            this.ConsultantName = consultantName;
            this.ConsultantContactNumber = consultantContactNumber;
            this.CaptureStartTime = captureStartTime;
            this.CaptureEndTime = captureEndTime;
            this.BranchId = branchId;
		
        }		

        public Guid Id { get; set; }

        public DateTime ApplicationDate { get; set; }

        public Guid ApplicationPurposeEnumId { get; set; }

        public int? ApplicationNumber { get; set; }

        public Guid UserId { get; set; }

        public Guid? AddressId { get; set; }

        public Guid? ApplicationStageTypeEnumId { get; set; }

        public Guid ApplicationStatusEnumId { get; set; }

        public DateTime LastStatusChangeDate { get; set; }

        public string ConsultantName { get; set; }

        public string ConsultantContactNumber { get; set; }

        public DateTime? CaptureStartTime { get; set; }

        public DateTime? CaptureEndTime { get; set; }

        public Guid? BranchId { get; set; }
    }
}