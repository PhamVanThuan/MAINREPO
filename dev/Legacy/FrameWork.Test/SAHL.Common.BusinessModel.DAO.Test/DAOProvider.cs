using SAHL.Common.BusinessModel.DAO.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.Common.BusinessModel.DAO.Test
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

        public static IEnumerable GetDaoTypesForFindFirst()
        {
            List<Type> typesToTest = new List<Type>();
            IEnumerable daoList = GetDAOTypes();
            foreach (Type dao in daoList)
            {
                if (IsAttributedWithFindFirst(dao) || !IsAttributedwithIgnore(dao))
                    typesToTest.Add(dao);
            }
            return typesToTest;
        }

        public static IEnumerable GetDaoTypesForLoadSaveLoad()
        {
            List<Type> typesToTest = new List<Type>();
            IEnumerable daoList = GetDAOTypes();
            foreach (Type dao in daoList)
            {
                if (!IsAttributedwithIgnore(dao) && !IsAttributedWithFindFirst(dao))
                    typesToTest.Add(dao);
            }
            return typesToTest;
        }

        private static bool IsAttributedWithFindFirst(Type dao)
        {
            var genericTestAttribute = (from attribute in dao.GetCustomAttributes(typeof(GenericTest), false)
                                        select attribute).FirstOrDefault();
            if (genericTestAttribute != null)
            {
                if (((GenericTest)genericTestAttribute).TestType == Globals.TestType.Find)
                    return true;
            }
            return false;
        }

        private static bool IsAttributedwithIgnore(Type dao)
        {
            var ignoreTestAttribute = (from attribute in dao.GetCustomAttributes(typeof(DoNotTestWithGenericTestAttribute), false)
                                       select attribute).FirstOrDefault();
            if (ignoreTestAttribute != null)
            {
                return true;
            }
            return false;
        }
    }
}