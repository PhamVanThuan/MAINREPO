using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel;
using SAHL.Common.Attributes;
using SAHL.Common.Globals;
using System.Data;
using SAHL.Common.DataAccess;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Validation;
using System.Data.SqlClient;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Factories;
using Castle.ActiveRecord;
using System.Security.Principal;
using SAHL.Common;
using SAHL.Common.Exceptions;
using System.Reflection;
using SAHL.Common.CacheData;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(ISantamPolicyDetailRepository))]
    public class SantamPolicyRepository : AbstractRepositoryBase, ISantamPolicyDetailRepository
    {
        public ISANTAMPolicyTracking GetSantamPolicyByAccountKey(int AccountKey)
        {

            SANTAMPolicyTracking_DAO[] SantamDAO = SANTAMPolicyTracking_DAO.FindAllByProperty("AccountKey", AccountKey);

            if (SantamDAO != null && SantamDAO.Length > 0)
            {
                IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return bmtm.GetMappedType<ISANTAMPolicyTracking>(SantamDAO[0]);
            }

            #region SQL
            //string SQL = String.Format(@"Select spt.* from SANTAMPolicyTracking spt where spt.AccountKey = {0}", AccountKey);
            //SimpleQuery<SANTAMPolicyTracking_DAO> query
            //= new SimpleQuery<SANTAMPolicyTracking_DAO>(QueryLanguage.Sql, SQL);
            
            //SANTAMPolicyTracking_DAO[] sptDAO = query.Execute();

            //if (sptDAO.Length > 0)
            //{
            //    IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
            //    return bmtm.GetMappedType<ISANTAMPolicyTracking>(sptDAO[0]);
            //}
            #endregion


            #region HQL

            //string HQL = "from SANTAMPolicyTracking_DAO spt where spt.AccountKey = ?";
            
            //SimpleQuery<SANTAMPolicyTracking_DAO> query
            //     = new SimpleQuery<SANTAMPolicyTracking_DAO>(HQL, AccountKey);

            //query.SetQueryRange(1);
            //SANTAMPolicyTracking_DAO[] sptDAO = query.Execute();

            //if (sptDAO.Length > 0)
            //{
            //    IBusinessModelTypeMapper bmtm = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
            //    return bmtm.GetMappedType<ISANTAMPolicyTracking>(sptDAO[0]);
            //}
            #endregion
            
            return null;
        }
    }
}
