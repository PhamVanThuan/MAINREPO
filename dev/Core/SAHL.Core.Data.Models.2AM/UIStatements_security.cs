using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models._2AM
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string authenticationtypedatamodel_selectwhere = "SELECT AuthenticationTypeKey, Description FROM [2AM].[security].[AuthenticationType] WHERE";
        public const string authenticationtypedatamodel_selectbykey = "SELECT AuthenticationTypeKey, Description FROM [2AM].[security].[AuthenticationType] WHERE AuthenticationTypeKey = @PrimaryKey";
        public const string authenticationtypedatamodel_delete = "DELETE FROM [2AM].[security].[AuthenticationType] WHERE AuthenticationTypeKey = @PrimaryKey";
        public const string authenticationtypedatamodel_deletewhere = "DELETE FROM [2AM].[security].[AuthenticationType] WHERE";
        public const string authenticationtypedatamodel_insert = "INSERT INTO [2AM].[security].[AuthenticationType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string authenticationtypedatamodel_update = "UPDATE [2AM].[security].[AuthenticationType] SET Description = @Description WHERE AuthenticationTypeKey = @AuthenticationTypeKey";



        public const string userdatamodel_selectwhere = "SELECT UserKey, AuthenticationTypeKey, UserName, IsActive, IsLockedOut FROM [2AM].[security].[User] WHERE";
        public const string userdatamodel_selectbykey = "SELECT UserKey, AuthenticationTypeKey, UserName, IsActive, IsLockedOut FROM [2AM].[security].[User] WHERE UserKey = @PrimaryKey";
        public const string userdatamodel_delete = "DELETE FROM [2AM].[security].[User] WHERE UserKey = @PrimaryKey";
        public const string userdatamodel_deletewhere = "DELETE FROM [2AM].[security].[User] WHERE";
        public const string userdatamodel_insert = "INSERT INTO [2AM].[security].[User] (AuthenticationTypeKey, UserName, IsActive, IsLockedOut) VALUES(@AuthenticationTypeKey, @UserName, @IsActive, @IsLockedOut); select cast(scope_identity() as int)";
        public const string userdatamodel_update = "UPDATE [2AM].[security].[User] SET AuthenticationTypeKey = @AuthenticationTypeKey, UserName = @UserName, IsActive = @IsActive, IsLockedOut = @IsLockedOut WHERE UserKey = @UserKey";



        public const string authtokendatamodel_selectwhere = "SELECT AuthTokenKey, UserKey, SecurityToken, IpAddress, IssueDate FROM [2AM].[security].[AuthToken] WHERE";
        public const string authtokendatamodel_selectbykey = "SELECT AuthTokenKey, UserKey, SecurityToken, IpAddress, IssueDate FROM [2AM].[security].[AuthToken] WHERE AuthTokenKey = @PrimaryKey";
        public const string authtokendatamodel_delete = "DELETE FROM [2AM].[security].[AuthToken] WHERE AuthTokenKey = @PrimaryKey";
        public const string authtokendatamodel_deletewhere = "DELETE FROM [2AM].[security].[AuthToken] WHERE";
        public const string authtokendatamodel_insert = "INSERT INTO [2AM].[security].[AuthToken] (UserKey, SecurityToken, IpAddress, IssueDate) VALUES(@UserKey, @SecurityToken, @IpAddress, @IssueDate); select cast(scope_identity() as int)";
        public const string authtokendatamodel_update = "UPDATE [2AM].[security].[AuthToken] SET UserKey = @UserKey, SecurityToken = @SecurityToken, IpAddress = @IpAddress, IssueDate = @IssueDate WHERE AuthTokenKey = @AuthTokenKey";



        public const string activitydatamodel_selectwhere = "SELECT ActivityKey, UserKey, LastLogin, LastActivity FROM [2AM].[security].[Activity] WHERE";
        public const string activitydatamodel_selectbykey = "SELECT ActivityKey, UserKey, LastLogin, LastActivity FROM [2AM].[security].[Activity] WHERE ActivityKey = @PrimaryKey";
        public const string activitydatamodel_delete = "DELETE FROM [2AM].[security].[Activity] WHERE ActivityKey = @PrimaryKey";
        public const string activitydatamodel_deletewhere = "DELETE FROM [2AM].[security].[Activity] WHERE";
        public const string activitydatamodel_insert = "INSERT INTO [2AM].[security].[Activity] (UserKey, LastLogin, LastActivity) VALUES(@UserKey, @LastLogin, @LastActivity); select cast(scope_identity() as int)";
        public const string activitydatamodel_update = "UPDATE [2AM].[security].[Activity] SET UserKey = @UserKey, LastLogin = @LastLogin, LastActivity = @LastActivity WHERE ActivityKey = @ActivityKey";



    }
}