using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IXSLTRepository))]
    public class XSLTRepository : AbstractRepositoryBase, IXSLTRepository
    {
        public IXSLTransformation GetLatestXSLTransformation(GenericKeyTypes genericKeyType)
        {
            string HQL = string.Format("from XSLTransformation_DAO x where x.GenericKeyType.Key = ? order by x.Version desc");
            SimpleQuery<XSLTransformation_DAO> q = new SimpleQuery<XSLTransformation_DAO>(HQL, (int)genericKeyType);
            XSLTransformation_DAO[] xslt = q.Execute();
            return new XSLTransformation(xslt[0]);
        }
    }
}
