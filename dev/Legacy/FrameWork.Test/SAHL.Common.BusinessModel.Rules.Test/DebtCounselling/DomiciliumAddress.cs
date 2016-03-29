using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Castle.ActiveRecord;
using SAHL.Test;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.DebtCounselling;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common;
using SAHL.Common.CacheData;
using SAHL.Common.Security;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using System.Data;
using SAHL.Common.Exceptions;
using SAHL.Common.BusinessModel.Helpers;

namespace SAHL.Common.BusinessModel.Rules.Test.DebtCounselling
{
	[TestFixture]
	public class DomiciliumAddress : RuleBase
	{
        IRuleService _ruleService;
        DomainMessageCollection _messages;
        ILookupRepository _lookups;
        ILegalEntityRepository _leRepo;
        IAccountRepository _accRepo;

		[NUnit.Framework.SetUp()]
		public override void Setup()
		{
			base.Setup();
			//SetRepositoryStrategy(TypeFactoryStrategy.Mock);
            _messages = new DomainMessageCollection();
            _lookups = RepositoryFactory.GetRepository<ILookupRepository>();
            _leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
            _ruleService = ServiceFactory.GetService<IRuleService>();
        }

		[TearDown]
		public override void TearDown()
		{
			base.TearDown();
		}

//        [Test]
//        public void LegalEntityAddressAccountDomiciliumAddressCheckFail()
//        {
//            using (new TransactionScope(OnDispose.Rollback))
//            {
//                string sql = @"select top 1 r.AccountKey, r.LegalEntityKey, lea.AddressKey, lea.LegalEntityAddressKey
//                            from [2am].[dbo].[Role] r
//                            join dbo.LegalEntityAddress lea on lea.LegalEntityKey = r.LegalEntityKey
//                            where lea.GeneralStatusKey = 1";
//                DataTable DT = base.GetQueryResults(sql);
//                int accountkey = Convert.ToInt32(DT.Rows[0][0]);
//                int leKey = Convert.ToInt32(DT.Rows[0][1]);
//                int addresskey = Convert.ToInt32(DT.Rows[0][2]);

//                sql = string.Format("INSERT into [2am].dbo.DomiciliumAddress (AccountKey, AddressKey, UsePropertyAddress) values ({0},{1},0)", accountkey, addresskey);
//                this.DBHelper.ExecuteNonQuery(sql);

//                ILegalEntity LE = _leRepo.GetLegalEntityByKey(leKey);
//                ILegalEntityAddress lea = LE.LegalEntityAddresses.Where(x => x.Address.Key == addresskey).FirstOrDefault();
//                lea.GeneralStatus = _lookups.GeneralStatuses[GeneralStatuses.Inactive];

//                _messages.Clear();
//                _ruleService.Enabled = true;
//                int result = _ruleService.ExecuteRule(_messages, "LegalEntityAddressAccountDomiciliumAddressCheck", lea);
//                _ruleService.Enabled = false;

//                Assert.That(result == 0);
//                Assert.That(_messages.Count > 0);
//            }
//        }

//        [Test]
//        public void LegalEntityAddressAccountDomiciliumAddressCheckPass()
//        {
//            using (new TransactionScope(OnDispose.Rollback))
//            {
//                string sql = @"select top 1 r.AccountKey, r.LegalEntityKey, lea.AddressKey, lea.LegalEntityAddressKey
//                            from [2am].[dbo].[Role] r
//                            join dbo.LegalEntityAddress lea on lea.LegalEntityKey = r.LegalEntityKey
//                            where lea.GeneralStatusKey = 1
//                            and lea.AddressKey not in (select AddressKey from dbo.DomiciliumAddress)";
//                DataTable DT = base.GetQueryResults(sql);
//                int accountkey = Convert.ToInt32(DT.Rows[0][0]);
//                int leKey = Convert.ToInt32(DT.Rows[0][1]);
//                int addresskey = Convert.ToInt32(DT.Rows[0][2]);

//                ILegalEntity LE = _leRepo.GetLegalEntityByKey(leKey);
//                ILegalEntityAddress lea = LE.LegalEntityAddresses.Where(x => x.Address.Key == addresskey).FirstOrDefault();
//                lea.GeneralStatus = _lookups.GeneralStatuses[GeneralStatuses.Inactive];

//                _ruleService.Enabled = true;
//                int result = _ruleService.ExecuteRule(_messages, "LegalEntityAddressAccountDomiciliumAddressCheck", lea);
//                _ruleService.Enabled = false;

//                Assert.That(result == 1);
//            }
//        }

//        [Test]
//        public void LegalEntityAddressAccountDomiciliumAddressCheckBeforeRemoveFail()
//        {
//            _messages.Clear();

//            using (new TransactionScope(OnDispose.Rollback))
//            {
//                string sql = @"select top 1 r.AccountKey, r.LegalEntityKey, lea.AddressKey, lea.LegalEntityAddressKey
//                            from [2am].[dbo].[Role] r
//                            join dbo.LegalEntityAddress lea on lea.LegalEntityKey = r.LegalEntityKey
//                            where lea.GeneralStatusKey = 1";
//                DataTable DT = base.GetQueryResults(sql);
//                int accountkey = Convert.ToInt32(DT.Rows[0][0]);
//                int leKey = Convert.ToInt32(DT.Rows[0][1]);
//                int addresskey = Convert.ToInt32(DT.Rows[0][2]);

//                sql = string.Format("INSERT into [2am].dbo.DomiciliumAddress (AccountKey, AddressKey, UsePropertyAddress) values ({0},{1},0)", accountkey, addresskey);
//                this.DBHelper.ExecuteNonQuery(sql);

//                ILegalEntity LE = _leRepo.GetLegalEntityByKey(leKey);
//                ILegalEntityAddress lea = LE.LegalEntityAddresses.Where(x => x.Address.Key == addresskey).FirstOrDefault();

//                _ruleService.Enabled = true;
//                int result = _ruleService.ExecuteRule(_messages, "LegalEntityAddressAccountDomiciliumAddressCheckBeforeRemove", lea);
//                _ruleService.Enabled = false;

//                Assert.That(result == 0);
//                Assert.That(_messages.Count > 0);
//            }
//        }

//        [Test]
//        public void LegalEntityAddressAccountDomiciliumAddressCheckBeforeRemovePass()
//        {
//            using (new TransactionScope(OnDispose.Rollback))
//            {
//                string sql = @"select top 1 r.AccountKey, r.LegalEntityKey, lea.AddressKey, lea.LegalEntityAddressKey
//                            from [2am].[dbo].[Role] r
//                            join dbo.LegalEntityAddress lea on lea.LegalEntityKey = r.LegalEntityKey
//                            where lea.GeneralStatusKey = 1
//                            and lea.AddressKey not in (select AddressKey from dbo.DomiciliumAddress)";
//                DataTable DT = base.GetQueryResults(sql);
//                int accountkey = Convert.ToInt32(DT.Rows[0][0]);
//                int leKey = Convert.ToInt32(DT.Rows[0][1]);
//                int addresskey = Convert.ToInt32(DT.Rows[0][2]);

//                ILegalEntity LE = _leRepo.GetLegalEntityByKey(leKey);
//                ILegalEntityAddress lea = LE.LegalEntityAddresses.Where(x => x.Address.Key == addresskey).FirstOrDefault();

//                _messages.Clear();
//                _ruleService.Enabled = true;
//                int result = _ruleService.ExecuteRule(_messages, "LegalEntityAddressAccountDomiciliumAddressCheckBeforeRemove", lea);
//                _ruleService.Enabled = false;

//                Assert.That(result == 1);
//            }
//        }
       
//        [Test]
//        public void RoleAccountDomiciliumAddressCheckFail()
//        {
//            _messages.Clear();

//            using (new TransactionScope(OnDispose.Rollback))
//            {
//                string sql = @"select top 1 r.AccountKey, r.LegalEntityKey, lea.AddressKey
//                            from [2am].[dbo].[Role] r
//                            join dbo.LegalEntityAddress lea on lea.LegalEntityKey = r.LegalEntityKey
//                            where r.GeneralStatusKey = 1";
//                DataTable DT = base.GetQueryResults(sql);
//                int accountkey = Convert.ToInt32(DT.Rows[0][0]);
//                int leKey = Convert.ToInt32(DT.Rows[0][1]);
//                int addresskey = Convert.ToInt32(DT.Rows[0][2]);

//                sql = string.Format("INSERT into [2am].dbo.DomiciliumAddress (AccountKey, AddressKey, UsePropertyAddress) values ({0},{1},0)", accountkey, addresskey);
//                this.DBHelper.ExecuteNonQuery(sql);

//                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
//                IAccount account = accRepo.GetAccountByKey(accountkey);
//                IRole role = account.Roles.Where(x => x.LegalEntity.Key == leKey).FirstOrDefault();
//                role.GeneralStatus = _lookups.GeneralStatuses[GeneralStatuses.Inactive];


//                //ILegalEntity le = _leRepo.GetLegalEntityByKey(leKey);
//                //IRole roleToRemove = le.Roles.Where(x => x.Key == role.Key).FirstOrDefault();
//                //le.Roles.Remove(_messages, roleToRemove);

//                _ruleService.Enabled = true;
//                int result = _ruleService.ExecuteRule(_messages, "RoleAccountDomiciliumAddressCheck", role);
//                _ruleService.Enabled = false;

//                Assert.That(result == 0);
//                Assert.That(_messages.Count > 0);
//            }
//        }

//        [Test]
//        public void RoleAccountDomiciliumAddressCheckPass()
//        {
//            _messages.Clear();

//            using (new TransactionScope(OnDispose.Rollback))
//            {
//                string sql = @"select top 1 r.AccountKey, r.LegalEntityKey, lea.AddressKey
//                            from [2am].[dbo].[Role] r
//                            join dbo.LegalEntityAddress lea on lea.LegalEntityKey = r.LegalEntityKey
//                            where r.GeneralStatusKey = 1 
//                            and lea.AddressKey not in (select AddressKey from dbo.DomiciliumAddress)";
//                DataTable DT = base.GetQueryResults(sql);
//                int accountkey = Convert.ToInt32(DT.Rows[0][0]);
//                int leKey = Convert.ToInt32(DT.Rows[0][1]);
//                int addresskey = Convert.ToInt32(DT.Rows[0][2]);

//                IAccountRepository accRepo = RepositoryFactory.GetRepository<IAccountRepository>();
//                IAccount account = accRepo.GetAccountByKey(accountkey);
//                IRole role = account.Roles.Where(x => x.LegalEntity.Key == leKey).FirstOrDefault();
//                role.GeneralStatus = _lookups.GeneralStatuses[GeneralStatuses.Inactive];

//                _ruleService.Enabled = true;
//                int result = _ruleService.ExecuteRule(_messages, "RoleAccountDomiciliumAddressCheck", role);
//                _ruleService.Enabled = false;

//                Assert.That(result == 1);
//            }
//        }

//        [Test]
//        public void RoleDomiciliumAddressBeforeRemoveFail()
//        {
//            _messages.Clear();

//            using (new TransactionScope(OnDispose.Rollback))
//            {
//                string sql = @"select top 1 r.AccountKey, r.LegalEntityKey, lea.AddressKey
//                            from [2am].[dbo].[Role] r
//                            join dbo.LegalEntityAddress lea on lea.LegalEntityKey = r.LegalEntityKey
//                            where r.GeneralStatusKey = 1";
//                DataTable DT = base.GetQueryResults(sql);
//                int accountkey = Convert.ToInt32(DT.Rows[0][0]);
//                int leKey = Convert.ToInt32(DT.Rows[0][1]);
//                int addresskey = Convert.ToInt32(DT.Rows[0][2]);

//                sql = string.Format("INSERT into [2am].dbo.DomiciliumAddress (AccountKey, AddressKey, UsePropertyAddress) values ({0},{1},0)", accountkey, addresskey);
//                this.DBHelper.ExecuteNonQuery(sql);

//                IAccount account = _accRepo.GetAccountByKey(accountkey);
//                IRole role = account.Roles.Where(x => x.LegalEntity.Key == leKey).FirstOrDefault();
//                //role.GeneralStatus = _lookups.GeneralStatuses[GeneralStatuses.Inactive];

//                //ILegalEntity le = _leRepo.GetLegalEntityByKey(leKey);
//                //IRole roleToRemove = le.Roles.Where(x => x.Key == role.Key).FirstOrDefault();
//                //le.Roles.Remove(_messages, roleToRemove);
//                //account.Roles.Remove(_messages, role);

//                _ruleService.Enabled = true;
//                int result = _ruleService.ExecuteRule(_messages, "RoleDomiciliumAddressBeforeRemove", role);
//                _ruleService.Enabled = false;

//                Assert.That(result == 0);
//                Assert.That(_messages.Count > 0);
//            }
//        }

//        [Test]
//        public void RoleDomiciliumAddressBeforeRemovePass()
//        {
//            _messages.Clear();

//            using (new TransactionScope(OnDispose.Rollback))
//            {
//                string sql = @"select top 1 r.AccountKey, r.LegalEntityKey, lea.AddressKey
//                            from [2am].[dbo].[Role] r
//                            join dbo.LegalEntityAddress lea on lea.LegalEntityKey = r.LegalEntityKey
//                            where r.GeneralStatusKey = 1 
//                            and lea.AddressKey not in (select AddressKey from dbo.DomiciliumAddress)";
//                DataTable DT = base.GetQueryResults(sql);
//                int accountkey = Convert.ToInt32(DT.Rows[0][0]);
//                int leKey = Convert.ToInt32(DT.Rows[0][1]);
//                int addresskey = Convert.ToInt32(DT.Rows[0][2]);

//                IAccount account = _accRepo.GetAccountByKey(accountkey);
//                IRole role = account.Roles.Where(x => x.LegalEntity.Key == leKey).FirstOrDefault();

//                _ruleService.Enabled = true;
//                int result = _ruleService.ExecuteRule(_messages, "RoleDomiciliumAddressBeforeRemove", role);
//                _ruleService.Enabled = false;

//                Assert.That(result == 1);
//            }
//        }

//        [Test]
//        public void PropertyDomiciliumAddressCheckBeforeRemoveFail()
//        {
//            _messages.Clear();

//            using (new TransactionScope(OnDispose.Rollback))
//            {
//                string sql = @"select top 1 fs.AccountKey ,p.PropertyKey, p.AddressKey  
//                            from [2am].[fin].MortgageLoan ml (nolock)
//                            join [2am].dbo.Property p (nolock) on p.PropertyKey = ml.PropertyKey
//                            join [2am].dbo.FinancialService fs (nolock) on fs.FinancialServiceKey = ml.FinancialServiceKey
//                            where fs.FinancialServiceTypeKey = 1";

//                DataTable DT = base.GetQueryResults(sql);
//                int accountkey = Convert.ToInt32(DT.Rows[0][0]);
//                int propertykey = Convert.ToInt32(DT.Rows[0][1]);
//                int addresskey = Convert.ToInt32(DT.Rows[0][2]);

//                sql = string.Format("INSERT into [2am].dbo.DomiciliumAddress (AccountKey, AddressKey, UsePropertyAddress) values ({0},{1},1)", accountkey, addresskey);
//                this.DBHelper.ExecuteNonQuery(sql);

//                IAccount account = _accRepo.GetAccountByKey(accountkey);
//                IPropertyRepository pRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
//                IProperty property = pRepo.GetPropertyByKey(propertykey);

//                _ruleService.Enabled = true;
//                int result = _ruleService.ExecuteRule(_messages, "PropertyDomiciliumAddressCheckBeforeRemove", property, account);
//                _ruleService.Enabled = false;

//                Assert.That(result == 0);
//                Assert.That(_messages.Count > 0);
//            }
//        }

//        [Test]
//        public void PropertyDomiciliumAddressCheckBeforeRemovePass()
//        {
//            _messages.Clear();

//            using (new TransactionScope(OnDispose.Rollback))
//            {
//                string sql = @"select top 1 fs.AccountKey ,p.PropertyKey, p.AddressKey 
//                            from [2am].[fin].MortgageLoan ml (nolock)
//                            join [2am].dbo.Property p (nolock) on p.PropertyKey = ml.PropertyKey
//                            join [2am].dbo.FinancialService fs (nolock) on fs.FinancialServiceKey = ml.FinancialServiceKey
//                            where fs.FinancialServiceTypeKey = 1
//                            and fs.AccountKey not in (select AccountKey from [2am].dbo.DomiciliumAddress (nolock))";

//                DataTable DT = base.GetQueryResults(sql);
//                int accountkey = Convert.ToInt32(DT.Rows[0][0]);
//                int propertykey = Convert.ToInt32(DT.Rows[0][1]);
//                int addresskey = Convert.ToInt32(DT.Rows[0][2]);

//                IAccount account = _accRepo.GetAccountByKey(accountkey);
//                IPropertyRepository pRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
//                IProperty property = pRepo.GetPropertyByKey(propertykey);

//                _ruleService.Enabled = true;
//                int result = _ruleService.ExecuteRule(_messages, "PropertyDomiciliumAddressCheckBeforeRemove", property, account);
//                _ruleService.Enabled = false;

//                Assert.That(result == 1);
//            }
//        }

//        [Test]
//        public void PropertyDomiciliumAddressChangeFail()
//        {
//            _messages.Clear();

//            using (new TransactionScope(OnDispose.Rollback))
//            {
//                string sql = @"select top 1 p.PropertyKey, p.AddressKey, fs.AccountKey  
//                            from [2am].[fin].MortgageLoan ml (nolock)
//                            join [2am].dbo.Property p (nolock) on p.PropertyKey = ml.PropertyKey
//                            join [2am].dbo.FinancialService fs (nolock) on fs.FinancialServiceKey = ml.FinancialServiceKey
//                            where fs.FinancialServiceTypeKey = 1";
//                DataTable DT = base.GetQueryResults(sql);
//                int propertykey = Convert.ToInt32(DT.Rows[0][0]);
//                int addresskey = Convert.ToInt32(DT.Rows[0][1]);
//                int accountkey = Convert.ToInt32(DT.Rows[0][2]);
 
//                IPropertyRepository pRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
//                IProperty property = pRepo.GetPropertyByKey(propertykey);

//                //make it a domicilium
//                sql = string.Format("INSERT into [2am].dbo.DomiciliumAddress (AccountKey, AddressKey, UsePropertyAddress) values ({0},{1},1)", accountkey, addresskey);
//                this.DBHelper.ExecuteNonQuery(sql);
                
//                //get some other address
//                sql = string.Format("select top 1 AddressKey from dbo.Address where AddressFormatKey = 1 and AddressKey <> {0}", addresskey);
//                DT = base.GetQueryResults(sql);
//                addresskey = Convert.ToInt32(DT.Rows[0][0]);
               
//                IAddressRepository addrRepo = RepositoryFactory.GetRepository<IAddressRepository>();
//                IAddress addr = addrRepo.GetAddressByKey(addresskey);
//                property.Address = addr;

//                _ruleService.Enabled = true;
//                int result = _ruleService.ExecuteRule(_messages, "PropertyDomiciliumAddressChange", property);
//                _ruleService.Enabled = false;
//                //pRepo.SaveAddress(property, addr);
//                Assert.That(result == 0);
//                Assert.That(_messages.Count > 0);
//            }
//        }

//        [Test]
//        public void PropertyDomiciliumAddressChangePass()
//        {
//            _messages.Clear();

//            using (new TransactionScope(OnDispose.Rollback))
//            {
//                string sql = @"select top 1 p.PropertyKey, p.AddressKey, fs.AccountKey  
//                            from [2am].[fin].MortgageLoan ml (nolock)
//                            join [2am].dbo.Property p (nolock) on p.PropertyKey = ml.PropertyKey
//                            join [2am].dbo.FinancialService fs (nolock) on fs.FinancialServiceKey = ml.FinancialServiceKey
//                            where fs.FinancialServiceTypeKey = 1
//                            and fs.AccountKey not in (select AccountKey from [2am].dbo.DomiciliumAddress (nolock))";
//                DataTable DT = base.GetQueryResults(sql);
//                int propertykey = Convert.ToInt32(DT.Rows[0][0]);
//                int addresskey = Convert.ToInt32(DT.Rows[0][1]);
//                int accountkey = Convert.ToInt32(DT.Rows[0][2]);

//                IPropertyRepository pRepo = RepositoryFactory.GetRepository<IPropertyRepository>();
//                IProperty property = pRepo.GetPropertyByKey(propertykey);

//                sql = string.Format("select top 1 AddressKey from dbo.Address where AddressFormatKey = 1 and AddressKey <> {0}", addresskey);
//                DT = base.GetQueryResults(sql);
//                addresskey = Convert.ToInt32(DT.Rows[0][0]);

//                IAddressRepository addrRepo = RepositoryFactory.GetRepository<IAddressRepository>();
//                IAddress addr = addrRepo.GetAddressByKey(addresskey);
//                property.Address = addr;

//                _ruleService.Enabled = true;
//                int result = _ruleService.ExecuteRule(_messages, "PropertyDomiciliumAddressChange", property);
//                _ruleService.Enabled = false;
//                //pRepo.SaveAddress(property, addr);
//                Assert.That(result == 1);
//            }
//        }
	}
}
