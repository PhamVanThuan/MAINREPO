using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SAHL.Test;
using SAHL.Common;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NUnit.Framework;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using Rhino.Mocks;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;
using System.Configuration;
using NHibernate.Expression;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class ApplicationDocumentTest : TestBase
    {
        [Test]
        public void ApplicationDocumentSave()
        {
            IList<int> KeyList = new List<int>();

            using (new SessionScope())
            {
                ApplicationDocument_DAO appDoc = new ApplicationDocument_DAO();
                IApplicationDocument appDoc1 = new ApplicationDocument(appDoc);
                DocumentType_DAO docType = DocumentType_DAO.FindFirst();
                Application_DAO app = Application_DAO.FindFirst();
                appDoc.DocumentType = docType;
                appDoc.Application = app;
                appDoc.DocumentReceivedBy = "user";
                //appDoc.DocumentReceivedDate = DateTime.Now;
                appDoc.GenericKey = 1;
                appDoc.Description = "test";
                appDoc.SaveAndFlush();
                KeyList.Add(appDoc.Key);
            }
            using (new SessionScope())
            {
                ApplicationDocument_DAO.DeleteAll(KeyList);
            }
        }
    }
}
