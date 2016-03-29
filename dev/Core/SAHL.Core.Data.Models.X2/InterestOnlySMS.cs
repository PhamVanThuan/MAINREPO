using System;
using SAHL.Core.Data;
using Newtonsoft.Json;

namespace SAHL.Core.Data.Models.X2
{
    [Serializable]
    public partial class InterestOnlySMSDataModel :  IDataModel
    {
        public InterestOnlySMSDataModel(long instanceID, double? bondAmount, string bondType, DateTime? callBackAt, string cellNumber, string consultant, string declineReason, double? estimatedAMInstallment, double? estimatedInstallment, bool? existingClient, string firstNames, string homeAddressLine1, string homeAddressLine2, string homeAddressLine3, string homeAddressLine4, string homeContactNo, string lastName, double? loanTerm, double? outstandingBalance, double? requiredAmount, string salutation, string transferBranch, string transferRegion, string transferType, string workContactNo)
        {
            this.InstanceID = instanceID;
            this.BondAmount = bondAmount;
            this.BondType = bondType;
            this.CallBackAt = callBackAt;
            this.CellNumber = cellNumber;
            this.Consultant = consultant;
            this.DeclineReason = declineReason;
            this.EstimatedAMInstallment = estimatedAMInstallment;
            this.EstimatedInstallment = estimatedInstallment;
            this.ExistingClient = existingClient;
            this.FirstNames = firstNames;
            this.HomeAddressLine1 = homeAddressLine1;
            this.HomeAddressLine2 = homeAddressLine2;
            this.HomeAddressLine3 = homeAddressLine3;
            this.HomeAddressLine4 = homeAddressLine4;
            this.HomeContactNo = homeContactNo;
            this.LastName = lastName;
            this.LoanTerm = loanTerm;
            this.OutstandingBalance = outstandingBalance;
            this.RequiredAmount = requiredAmount;
            this.Salutation = salutation;
            this.TransferBranch = transferBranch;
            this.TransferRegion = transferRegion;
            this.TransferType = transferType;
            this.WorkContactNo = workContactNo;
		
        }		

        public long InstanceID { get; set; }

        public double? BondAmount { get; set; }

        public string BondType { get; set; }

        public DateTime? CallBackAt { get; set; }

        public string CellNumber { get; set; }

        public string Consultant { get; set; }

        public string DeclineReason { get; set; }

        public double? EstimatedAMInstallment { get; set; }

        public double? EstimatedInstallment { get; set; }

        public bool? ExistingClient { get; set; }

        public string FirstNames { get; set; }

        public string HomeAddressLine1 { get; set; }

        public string HomeAddressLine2 { get; set; }

        public string HomeAddressLine3 { get; set; }

        public string HomeAddressLine4 { get; set; }

        public string HomeContactNo { get; set; }

        public string LastName { get; set; }

        public double? LoanTerm { get; set; }

        public double? OutstandingBalance { get; set; }

        public double? RequiredAmount { get; set; }

        public string Salutation { get; set; }

        public string TransferBranch { get; set; }

        public string TransferRegion { get; set; }

        public string TransferType { get; set; }

        public string WorkContactNo { get; set; }
    }
}