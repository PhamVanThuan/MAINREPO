using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	public partial class UIStatements : IUIStatementsProvider
    {
		
		public const string roledatamodel_selectwhere = "SELECT Id, Name FROM [Capitec].[security].[Role] WHERE";
		public const string roledatamodel_selectbykey = "SELECT Id, Name FROM [Capitec].[security].[Role] WHERE Id = @PrimaryKey";
		public const string roledatamodel_delete = "DELETE FROM [Capitec].[security].[Role] WHERE Id = @PrimaryKey";
		public const string roledatamodel_deletewhere = "DELETE FROM [Capitec].[security].[Role] WHERE";
		public const string roledatamodel_insert = "INSERT INTO [Capitec].[security].[Role] (Id, Name) VALUES(@Id, @Name); ";
		public const string roledatamodel_update = "UPDATE [Capitec].[security].[Role] SET Id = @Id, Name = @Name WHERE Id = @Id";



		public const string branchdatamodel_selectwhere = "SELECT Id, BranchName, SuburbId, IsActive, BranchCode FROM [Capitec].[security].[Branch] WHERE";
		public const string branchdatamodel_selectbykey = "SELECT Id, BranchName, SuburbId, IsActive, BranchCode FROM [Capitec].[security].[Branch] WHERE Id = @PrimaryKey";
		public const string branchdatamodel_delete = "DELETE FROM [Capitec].[security].[Branch] WHERE Id = @PrimaryKey";
		public const string branchdatamodel_deletewhere = "DELETE FROM [Capitec].[security].[Branch] WHERE";
		public const string branchdatamodel_insert = "INSERT INTO [Capitec].[security].[Branch] (Id, BranchName, SuburbId, IsActive, BranchCode) VALUES(@Id, @BranchName, @SuburbId, @IsActive, @BranchCode); ";
		public const string branchdatamodel_update = "UPDATE [Capitec].[security].[Branch] SET Id = @Id, BranchName = @BranchName, SuburbId = @SuburbId, IsActive = @IsActive, BranchCode = @BranchCode WHERE Id = @Id";



		public const string userdatamodel_selectwhere = "SELECT Id, UserName, PasswordHash, SecurityStamp, IsActive, IsLockedOut FROM [Capitec].[security].[User] WHERE";
		public const string userdatamodel_selectbykey = "SELECT Id, UserName, PasswordHash, SecurityStamp, IsActive, IsLockedOut FROM [Capitec].[security].[User] WHERE Id = @PrimaryKey";
		public const string userdatamodel_delete = "DELETE FROM [Capitec].[security].[User] WHERE Id = @PrimaryKey";
		public const string userdatamodel_deletewhere = "DELETE FROM [Capitec].[security].[User] WHERE";
		public const string userdatamodel_insert = "INSERT INTO [Capitec].[security].[User] (Id, UserName, PasswordHash, SecurityStamp, IsActive, IsLockedOut) VALUES(@Id, @UserName, @PasswordHash, @SecurityStamp, @IsActive, @IsLockedOut); ";
		public const string userdatamodel_update = "UPDATE [Capitec].[security].[User] SET Id = @Id, UserName = @UserName, PasswordHash = @PasswordHash, SecurityStamp = @SecurityStamp, IsActive = @IsActive, IsLockedOut = @IsLockedOut WHERE Id = @Id";



		public const string userinformationdatamodel_selectwhere = "SELECT Id, UserId, EmailAddress, FirstName, LastName FROM [Capitec].[security].[UserInformation] WHERE";
		public const string userinformationdatamodel_selectbykey = "SELECT Id, UserId, EmailAddress, FirstName, LastName FROM [Capitec].[security].[UserInformation] WHERE Id = @PrimaryKey";
		public const string userinformationdatamodel_delete = "DELETE FROM [Capitec].[security].[UserInformation] WHERE Id = @PrimaryKey";
		public const string userinformationdatamodel_deletewhere = "DELETE FROM [Capitec].[security].[UserInformation] WHERE";
		public const string userinformationdatamodel_insert = "INSERT INTO [Capitec].[security].[UserInformation] (Id, UserId, EmailAddress, FirstName, LastName) VALUES(@Id, @UserId, @EmailAddress, @FirstName, @LastName); ";
		public const string userinformationdatamodel_update = "UPDATE [Capitec].[security].[UserInformation] SET Id = @Id, UserId = @UserId, EmailAddress = @EmailAddress, FirstName = @FirstName, LastName = @LastName WHERE Id = @Id";



		public const string userbranchdatamodel_selectwhere = "SELECT Id, UserId, BranchId FROM [Capitec].[security].[UserBranch] WHERE";
		public const string userbranchdatamodel_selectbykey = "SELECT Id, UserId, BranchId FROM [Capitec].[security].[UserBranch] WHERE Id = @PrimaryKey";
		public const string userbranchdatamodel_delete = "DELETE FROM [Capitec].[security].[UserBranch] WHERE Id = @PrimaryKey";
		public const string userbranchdatamodel_deletewhere = "DELETE FROM [Capitec].[security].[UserBranch] WHERE";
		public const string userbranchdatamodel_insert = "INSERT INTO [Capitec].[security].[UserBranch] (Id, UserId, BranchId) VALUES(@Id, @UserId, @BranchId); ";
		public const string userbranchdatamodel_update = "UPDATE [Capitec].[security].[UserBranch] SET Id = @Id, UserId = @UserId, BranchId = @BranchId WHERE Id = @Id";



		public const string userroledatamodel_selectwhere = "SELECT Id, UserId, RoleId FROM [Capitec].[security].[UserRole] WHERE";
		public const string userroledatamodel_selectbykey = "SELECT Id, UserId, RoleId FROM [Capitec].[security].[UserRole] WHERE Id = @PrimaryKey";
		public const string userroledatamodel_delete = "DELETE FROM [Capitec].[security].[UserRole] WHERE Id = @PrimaryKey";
		public const string userroledatamodel_deletewhere = "DELETE FROM [Capitec].[security].[UserRole] WHERE";
		public const string userroledatamodel_insert = "INSERT INTO [Capitec].[security].[UserRole] (Id, UserId, RoleId) VALUES(@Id, @UserId, @RoleId); ";
		public const string userroledatamodel_update = "UPDATE [Capitec].[security].[UserRole] SET Id = @Id, UserId = @UserId, RoleId = @RoleId WHERE Id = @Id";



		public const string activitydatamodel_selectwhere = "SELECT Id, UserId, LastLogin, LastActivity FROM [Capitec].[security].[Activity] WHERE";
		public const string activitydatamodel_selectbykey = "SELECT Id, UserId, LastLogin, LastActivity FROM [Capitec].[security].[Activity] WHERE Id = @PrimaryKey";
		public const string activitydatamodel_delete = "DELETE FROM [Capitec].[security].[Activity] WHERE Id = @PrimaryKey";
		public const string activitydatamodel_deletewhere = "DELETE FROM [Capitec].[security].[Activity] WHERE";
		public const string activitydatamodel_insert = "INSERT INTO [Capitec].[security].[Activity] (Id, UserId, LastLogin, LastActivity) VALUES(@Id, @UserId, @LastLogin, @LastActivity); ";
		public const string activitydatamodel_update = "UPDATE [Capitec].[security].[Activity] SET Id = @Id, UserId = @UserId, LastLogin = @LastLogin, LastActivity = @LastActivity WHERE Id = @Id";



		public const string tokendatamodel_selectwhere = "SELECT Id, UserId, SecurityToken, IpAddress, IssueDate FROM [Capitec].[security].[Token] WHERE";
		public const string tokendatamodel_selectbykey = "SELECT Id, UserId, SecurityToken, IpAddress, IssueDate FROM [Capitec].[security].[Token] WHERE Id = @PrimaryKey";
		public const string tokendatamodel_delete = "DELETE FROM [Capitec].[security].[Token] WHERE Id = @PrimaryKey";
		public const string tokendatamodel_deletewhere = "DELETE FROM [Capitec].[security].[Token] WHERE";
		public const string tokendatamodel_insert = "INSERT INTO [Capitec].[security].[Token] (Id, UserId, SecurityToken, IpAddress, IssueDate) VALUES(@Id, @UserId, @SecurityToken, @IpAddress, @IssueDate); ";
		public const string tokendatamodel_update = "UPDATE [Capitec].[security].[Token] SET Id = @Id, UserId = @UserId, SecurityToken = @SecurityToken, IpAddress = @IpAddress, IssueDate = @IssueDate WHERE Id = @Id";



		public const string invitationdatamodel_selectwhere = "SELECT Id, UserId, InvitationToken, InvitationDate, AcceptedDate FROM [Capitec].[security].[Invitation] WHERE";
		public const string invitationdatamodel_selectbykey = "SELECT Id, UserId, InvitationToken, InvitationDate, AcceptedDate FROM [Capitec].[security].[Invitation] WHERE Id = @PrimaryKey";
		public const string invitationdatamodel_delete = "DELETE FROM [Capitec].[security].[Invitation] WHERE Id = @PrimaryKey";
		public const string invitationdatamodel_deletewhere = "DELETE FROM [Capitec].[security].[Invitation] WHERE";
		public const string invitationdatamodel_insert = "INSERT INTO [Capitec].[security].[Invitation] (Id, UserId, InvitationToken, InvitationDate, AcceptedDate) VALUES(@Id, @UserId, @InvitationToken, @InvitationDate, @AcceptedDate); ";
		public const string invitationdatamodel_update = "UPDATE [Capitec].[security].[Invitation] SET Id = @Id, UserId = @UserId, InvitationToken = @InvitationToken, InvitationDate = @InvitationDate, AcceptedDate = @AcceptedDate WHERE Id = @Id";



	}
}