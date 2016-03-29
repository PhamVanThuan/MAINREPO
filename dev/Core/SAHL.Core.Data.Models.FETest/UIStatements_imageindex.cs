using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.FETest
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string datadatamodel_selectwhere = "SELECT ID, securityGroup, archiveDate, dataContainer, backupVolume, Overlay, STOR, GUID, Extension, Key1, Key2, Key3, Key4, Key5, Key6, Key7, Key8, msgTo, msgFrom, msgSubject, msgReceived, msgSent, Key9, Key10, Key11, Key12, Key13, Key14, Key15, Key16, Title, OriginalFilename FROM [FETest].[imageindex].[Data] WHERE";
        public const string datadatamodel_selectbykey = "SELECT ID, securityGroup, archiveDate, dataContainer, backupVolume, Overlay, STOR, GUID, Extension, Key1, Key2, Key3, Key4, Key5, Key6, Key7, Key8, msgTo, msgFrom, msgSubject, msgReceived, msgSent, Key9, Key10, Key11, Key12, Key13, Key14, Key15, Key16, Title, OriginalFilename FROM [FETest].[imageindex].[Data] WHERE ID = @PrimaryKey";
        public const string datadatamodel_delete = "DELETE FROM [FETest].[imageindex].[Data] WHERE ID = @PrimaryKey";
        public const string datadatamodel_deletewhere = "DELETE FROM [FETest].[imageindex].[Data] WHERE";
        public const string datadatamodel_insert = "INSERT INTO [FETest].[imageindex].[Data] (securityGroup, archiveDate, dataContainer, backupVolume, Overlay, STOR, GUID, Extension, Key1, Key2, Key3, Key4, Key5, Key6, Key7, Key8, msgTo, msgFrom, msgSubject, msgReceived, msgSent, Key9, Key10, Key11, Key12, Key13, Key14, Key15, Key16, Title, OriginalFilename) VALUES(@securityGroup, @archiveDate, @dataContainer, @backupVolume, @Overlay, @STOR, @GUID, @Extension, @Key1, @Key2, @Key3, @Key4, @Key5, @Key6, @Key7, @Key8, @msgTo, @msgFrom, @msgSubject, @msgReceived, @msgSent, @Key9, @Key10, @Key11, @Key12, @Key13, @Key14, @Key15, @Key16, @Title, @OriginalFilename); select cast(scope_identity() as int)";
        public const string datadatamodel_update = "UPDATE [FETest].[imageindex].[Data] SET securityGroup = @securityGroup, archiveDate = @archiveDate, dataContainer = @dataContainer, backupVolume = @backupVolume, Overlay = @Overlay, STOR = @STOR, GUID = @GUID, Extension = @Extension, Key1 = @Key1, Key2 = @Key2, Key3 = @Key3, Key4 = @Key4, Key5 = @Key5, Key6 = @Key6, Key7 = @Key7, Key8 = @Key8, msgTo = @msgTo, msgFrom = @msgFrom, msgSubject = @msgSubject, msgReceived = @msgReceived, msgSent = @msgSent, Key9 = @Key9, Key10 = @Key10, Key11 = @Key11, Key12 = @Key12, Key13 = @Key13, Key14 = @Key14, Key15 = @Key15, Key16 = @Key16, Title = @Title, OriginalFilename = @OriginalFilename WHERE ID = @ID";



    }
}