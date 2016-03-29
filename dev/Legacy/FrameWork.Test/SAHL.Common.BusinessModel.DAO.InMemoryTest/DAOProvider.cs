using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SAHL.Common.BusinessModel.DAO.InMemoryTest
{
    public class DAOProvider
    {
        public static IEnumerable GetDAOTypes()
        {
            var daos = (from type in Assembly.Load("SAHL.Common.BusinessModel.DAO").GetTypes()
                        where type.Name.Contains("_DAO")
                        orderby type.Name
                        select type).ToList<Type>();

            return daos;
        }
    }
}