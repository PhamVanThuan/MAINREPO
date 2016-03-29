using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Globals;
using SAHL.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAHL.Common.BusinessModel.Spec.Test.Repositories.Correspondence
{
    public class CorrespondenceTestBase : TestBase
    {
        public static ICorrespondenceRepository _correspondenceRepo;

        public CorrespondenceTestBase()
        {
            _correspondenceRepo = new CorrespondenceRepository();
        }

        public void GetEmailTemplate(ILegalEntity legalEntity, string consultantName, string emailFrom, Int32 genericKey, CorrespondenceTemplates correspondenceTemplate, out string subject, out string body)
        {
            using (new SessionScope())
            {
                _correspondenceRepo.GetEmailTemplate(legalEntity, consultantName, emailFrom, genericKey, correspondenceTemplate, out subject, out body);
            }
        }

        public ICorrespondenceTemplate GetActualTemplate(CorrespondenceTemplates templateType)
        {
            ICorrespondenceTemplate template;
            using (new SessionScope())
            {
                template = new CorrespondenceTemplate(CorrespondenceTemplate_DAO.Find((int)templateType));
            }
            return template;
        }
    }
}
