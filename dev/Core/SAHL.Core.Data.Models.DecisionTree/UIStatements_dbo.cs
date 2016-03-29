using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.DecisionTree
{
	public partial class UIStatements : IUIStatementsProvider
    {
		
		public const string usermrudecisiontreedatamodel_selectwhere = "SELECT Id, UserName, TreeId, ModifiedDate, Pinned FROM [DecisionTree].[dbo].[UserMRUDecisionTree] WHERE";
		public const string usermrudecisiontreedatamodel_selectbykey = "SELECT Id, UserName, TreeId, ModifiedDate, Pinned FROM [DecisionTree].[dbo].[UserMRUDecisionTree] WHERE Id = @PrimaryKey";
		public const string usermrudecisiontreedatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[UserMRUDecisionTree] WHERE Id = @PrimaryKey";
		public const string usermrudecisiontreedatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[UserMRUDecisionTree] WHERE";
		public const string usermrudecisiontreedatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[UserMRUDecisionTree] (Id, UserName, TreeId, ModifiedDate, Pinned) VALUES(@Id, @UserName, @TreeId, @ModifiedDate, @Pinned); ";
		public const string usermrudecisiontreedatamodel_update = "UPDATE [DecisionTree].[dbo].[UserMRUDecisionTree] SET Id = @Id, UserName = @UserName, TreeId = @TreeId, ModifiedDate = @ModifiedDate, Pinned = @Pinned WHERE Id = @Id";



		public const string decisiontreedatamodel_selectwhere = "SELECT Id, Name, Description, IsActive, Thumbnail FROM [DecisionTree].[dbo].[DecisionTree] WHERE";
		public const string decisiontreedatamodel_selectbykey = "SELECT Id, Name, Description, IsActive, Thumbnail FROM [DecisionTree].[dbo].[DecisionTree] WHERE Id = @PrimaryKey";
		public const string decisiontreedatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[DecisionTree] WHERE Id = @PrimaryKey";
		public const string decisiontreedatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[DecisionTree] WHERE";
		public const string decisiontreedatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[DecisionTree] (Id, Name, Description, IsActive, Thumbnail) VALUES(@Id, @Name, @Description, @IsActive, @Thumbnail); ";
		public const string decisiontreedatamodel_update = "UPDATE [DecisionTree].[dbo].[DecisionTree] SET Id = @Id, Name = @Name, Description = @Description, IsActive = @IsActive, Thumbnail = @Thumbnail WHERE Id = @Id";



		public const string decisiontreeversiondatamodel_selectwhere = "SELECT Id, DecisionTreeId, Version, Data FROM [DecisionTree].[dbo].[DecisionTreeVersion] WHERE";
		public const string decisiontreeversiondatamodel_selectbykey = "SELECT Id, DecisionTreeId, Version, Data FROM [DecisionTree].[dbo].[DecisionTreeVersion] WHERE Id = @PrimaryKey";
		public const string decisiontreeversiondatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[DecisionTreeVersion] WHERE Id = @PrimaryKey";
		public const string decisiontreeversiondatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[DecisionTreeVersion] WHERE";
		public const string decisiontreeversiondatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[DecisionTreeVersion] (Id, DecisionTreeId, Version, Data) VALUES(@Id, @DecisionTreeId, @Version, @Data); ";
		public const string decisiontreeversiondatamodel_update = "UPDATE [DecisionTree].[dbo].[DecisionTreeVersion] SET Id = @Id, DecisionTreeId = @DecisionTreeId, Version = @Version, Data = @Data WHERE Id = @Id";



		public const string decisiontreehistorydatamodel_selectwhere = "SELECT Id, DecisionTreeVersionId, ModificationUser, ModificationDate FROM [DecisionTree].[dbo].[DecisionTreeHistory] WHERE";
		public const string decisiontreehistorydatamodel_selectbykey = "SELECT Id, DecisionTreeVersionId, ModificationUser, ModificationDate FROM [DecisionTree].[dbo].[DecisionTreeHistory] WHERE Id = @PrimaryKey";
		public const string decisiontreehistorydatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[DecisionTreeHistory] WHERE Id = @PrimaryKey";
		public const string decisiontreehistorydatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[DecisionTreeHistory] WHERE";
		public const string decisiontreehistorydatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[DecisionTreeHistory] (Id, DecisionTreeVersionId, ModificationUser, ModificationDate) VALUES(@Id, @DecisionTreeVersionId, @ModificationUser, @ModificationDate); ";
		public const string decisiontreehistorydatamodel_update = "UPDATE [DecisionTree].[dbo].[DecisionTreeHistory] SET Id = @Id, DecisionTreeVersionId = @DecisionTreeVersionId, ModificationUser = @ModificationUser, ModificationDate = @ModificationDate WHERE Id = @Id";



		public const string currentlyopendecisiontreedatamodel_selectwhere = "SELECT Id, DecisionTreeVersionId, Username, OpenDate FROM [DecisionTree].[dbo].[CurrentlyOpenDecisionTree] WHERE";
		public const string currentlyopendecisiontreedatamodel_selectbykey = "SELECT Id, DecisionTreeVersionId, Username, OpenDate FROM [DecisionTree].[dbo].[CurrentlyOpenDecisionTree] WHERE Id = @PrimaryKey";
		public const string currentlyopendecisiontreedatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[CurrentlyOpenDecisionTree] WHERE Id = @PrimaryKey";
		public const string currentlyopendecisiontreedatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[CurrentlyOpenDecisionTree] WHERE";
		public const string currentlyopendecisiontreedatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[CurrentlyOpenDecisionTree] (Id, DecisionTreeVersionId, Username, OpenDate) VALUES(@Id, @DecisionTreeVersionId, @Username, @OpenDate); ";
		public const string currentlyopendecisiontreedatamodel_update = "UPDATE [DecisionTree].[dbo].[CurrentlyOpenDecisionTree] SET Id = @Id, DecisionTreeVersionId = @DecisionTreeVersionId, Username = @Username, OpenDate = @OpenDate WHERE Id = @Id";



		public const string documenttypeenumdatamodel_selectwhere = "SELECT Id, Name FROM [DecisionTree].[dbo].[DocumentTypeEnum] WHERE";
		public const string documenttypeenumdatamodel_selectbykey = "SELECT Id, Name FROM [DecisionTree].[dbo].[DocumentTypeEnum] WHERE Id = @PrimaryKey";
		public const string documenttypeenumdatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[DocumentTypeEnum] WHERE Id = @PrimaryKey";
		public const string documenttypeenumdatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[DocumentTypeEnum] WHERE";
		public const string documenttypeenumdatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[DocumentTypeEnum] (Id, Name) VALUES(@Id, @Name); ";
		public const string documenttypeenumdatamodel_update = "UPDATE [DecisionTree].[dbo].[DocumentTypeEnum] SET Id = @Id, Name = @Name WHERE Id = @Id";



		public const string currentlyopendocumentdatamodel_selectwhere = "SELECT Id, DocumentVersionId, Username, OpenDate, DocumentTypeId FROM [DecisionTree].[dbo].[CurrentlyOpenDocument] WHERE";
		public const string currentlyopendocumentdatamodel_selectbykey = "SELECT Id, DocumentVersionId, Username, OpenDate, DocumentTypeId FROM [DecisionTree].[dbo].[CurrentlyOpenDocument] WHERE Id = @PrimaryKey";
		public const string currentlyopendocumentdatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[CurrentlyOpenDocument] WHERE Id = @PrimaryKey";
		public const string currentlyopendocumentdatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[CurrentlyOpenDocument] WHERE";
		public const string currentlyopendocumentdatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[CurrentlyOpenDocument] (Id, DocumentVersionId, Username, OpenDate, DocumentTypeId) VALUES(@Id, @DocumentVersionId, @Username, @OpenDate, @DocumentTypeId); ";
		public const string currentlyopendocumentdatamodel_update = "UPDATE [DecisionTree].[dbo].[CurrentlyOpenDocument] SET Id = @Id, DocumentVersionId = @DocumentVersionId, Username = @Username, OpenDate = @OpenDate, DocumentTypeId = @DocumentTypeId WHERE Id = @Id";



		public const string enumerationsetdatamodel_selectwhere = "SELECT Id, Version, Data FROM [DecisionTree].[dbo].[EnumerationSet] WHERE";
		public const string enumerationsetdatamodel_selectbykey = "SELECT Id, Version, Data FROM [DecisionTree].[dbo].[EnumerationSet] WHERE Id = @PrimaryKey";
		public const string enumerationsetdatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[EnumerationSet] WHERE Id = @PrimaryKey";
		public const string enumerationsetdatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[EnumerationSet] WHERE";
		public const string enumerationsetdatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[EnumerationSet] (Id, Version, Data) VALUES(@Id, @Version, @Data); ";
		public const string enumerationsetdatamodel_update = "UPDATE [DecisionTree].[dbo].[EnumerationSet] SET Id = @Id, Version = @Version, Data = @Data WHERE Id = @Id";



		public const string messagesetdatamodel_selectwhere = "SELECT Id, Version, Data FROM [DecisionTree].[dbo].[MessageSet] WHERE";
		public const string messagesetdatamodel_selectbykey = "SELECT Id, Version, Data FROM [DecisionTree].[dbo].[MessageSet] WHERE Id = @PrimaryKey";
		public const string messagesetdatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[MessageSet] WHERE Id = @PrimaryKey";
		public const string messagesetdatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[MessageSet] WHERE";
		public const string messagesetdatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[MessageSet] (Id, Version, Data) VALUES(@Id, @Version, @Data); ";
		public const string messagesetdatamodel_update = "UPDATE [DecisionTree].[dbo].[MessageSet] SET Id = @Id, Version = @Version, Data = @Data WHERE Id = @Id";



		public const string variablesetdatamodel_selectwhere = "SELECT Id, Version, Data FROM [DecisionTree].[dbo].[VariableSet] WHERE";
		public const string variablesetdatamodel_selectbykey = "SELECT Id, Version, Data FROM [DecisionTree].[dbo].[VariableSet] WHERE Id = @PrimaryKey";
		public const string variablesetdatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[VariableSet] WHERE Id = @PrimaryKey";
		public const string variablesetdatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[VariableSet] WHERE";
		public const string variablesetdatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[VariableSet] (Id, Version, Data) VALUES(@Id, @Version, @Data); ";
		public const string variablesetdatamodel_update = "UPDATE [DecisionTree].[dbo].[VariableSet] SET Id = @Id, Version = @Version, Data = @Data WHERE Id = @Id";



		public const string publishstatusenumdatamodel_selectwhere = "SELECT Id, Name, IsActive FROM [DecisionTree].[dbo].[PublishStatusEnum] WHERE";
		public const string publishstatusenumdatamodel_selectbykey = "SELECT Id, Name, IsActive FROM [DecisionTree].[dbo].[PublishStatusEnum] WHERE Id = @PrimaryKey";
		public const string publishstatusenumdatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[PublishStatusEnum] WHERE Id = @PrimaryKey";
		public const string publishstatusenumdatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[PublishStatusEnum] WHERE";
		public const string publishstatusenumdatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[PublishStatusEnum] (Id, Name, IsActive) VALUES(@Id, @Name, @IsActive); ";
		public const string publishstatusenumdatamodel_update = "UPDATE [DecisionTree].[dbo].[PublishStatusEnum] SET Id = @Id, Name = @Name, IsActive = @IsActive WHERE Id = @Id";



		public const string publisheddecisiontreedatamodel_selectwhere = "SELECT Id, DecisionTreeVersionId, PublishStatusId, PublishDate, Publisher FROM [DecisionTree].[dbo].[PublishedDecisionTree] WHERE";
		public const string publisheddecisiontreedatamodel_selectbykey = "SELECT Id, DecisionTreeVersionId, PublishStatusId, PublishDate, Publisher FROM [DecisionTree].[dbo].[PublishedDecisionTree] WHERE Id = @PrimaryKey";
		public const string publisheddecisiontreedatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[PublishedDecisionTree] WHERE Id = @PrimaryKey";
		public const string publisheddecisiontreedatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[PublishedDecisionTree] WHERE";
		public const string publisheddecisiontreedatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[PublishedDecisionTree] (Id, DecisionTreeVersionId, PublishStatusId, PublishDate, Publisher) VALUES(@Id, @DecisionTreeVersionId, @PublishStatusId, @PublishDate, @Publisher); ";
		public const string publisheddecisiontreedatamodel_update = "UPDATE [DecisionTree].[dbo].[PublishedDecisionTree] SET Id = @Id, DecisionTreeVersionId = @DecisionTreeVersionId, PublishStatusId = @PublishStatusId, PublishDate = @PublishDate, Publisher = @Publisher WHERE Id = @Id";



		public const string publishedenumerationsetdatamodel_selectwhere = "SELECT Id, EnumerationSetId, PublishStatusId, PublishDate, Publisher FROM [DecisionTree].[dbo].[PublishedEnumerationSet] WHERE";
		public const string publishedenumerationsetdatamodel_selectbykey = "SELECT Id, EnumerationSetId, PublishStatusId, PublishDate, Publisher FROM [DecisionTree].[dbo].[PublishedEnumerationSet] WHERE Id = @PrimaryKey";
		public const string publishedenumerationsetdatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[PublishedEnumerationSet] WHERE Id = @PrimaryKey";
		public const string publishedenumerationsetdatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[PublishedEnumerationSet] WHERE";
		public const string publishedenumerationsetdatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[PublishedEnumerationSet] (Id, EnumerationSetId, PublishStatusId, PublishDate, Publisher) VALUES(@Id, @EnumerationSetId, @PublishStatusId, @PublishDate, @Publisher); ";
		public const string publishedenumerationsetdatamodel_update = "UPDATE [DecisionTree].[dbo].[PublishedEnumerationSet] SET Id = @Id, EnumerationSetId = @EnumerationSetId, PublishStatusId = @PublishStatusId, PublishDate = @PublishDate, Publisher = @Publisher WHERE Id = @Id";



		public const string publishedmessagesetdatamodel_selectwhere = "SELECT Id, MessageSetId, PublishStatusId, PublishDate, Publisher FROM [DecisionTree].[dbo].[PublishedMessageSet] WHERE";
		public const string publishedmessagesetdatamodel_selectbykey = "SELECT Id, MessageSetId, PublishStatusId, PublishDate, Publisher FROM [DecisionTree].[dbo].[PublishedMessageSet] WHERE Id = @PrimaryKey";
		public const string publishedmessagesetdatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[PublishedMessageSet] WHERE Id = @PrimaryKey";
		public const string publishedmessagesetdatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[PublishedMessageSet] WHERE";
		public const string publishedmessagesetdatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[PublishedMessageSet] (Id, MessageSetId, PublishStatusId, PublishDate, Publisher) VALUES(@Id, @MessageSetId, @PublishStatusId, @PublishDate, @Publisher); ";
		public const string publishedmessagesetdatamodel_update = "UPDATE [DecisionTree].[dbo].[PublishedMessageSet] SET Id = @Id, MessageSetId = @MessageSetId, PublishStatusId = @PublishStatusId, PublishDate = @PublishDate, Publisher = @Publisher WHERE Id = @Id";



		public const string publishedvariablesetdatamodel_selectwhere = "SELECT Id, VariableSetId, PublishStatusId, PublishDate, Publisher FROM [DecisionTree].[dbo].[PublishedVariableSet] WHERE";
		public const string publishedvariablesetdatamodel_selectbykey = "SELECT Id, VariableSetId, PublishStatusId, PublishDate, Publisher FROM [DecisionTree].[dbo].[PublishedVariableSet] WHERE Id = @PrimaryKey";
		public const string publishedvariablesetdatamodel_delete = "DELETE FROM [DecisionTree].[dbo].[PublishedVariableSet] WHERE Id = @PrimaryKey";
		public const string publishedvariablesetdatamodel_deletewhere = "DELETE FROM [DecisionTree].[dbo].[PublishedVariableSet] WHERE";
		public const string publishedvariablesetdatamodel_insert = "INSERT INTO [DecisionTree].[dbo].[PublishedVariableSet] (Id, VariableSetId, PublishStatusId, PublishDate, Publisher) VALUES(@Id, @VariableSetId, @PublishStatusId, @PublishDate, @Publisher); ";
		public const string publishedvariablesetdatamodel_update = "UPDATE [DecisionTree].[dbo].[PublishedVariableSet] SET Id = @Id, VariableSetId = @VariableSetId, PublishStatusId = @PublishStatusId, PublishDate = @PublishDate, Publisher = @Publisher WHERE Id = @Id";



	}
}