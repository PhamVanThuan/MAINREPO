using System;
using SAHL.Core.Data;

namespace SAHL.Core.Data.Models.Capitec
{
	public partial class UIStatements : IUIStatementsProvider
    {
		
		public const string countrydatamodel_selectwhere = "SELECT Id, SAHLCountryKey, CountryName FROM [Capitec].[geo].[Country] WHERE";
		public const string countrydatamodel_selectbykey = "SELECT Id, SAHLCountryKey, CountryName FROM [Capitec].[geo].[Country] WHERE Id = @PrimaryKey";
		public const string countrydatamodel_delete = "DELETE FROM [Capitec].[geo].[Country] WHERE Id = @PrimaryKey";
		public const string countrydatamodel_deletewhere = "DELETE FROM [Capitec].[geo].[Country] WHERE";
		public const string countrydatamodel_insert = "INSERT INTO [Capitec].[geo].[Country] (Id, SAHLCountryKey, CountryName) VALUES(@Id, @SAHLCountryKey, @CountryName); ";
		public const string countrydatamodel_update = "UPDATE [Capitec].[geo].[Country] SET Id = @Id, SAHLCountryKey = @SAHLCountryKey, CountryName = @CountryName WHERE Id = @Id";



		public const string provincedatamodel_selectwhere = "SELECT Id, SAHLProvinceKey, ProvinceName, CountryId FROM [Capitec].[geo].[Province] WHERE";
		public const string provincedatamodel_selectbykey = "SELECT Id, SAHLProvinceKey, ProvinceName, CountryId FROM [Capitec].[geo].[Province] WHERE Id = @PrimaryKey";
		public const string provincedatamodel_delete = "DELETE FROM [Capitec].[geo].[Province] WHERE Id = @PrimaryKey";
		public const string provincedatamodel_deletewhere = "DELETE FROM [Capitec].[geo].[Province] WHERE";
		public const string provincedatamodel_insert = "INSERT INTO [Capitec].[geo].[Province] (Id, SAHLProvinceKey, ProvinceName, CountryId) VALUES(@Id, @SAHLProvinceKey, @ProvinceName, @CountryId); ";
		public const string provincedatamodel_update = "UPDATE [Capitec].[geo].[Province] SET Id = @Id, SAHLProvinceKey = @SAHLProvinceKey, ProvinceName = @ProvinceName, CountryId = @CountryId WHERE Id = @Id";



		public const string citydatamodel_selectwhere = "SELECT Id, SAHLCityKey, CityName, ProvinceId FROM [Capitec].[geo].[City] WHERE";
		public const string citydatamodel_selectbykey = "SELECT Id, SAHLCityKey, CityName, ProvinceId FROM [Capitec].[geo].[City] WHERE Id = @PrimaryKey";
		public const string citydatamodel_delete = "DELETE FROM [Capitec].[geo].[City] WHERE Id = @PrimaryKey";
		public const string citydatamodel_deletewhere = "DELETE FROM [Capitec].[geo].[City] WHERE";
		public const string citydatamodel_insert = "INSERT INTO [Capitec].[geo].[City] (Id, SAHLCityKey, CityName, ProvinceId) VALUES(@Id, @SAHLCityKey, @CityName, @ProvinceId); ";
		public const string citydatamodel_update = "UPDATE [Capitec].[geo].[City] SET Id = @Id, SAHLCityKey = @SAHLCityKey, CityName = @CityName, ProvinceId = @ProvinceId WHERE Id = @Id";



		public const string suburbdatamodel_selectwhere = "SELECT Id, SAHLSuburbKey, SuburbName, PostalCode, CityId FROM [Capitec].[geo].[Suburb] WHERE";
		public const string suburbdatamodel_selectbykey = "SELECT Id, SAHLSuburbKey, SuburbName, PostalCode, CityId FROM [Capitec].[geo].[Suburb] WHERE Id = @PrimaryKey";
		public const string suburbdatamodel_delete = "DELETE FROM [Capitec].[geo].[Suburb] WHERE Id = @PrimaryKey";
		public const string suburbdatamodel_deletewhere = "DELETE FROM [Capitec].[geo].[Suburb] WHERE";
		public const string suburbdatamodel_insert = "INSERT INTO [Capitec].[geo].[Suburb] (Id, SAHLSuburbKey, SuburbName, PostalCode, CityId) VALUES(@Id, @SAHLSuburbKey, @SuburbName, @PostalCode, @CityId); ";
		public const string suburbdatamodel_update = "UPDATE [Capitec].[geo].[Suburb] SET Id = @Id, SAHLSuburbKey = @SAHLSuburbKey, SuburbName = @SuburbName, PostalCode = @PostalCode, CityId = @CityId WHERE Id = @Id";



		public const string postofficedatamodel_selectwhere = "SELECT Id, SAHLPostOfficeKey, PostOfficeName, PostalCode, CityId FROM [Capitec].[geo].[PostOffice] WHERE";
		public const string postofficedatamodel_selectbykey = "SELECT Id, SAHLPostOfficeKey, PostOfficeName, PostalCode, CityId FROM [Capitec].[geo].[PostOffice] WHERE Id = @PrimaryKey";
		public const string postofficedatamodel_delete = "DELETE FROM [Capitec].[geo].[PostOffice] WHERE Id = @PrimaryKey";
		public const string postofficedatamodel_deletewhere = "DELETE FROM [Capitec].[geo].[PostOffice] WHERE";
		public const string postofficedatamodel_insert = "INSERT INTO [Capitec].[geo].[PostOffice] (Id, SAHLPostOfficeKey, PostOfficeName, PostalCode, CityId) VALUES(@Id, @SAHLPostOfficeKey, @PostOfficeName, @PostalCode, @CityId); ";
		public const string postofficedatamodel_update = "UPDATE [Capitec].[geo].[PostOffice] SET Id = @Id, SAHLPostOfficeKey = @SAHLPostOfficeKey, PostOfficeName = @PostOfficeName, PostalCode = @PostalCode, CityId = @CityId WHERE Id = @Id";



	}
}