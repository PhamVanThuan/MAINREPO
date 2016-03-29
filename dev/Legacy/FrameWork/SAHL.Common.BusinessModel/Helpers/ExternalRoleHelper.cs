using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Helpers
{
    public static class ExternalRoleHelper
    {
        public static IList<ILegalEntity> GetExternalRoleList(int genericKey, GenericKeyTypes gkType, ExternalRoleTypes externalRoleType, GeneralStatuses generalStatus)
        {
            string HQL = "select er.LegalEntity from ExternalRole_DAO er where er.GenericKey = ? and er.GenericKeyType.Key = ? and er.ExternalRoleType.Key = ? and er.GeneralStatus.Key = ? ";

            SimpleQuery<LegalEntity_DAO> q = new SimpleQuery<LegalEntity_DAO>(HQL,
                genericKey,
                (int)gkType,
                (int)externalRoleType,
                (int)generalStatus);
            LegalEntity_DAO[] list = q.Execute();

            IEventList<ILegalEntity> listM = new DAOEventList<LegalEntity_DAO, ILegalEntity, SAHL.Common.BusinessModel.LegalEntity>(list);

            IList<ILegalEntity> listLE = listM.ToList<ILegalEntity>();

            return listLE;
        }

        //public static IList<IExternalRole> GetExternalRoles(int genericKey, GenericKeyTypes gkType, GeneralStatuses generalStatus)
        //{
        //    string HQL = "select er from ExternalRole_DAO er where er.GenericKey = ? and er.GenericKeyType.Key = ? and er.GeneralStatus.Key = ? ";

        //    SimpleQuery<ExternalRole_DAO> q = new SimpleQuery<ExternalRole_DAO>(HQL,
        //        genericKey,
        //        (int)gkType,
        //        (int)generalStatus);
        //    ExternalRole_DAO[] list = q.Execute();

        //    IEventList<IExternalRole> listR = new DAOEventList<ExternalRole_DAO, IExternalRole, SAHL.Common.BusinessModel.ExternalRole>(list);

        //    IList<IExternalRole> listER = listR.ToList<IExternalRole>();

        //    return listER;
        //}

        //public static IList<IExternalRole> GetExternalRoles(int genericKey, GenericKeyTypes gkType)
        //{
        //    string HQL = "select er from ExternalRole_DAO er where er.GenericKey = ? and er.GenericKeyType.Key = ? and er.GeneralStatus.Key = ? ";

        //    SimpleQuery<ExternalRole_DAO> q = new SimpleQuery<ExternalRole_DAO>(HQL,
        //        genericKey,
        //        (int)gkType);
        //    ExternalRole_DAO[] list = q.Execute();

        //    IEventList<IExternalRole> listR = new DAOEventList<ExternalRole_DAO, IExternalRole, SAHL.Common.BusinessModel.ExternalRole>(list);

        //    IList<IExternalRole> listER = listR.ToList<IExternalRole>();

        //    return listER;
        //}

        public static ILegalEntity GetExternalRole(int genericKey, GenericKeyTypes gkType, ExternalRoleTypes externalRoleType, GeneralStatuses generalStatus)
        {
            IList<ILegalEntity> listLE = GetExternalRoleList(genericKey, gkType, externalRoleType, generalStatus);

            if (listLE.Count > 0)
                return listLE[0];
            else
                return null;
        }
    }
}
