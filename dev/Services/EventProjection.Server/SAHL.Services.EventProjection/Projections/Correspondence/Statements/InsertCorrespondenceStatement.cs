using SAHL.Core.Attributes;
using SAHL.Core.Data;
using System;

namespace SAHL.Services.EventProjection.Projections.Correspondence.Statements
{
    [InsertConventionExclude]
    public class InsertCorrespondenceStatement : ISqlStatement<bool>
    {
        public int GenericKey { get; protected set; }

        public int GenericKeyTypeKey { get; protected set; }

        public string CorrespondenceReason { get; protected set; }

        public string CorrespondenceType { get; protected set; }

        public string CorrespondenceMedium { get; protected set; }

        public string UserName { get; protected set; }

        public DateTime Date { get; protected set; }

        public string MemoText { get; protected set; }

        public InsertCorrespondenceStatement(int genericKey, int genericKeyTypeKey, string correspondenceReason, string correspondenceType, string correspondenceMedium,
            string userName, DateTime date, string memoText)
        {
            this.GenericKey = genericKey;
            this.GenericKeyTypeKey = genericKeyTypeKey;
            this.CorrespondenceReason = correspondenceReason;
            this.CorrespondenceMedium = correspondenceMedium;
            this.CorrespondenceType = correspondenceType;
            this.UserName = userName;
            this.Date = date;
            this.MemoText = memoText;
        }

        public string GetStatement()
        {
            return @"INSERT INTO [EventProjection].[projection].[Correspondence]
                    ([CorrespondenceType]
                    ,[CorrespondenceReason]
                    ,[CorrespondenceMedium]
                    ,[Date]
                    ,[UserName]
                    ,[MemoText]
                    ,[GenericKey]
                    ,[GenericKeyTypeKey])
                 VALUES
                   (@CorrespondenceType
                   ,@CorrespondenceReason
                   ,@CorrespondenceMedium
                   ,@Date
                   ,@UserName
                   ,@MemoText
                   ,@GenericKey
                   ,@GenericKeyTypeKey)";
        }
    }
}