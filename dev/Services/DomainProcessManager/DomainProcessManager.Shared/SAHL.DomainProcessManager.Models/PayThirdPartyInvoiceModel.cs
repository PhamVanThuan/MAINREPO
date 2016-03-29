using System;
using System.Runtime.Serialization;

namespace SAHL.DomainProcessManager.Models
{
    [Serializable]
    [DataContract]
    public class PayThirdPartyInvoiceModel
    {
        private PaymentProcessStep stepInProcess;
        protected readonly object stepInProcessLock = new object();

        [DataMember]
        public int ThirdPartyInvoiceKey { get; set; }

        [DataMember]
        public long InstanceId { get; set; }

        [DataMember]
        public int AccountNumber { get; set; }

        [DataMember]
        public string SAHLReference { get; set; }

        [DataMember]
        public PaymentProcessStep StepInProcess
        {
            get { lock (stepInProcessLock) { return stepInProcess; } }
            set { lock (stepInProcessLock) { stepInProcess = value; } }
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext c)
        {
            var lockField = GetType().GetField("stepInProcessLock",
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.DeclaredOnly |
            System.Reflection.BindingFlags.NonPublic);
            lockField.SetValue(this, new object());
        }

        public PayThirdPartyInvoiceModel(int thirdPartyInvoiceKey, long instanceId, int accountNumber, string sahlReference, PaymentProcessStep? stepInProcess)
        {
            this.ThirdPartyInvoiceKey = thirdPartyInvoiceKey;
            this.InstanceId = instanceId;
            this.AccountNumber = accountNumber;
            this.SAHLReference = sahlReference;
            this.StepInProcess = stepInProcess == null ?
                                    PaymentProcessStep.PreparingWorkflowCase : stepInProcess.Value;
        }
    }

    [Serializable]
    [DataContract]
    public enum PaymentProcessStep
    {
        [EnumMember]
        PreparingWorkflowCase
        ,

        [EnumMember]
        PreparingWorkflowCaseFailed
        ,

        [EnumMember]
        ReadyForBatching
      ,

        [EnumMember]
        BatchingFailed
      ,

        [EnumMember]
        ReadyForPostingTransation
      ,

        [EnumMember]
        PostingTransactionFailed
      ,

        [EnumMember]
        PaymentBatchFailed
      ,

        [EnumMember]
        ReadyForArchiving
      ,

        [EnumMember]
        ArchivingFailed
      ,

        [EnumMember]
        Archived
    }
}