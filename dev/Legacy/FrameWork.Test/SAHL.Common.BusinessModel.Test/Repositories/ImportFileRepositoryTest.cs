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
using SAHL.Common.Factories;
using System.Xml;
using SAHL.Common.BusinessModel.Repositories;

namespace SAHL.Common.BusinessModel.Test
{
    [TestFixture]
    public class ImportFileRepositoryTest : TestBase
    {
        [Test]
        public void GetImportHistory_FromRepository()
        {
            using (new SessionScope())
            {
                IImportFileRepository repo = RepositoryFactory.GetRepository<IImportFileRepository>();
                IReadOnlyEventList<IImportFile> importLegalEntities = repo.GetImportHistory();
                Assert.IsTrue(importLegalEntities.Count > 0);
            }
        }

        [Test]
        public void GetImportResultsByFileKey_FromRepository()
        {
            using (new SessionScope())
            {
                int fileKey = ImportFile_DAO.FindFirst().Key;

                IImportFileRepository repo = RepositoryFactory.GetRepository<IImportFileRepository>();
                IReadOnlyEventList<IImportLegalEntity> importLegalEntities = repo.GetImportResultsByFileKey(fileKey);
                Assert.IsTrue(importLegalEntities.Count > 0);
            }
        }

        [Test]
        public void GetImportResultsByFileKey_HQL()
        {
            using (new SessionScope())
            {

                int fileKey = ImportFile_DAO.FindFirst().Key;

                string HQL = "from ImportLegalEntity_DAO L where L.ImportApplication.ImportFile.Key = ?";
                SimpleQuery query = new SimpleQuery(typeof(SAHL.Common.BusinessModel.DAO.ImportLegalEntity_DAO), HQL, fileKey);
                query.SetQueryRange(10);
                ImportLegalEntity_DAO[] res = ImportLegalEntity_DAO.ExecuteQuery(query) as ImportLegalEntity_DAO[];

                IEventList<IImportLegalEntity> list = new DAOEventList<ImportLegalEntity_DAO, IImportLegalEntity, ImportLegalEntity>(res);
                ReadOnlyEventList<IImportLegalEntity> importLegalEntities = new ReadOnlyEventList<IImportLegalEntity>(list);

                Assert.IsTrue(importLegalEntities.Count > 0);
            }
        }


        [Test]
        public void GetRCSUploadExportPath_FromRepository()
        {
            try
            {
                IImportFileRepository repo = RepositoryFactory.GetRepository<IImportFileRepository>();
                string exportpath = repo.GetRCSUploadExportPath();
            }
            catch
            {
                Assert.Fail("Error with method");
            }


        }




        [Test]
        public void LoadRCS_CSVFile_FromRepository()
        {

            IXsdAbstraction xsdAbs;
            xsdAbs = null as IXsdAbstraction;
            List<string> missingColumns = new List<string>();
            List<string> extraColumns = new List<string>();
            Dictionary<string, List<string>> invalidData = new Dictionary<string, List<string>>();
            DataTable DT = new DataTable();
                       
            missingColumns.Add("test");
            extraColumns.Add("test");

            DT.Columns.Add("Test", typeof(string));
            DT.Columns.Add("Test2", typeof(string));
            DT.Rows.Add("val1", "val2");
      
            try
            {
                IImportFileRepository repo = RepositoryFactory.GetRepository<IImportFileRepository>();
                repo.LoadRCS_CSVFile("test", "test", ref DT, ref xsdAbs, ref missingColumns, ref extraColumns, ref invalidData);
            }
            catch
            {
                Assert.Fail("Error with method");
            }

        }

       
        [Test]
        public void GenerateXML_FromRepository()
        {
           
            List<string> missingColumns = new List<string>();
            List<string> extraColumns = new List<string>();
            Dictionary<string, List<string>> invalidData = new Dictionary<string, List<string>>();
            DataTable DT = new DataTable();
            missingColumns.Add("test");
            extraColumns.Add("test");

            DT.Columns.Add("Test", typeof(string));
            DT.Columns.Add("Test2", typeof(string));
            DT.Rows.Add("val1", "val2");


            try
            {
                IImportFileRepository repo = RepositoryFactory.GetRepository<IImportFileRepository>();
                string result = repo.GenerateXML(DT,  ref missingColumns, ref extraColumns, ref invalidData);
            }
            catch
            {
                Assert.Fail("Error with method");
            }




        }

      
        [Test]
        public void CreateEmptyImportFile_FromRepository()
        {
            try
            {

                IImportFileRepository repo = RepositoryFactory.GetRepository<IImportFileRepository>();
                IImportFile Impf = repo.CreateEmptyImportFile();
            }
            catch
            {
                Assert.Fail("Error with method");
            }

        }


       
        [Test]
        public void ImportDataFromXML_FromRepository()
        {
            try
            {
                IImportFileRepository repo = RepositoryFactory.GetRepository<IImportFileRepository>();
                IImportFile Impf = repo.CreateEmptyImportFile();
                int file = repo.ImportDataFromXML("test", Impf);

            }
            catch
            {
                Assert.Fail("Error with method");
            }
        }

       
        [Test]
        public void GetSimpleNode_FromRepository()
        {
            try
            {
                IXsdAbstraction xsdAbs;
                xsdAbs = null as IXsdAbstraction;
                IImportFileRepository repo = RepositoryFactory.GetRepository<IImportFileRepository>();
                ISimpleNode ISN = repo.GetSimpleNode("test", xsdAbs);
                

            }
            catch
            {
                Assert.Fail("Error with method");
            }
        }

        /// <summary>
        /// Exposes all functions that are needed for the CSV and XML data
        /// </summary>
        internal class CsvXml
        {
            /// <summary>
            /// Removes characters from a string
            /// </summary>
            /// <param name="victim"></param>
            private static string RemoveXS(string victim)
            {
                int idx = victim.IndexOf(":");
                if (idx > -1)
                    victim = victim.Remove(0, idx + 1);
                return victim;
            }

            /// <summary>
            /// Creates an element node
            /// </summary>
            /// <param name="xsdAbs"></param>
            /// <param name="node"></param>
     
        }
    }
}
