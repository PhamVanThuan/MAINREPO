using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections.Interfaces;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Exceptions;

namespace SAHL.Common.BusinessModel.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    [FactoryType(typeof(IDataStorRepository))]
    public class DataStorRepository : AbstractRepositoryBase, IDataStorRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public DataStorRepository()
        {

        }


        #region IDataStorRepository Members

        public SAHL.Common.BusinessModel.Interfaces.IData CreateEmptyData()
        {
			return base.CreateEmpty<IData, Data_DAO>();
			//return new SAHL.Common.BusinessModel.Data(new Data_DAO());
        }

        public SAHL.Common.BusinessModel.Interfaces.ISTOR GetSTORByName(string storName)
        {
            string HQL = "from STOR_DAO s where s.Name = ?";
            SimpleQuery query = new SimpleQuery(typeof(STOR_DAO), HQL, storName);
            STOR_DAO[] o = STOR_DAO.ExecuteQuery(query) as STOR_DAO[];
            if (o.Length == 0)
                return null;

            return new SAHL.Common.BusinessModel.STOR(o[0]);
        }

        public void SaveData(SAHL.Common.BusinessModel.Interfaces.IData data)
        {
			base.Save<IData, Data_DAO>(data);
			//IDAOObject IDAO = data as IDAOObject;
			//Data_DAO dao = (Data_DAO)IDAO.GetDAOObject();
			//dao.SaveAndFlush();
			//if (ValidationHelper.PrincipalHasValidationErrors())
			//    throw new DomainValidationException();
        }

        public void SaveDataList(List<IData> dataList)
        {
            foreach (IData data in dataList)
            {
                IDAOObject IDAO = data as IDAOObject;
                Data_DAO dao = (Data_DAO)IDAO.GetDAOObject();
                dao.SaveAndFlush();
            }
            if (ValidationHelper.PrincipalHasValidationErrors())
                throw new DomainValidationException();
        }

        #endregion
 
    }
}
