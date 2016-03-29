using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IControlRepository))]
    public class ControlRepository : AbstractRepositoryBase, IControlRepository
    {
        /// <summary>
        /// Gets the IControl object using the specified control description
        /// </summary>
        /// <param name="controlDescription"></param>
        /// <returns></returns>
        public IControl GetControlByDescription(string controlDescription)
        {
            string HQL = "from Control_DAO where ControlDescription = ?";
            SimpleQuery query = new SimpleQuery(typeof(SAHL.Common.BusinessModel.DAO.Control_DAO), HQL, controlDescription);
            Control_DAO[] o = Control_DAO.ExecuteQuery(query) as Control_DAO[];
            if (o.Length == 0)
                return null;

            return new Control(o[0]);
        }
    }
}
