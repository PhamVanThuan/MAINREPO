namespace X2DomainService.Interface.Origination
{
    using System;
    using SAHL.Common.Collections.Interfaces;
    using SAHL.X2.Common;

    public interface IFL : IX2WorkflowService
    {
        bool IsFurtherLoanApplication(IDomainMessageCollection messages, int applicationKey);

        bool IsFurtherAdvanceApplication(IDomainMessageCollection messages, int applicationKey);

        bool IsReadvanceAdvanceApplication(IDomainMessageCollection messages, int applicationKey);

        void RemoveDetailTypes(IDomainMessageCollection messages, int applicationKey);

        void ArchiveFLRelatedCases(IDomainMessageCollection messages, Int64 instanceID, int applicationKey, string ADUser);

        void FLCompleteUnholdNextApplicationWhereApplicable(IDomainMessageCollection messages, int applicationKey);

        void AddDetailTypes(IDomainMessageCollection messages, int applicationKey, string ADUser);

        bool AppsInProgOfHigherPri(IDomainMessageCollection messages, int applicationKey);

        bool HighestPriority(IDomainMessageCollection messages, int applicationKey);

        bool ValuationRequired(IDomainMessageCollection messages, int applicationKey);

        //double LTVForFLCase(IDomainMessageCollection messages, int applicationKey);

        void InitialFLNTU(IDomainMessageCollection messages, string ADUser, int applicationKey, Int64 instanceID);

        void AddAccountMemoMessageOnReceiptOfApplication(IDomainMessageCollection messages, int applicationKey, string ADUser, string assignedTo);

        void SuretySignedConfirmed(IDomainMessageCollection messages, int applicationKey);

        bool CanRollbackReadvanceCorrectionTransaction(IDomainMessageCollection messages, int applicationKey);

        bool CheckCanDisburseReadvanceRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CheckInformClientRule(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CheckRapidShouldGotoCreditRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CheckSuretyForReAdvanceRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CheckNTUDeclineFinalRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        //bool CheckOptOutSuperLoRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CheckSuperLoOptOutRequiredRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CheckSuperLoOptOutRequiredWithNoMessagesRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        //bool CheckSuperLoOptOutRules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool CheckIsFurtherAdvanceBelowLAARules(IDomainMessageCollection messages, int applicationKey, bool ignoreWarnings);

        bool OptOutSuperLo(IDomainMessageCollection messages, int applicationKey, string aDUser);
    }
}