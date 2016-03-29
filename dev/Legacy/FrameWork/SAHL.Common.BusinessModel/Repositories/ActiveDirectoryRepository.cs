using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections;
using Castle.ActiveRecord.Queries;
using System.Data;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Exceptions;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord;
using NHibernate;
using System.Collections;
using SAHL.Common.X2.BusinessModel.DAO;
using SAHL.Common.Globals;
using System.Data.SqlClient;
using NHibernate.Criterion;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Interfaces;
using System.DirectoryServices;
using ActiveDs;
using BdsSoft.DirectoryServices.Linq;
using System.Linq;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IActiveDirectoryRepository))]
    public class ActiveDirectoryRepository : AbstractRepositoryBase, IActiveDirectoryRepository
    {
        public IList<ActiveDirectoryUserBindableObject> GetActiveDirectoryUsers(string ADUserNamePartial)
        {
            IList<ActiveDirectoryUserBindableObject> activeDirectoryUsers = new List<ActiveDirectoryUserBindableObject>();

            // get login credentials from control table
            IControlRepository controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
            string serviceUser = controlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.ServiceUser).ControlText;
            string servicePassword = controlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.ServicePassword).ControlText;
            string serviceDomain = controlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.ServiceDomain).ControlText;

            // setup active directory connection
            DirectoryEntry ROOT = new DirectoryEntry("LDAP://DC=" + serviceDomain + ",DC=COM", serviceUser, servicePassword);

            // get list of users from active directory using LINQ to AD provider method
            // must have FirstName & Surname & Phone Number or Cell Number
            var users = new DirectorySource<ActiveDirectoryUser>(ROOT, SearchScope.Subtree);

            var result2 = from usr in users
                          where usr.AccountName == (ADUserNamePartial + "*")
                          select usr;

            foreach (var user in result2)
            {
                ActiveDirectoryUserBindableObject activeDirectoryUser = new ActiveDirectoryUserBindableObject();
                activeDirectoryUser.ADUserName = user.AccountName;
                activeDirectoryUser.FirstName = user.FirstName;
                activeDirectoryUser.Surname = user.Surname;
                activeDirectoryUser.EmailAddress = user.Email;
                activeDirectoryUser.CellNumber = user.CellPhone;

                activeDirectoryUsers.Add(activeDirectoryUser);
            }

            return activeDirectoryUsers;
        }

        public ActiveDirectoryUserBindableObject GetActiveDirectoryUser(string ADUserName)
        {
            ActiveDirectoryUserBindableObject activeDirectoryUser = new ActiveDirectoryUserBindableObject();

            // get login credentials from control table
            IControlRepository controlRepo = RepositoryFactory.GetRepository<IControlRepository>();
            IOrganisationStructureRepository orgStructureRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            string serviceUser = controlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.ServiceUser).ControlText;
            string servicePassword = controlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.ServicePassword).ControlText;
            string serviceDomain = controlRepo.GetControlByDescription(SAHL.Common.Constants.ControlTable.ServiceDomain).ControlText;

            // setup active directory connection
            DirectoryEntry ROOT = new DirectoryEntry("LDAP://DC=" + serviceDomain + ",DC=COM", serviceUser, servicePassword);

            // get user from active directory using LINQ to AD provider method
            var users = new DirectorySource<ActiveDirectoryUser>(ROOT, SearchScope.Subtree);

            var result2 = from usr in users
                          where usr.AccountName == ADUserName
                          select usr;

            foreach (var user in result2)
            {
                activeDirectoryUser.ADUserName = user.AccountName;
                activeDirectoryUser.FirstName = user.FirstName;
                activeDirectoryUser.Surname = user.Surname;
                activeDirectoryUser.EmailAddress = user.Email;
                activeDirectoryUser.CellNumber = user.CellPhone;
                break;
            }

            return activeDirectoryUser;
        }

        #region fxcop
        //private List<string> GetADPropertyValues(SearchResult result, string propertyName)
        //{
        //    ResultPropertyValueCollection rpc = result.Properties[propertyName];
        //    List<string> values = new List<string>(rpc.Count);
        //    foreach (object val in rpc)
        //    {
        //        values.Add(val.ToString());
        //    }
        //    return values;
        //}
        #endregion
        
    }

    [DirectorySchema("user", typeof(IADsUser))]
    class ActiveDirectoryUser : DirectoryEntity
    {
        [DirectoryAttribute("samaccountname")]
        public string AccountName { get; set; }

        [DirectoryAttribute("givenName")]
        public string FirstName { get; set; }

        [DirectoryAttribute("sn")]
        public string Surname { get; set; }

        [DirectoryAttribute("mail")]
        public string Email { get; set; }

        [DirectoryAttribute("mobile")]
        public string CellPhone { get; set; }
    }
}
