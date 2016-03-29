using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models._2AM
{
    public partial class UIStatements : IUIStatementsProvider
    {
        
        public const string capabilitydatamodel_selectwhere = "SELECT CapabilityKey, Description FROM [2AM].[OrgStruct].[Capability] WHERE";
        public const string capabilitydatamodel_selectbykey = "SELECT CapabilityKey, Description FROM [2AM].[OrgStruct].[Capability] WHERE CapabilityKey = @PrimaryKey";
        public const string capabilitydatamodel_delete = "DELETE FROM [2AM].[OrgStruct].[Capability] WHERE CapabilityKey = @PrimaryKey";
        public const string capabilitydatamodel_deletewhere = "DELETE FROM [2AM].[OrgStruct].[Capability] WHERE";
        public const string capabilitydatamodel_insert = "INSERT INTO [2AM].[OrgStruct].[Capability] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string capabilitydatamodel_update = "UPDATE [2AM].[OrgStruct].[Capability] SET Description = @Description WHERE CapabilityKey = @CapabilityKey";



        public const string userorganisationstructurecapabilitydatamodel_selectwhere = "SELECT UserOrganisationStructureCapabilityKey, UserOrganisationStructureKey, CapabilityKey FROM [2AM].[OrgStruct].[UserOrganisationStructureCapability] WHERE";
        public const string userorganisationstructurecapabilitydatamodel_selectbykey = "SELECT UserOrganisationStructureCapabilityKey, UserOrganisationStructureKey, CapabilityKey FROM [2AM].[OrgStruct].[UserOrganisationStructureCapability] WHERE UserOrganisationStructureCapabilityKey = @PrimaryKey";
        public const string userorganisationstructurecapabilitydatamodel_delete = "DELETE FROM [2AM].[OrgStruct].[UserOrganisationStructureCapability] WHERE UserOrganisationStructureCapabilityKey = @PrimaryKey";
        public const string userorganisationstructurecapabilitydatamodel_deletewhere = "DELETE FROM [2AM].[OrgStruct].[UserOrganisationStructureCapability] WHERE";
        public const string userorganisationstructurecapabilitydatamodel_insert = "INSERT INTO [2AM].[OrgStruct].[UserOrganisationStructureCapability] (UserOrganisationStructureKey, CapabilityKey) VALUES(@UserOrganisationStructureKey, @CapabilityKey); select cast(scope_identity() as int)";
        public const string userorganisationstructurecapabilitydatamodel_update = "UPDATE [2AM].[OrgStruct].[UserOrganisationStructureCapability] SET UserOrganisationStructureKey = @UserOrganisationStructureKey, CapabilityKey = @CapabilityKey WHERE UserOrganisationStructureCapabilityKey = @UserOrganisationStructureCapabilityKey";



        public const string capabilityinheritancedatamodel_selectwhere = "SELECT CapabilityInheritanceKey, UserOrganisationStructureCapabilityKey, OrganisationStructureKey FROM [2AM].[OrgStruct].[CapabilityInheritance] WHERE";
        public const string capabilityinheritancedatamodel_selectbykey = "SELECT CapabilityInheritanceKey, UserOrganisationStructureCapabilityKey, OrganisationStructureKey FROM [2AM].[OrgStruct].[CapabilityInheritance] WHERE CapabilityInheritanceKey = @PrimaryKey";
        public const string capabilityinheritancedatamodel_delete = "DELETE FROM [2AM].[OrgStruct].[CapabilityInheritance] WHERE CapabilityInheritanceKey = @PrimaryKey";
        public const string capabilityinheritancedatamodel_deletewhere = "DELETE FROM [2AM].[OrgStruct].[CapabilityInheritance] WHERE";
        public const string capabilityinheritancedatamodel_insert = "INSERT INTO [2AM].[OrgStruct].[CapabilityInheritance] (UserOrganisationStructureCapabilityKey, OrganisationStructureKey) VALUES(@UserOrganisationStructureCapabilityKey, @OrganisationStructureKey); select cast(scope_identity() as int)";
        public const string capabilityinheritancedatamodel_update = "UPDATE [2AM].[OrgStruct].[CapabilityInheritance] SET UserOrganisationStructureCapabilityKey = @UserOrganisationStructureCapabilityKey, OrganisationStructureKey = @OrganisationStructureKey WHERE CapabilityInheritanceKey = @CapabilityInheritanceKey";



        public const string mandatetypedatamodel_selectwhere = "SELECT MandateTypeKey, Description FROM [2AM].[OrgStruct].[MandateType] WHERE";
        public const string mandatetypedatamodel_selectbykey = "SELECT MandateTypeKey, Description FROM [2AM].[OrgStruct].[MandateType] WHERE MandateTypeKey = @PrimaryKey";
        public const string mandatetypedatamodel_delete = "DELETE FROM [2AM].[OrgStruct].[MandateType] WHERE MandateTypeKey = @PrimaryKey";
        public const string mandatetypedatamodel_deletewhere = "DELETE FROM [2AM].[OrgStruct].[MandateType] WHERE";
        public const string mandatetypedatamodel_insert = "INSERT INTO [2AM].[OrgStruct].[MandateType] (Description) VALUES(@Description); select cast(scope_identity() as int)";
        public const string mandatetypedatamodel_update = "UPDATE [2AM].[OrgStruct].[MandateType] SET Description = @Description WHERE MandateTypeKey = @MandateTypeKey";



        public const string capabilitymandatedatamodel_selectwhere = "SELECT CapabilityMandateKey, MandateTypeKey, CapabilityKey, StartRange, EndRange FROM [2AM].[OrgStruct].[CapabilityMandate] WHERE";
        public const string capabilitymandatedatamodel_selectbykey = "SELECT CapabilityMandateKey, MandateTypeKey, CapabilityKey, StartRange, EndRange FROM [2AM].[OrgStruct].[CapabilityMandate] WHERE CapabilityMandateKey = @PrimaryKey";
        public const string capabilitymandatedatamodel_delete = "DELETE FROM [2AM].[OrgStruct].[CapabilityMandate] WHERE CapabilityMandateKey = @PrimaryKey";
        public const string capabilitymandatedatamodel_deletewhere = "DELETE FROM [2AM].[OrgStruct].[CapabilityMandate] WHERE";
        public const string capabilitymandatedatamodel_insert = "INSERT INTO [2AM].[OrgStruct].[CapabilityMandate] (MandateTypeKey, CapabilityKey, StartRange, EndRange) VALUES(@MandateTypeKey, @CapabilityKey, @StartRange, @EndRange); select cast(scope_identity() as int)";
        public const string capabilitymandatedatamodel_update = "UPDATE [2AM].[OrgStruct].[CapabilityMandate] SET MandateTypeKey = @MandateTypeKey, CapabilityKey = @CapabilityKey, StartRange = @StartRange, EndRange = @EndRange WHERE CapabilityMandateKey = @CapabilityMandateKey";



    }
}