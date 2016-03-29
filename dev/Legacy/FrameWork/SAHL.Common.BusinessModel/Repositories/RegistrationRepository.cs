using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Castle.ActiveRecord.Queries;
using NHibernate.Type;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Exceptions;
using SAHL.Common.Globals;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IRegistrationRepository))]
    public class RegistrationRepository : AbstractRepositoryBase, IRegistrationRepository
    {
        public RegistrationRepository()
        {
            if (castleTransactionService == null)
            {
                castleTransactionService = new CastleTransactionsService();
            }
        }

        public RegistrationRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public void DeleteRegmailByAccountKey(int accountKey)
        {
            string sql = string.Format("delete from RegMail where LoanNumber = ?");
            SimpleQuery<RegMail_DAO> SQ = new SimpleQuery<RegMail_DAO>(QueryLanguage.Sql, sql, accountKey);
            RegMail_DAO.ExecuteQuery(SQ);
        }

        public IRegMail GetRegmailByAccountKey(int accountKey)
        {
            string query = "select r from RegMail_DAO r where r.LoanNumber = ?";
            SimpleQuery<RegMail_DAO> SQ = new SimpleQuery<RegMail_DAO>(query, accountKey);

            object o = RegMail_DAO.ExecuteQuery(SQ);
            RegMail_DAO[] Accs = o as RegMail_DAO[];
            if (Accs != null && Accs.Length == 1)
            {
                IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IRegMail, RegMail_DAO>(Accs[0]);
            }
            return null;
        }

        public void SaveRegmail(IRegMail regmail)
        {
            base.Save<IRegMail, RegMail_DAO>(regmail);
        }

        public IList<IAttorney> GetAttorneysByDeedsOfficeKey(int deedsOfficeKey)
        {
            string query = "select a from Attorney_DAO a where a.DeedsOffice.Key = ? ";
            SimpleQuery<Attorney_DAO> q = new SimpleQuery<Attorney_DAO>(query, deedsOfficeKey);
            Attorney_DAO[] res = q.Execute();

            IList<IAttorney> retval = new List<IAttorney>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new Attorney(res[i]));
            }

            return retval;
        }

        public IList<IAttorney> GetAttorneysByDeedsOfficeKeyAndOSKey(int deedsOfficeKey, int OriginationSourceKey)
        {
            /*string query = "select a from Attorney_DAO a where a.DeedsOffice.Key = ? ";
            SimpleQuery<Attorney_DAO> q = new SimpleQuery<Attorney_DAO>(query, deedsOfficeKey);
            Attorney_DAO[] res = q.Execute();

            IList<IAttorney> retval = new List<IAttorney>();

            for (int i = 0; i < res.Length; i++)
            {
                retval.Add(new Attorney(res[i]));
            }

            return retval;
             */
            IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
            IList<IAttorney> retval = new List<IAttorney>();

            string sql = @"select att.*
            from [2am].[dbo].[Attorney] att (nolock)
            inner join [2am].[dbo].[DeedsOffice] do (nolock)
	            on  att.DeedsOfficeKey = do.DeedsOfficeKey
            inner join [2am].[dbo].[OriginationSourceAttorney] osAtt (nolock)
                on att.AttorneyKey = osAtt.AttorneyKey
            where osAtt.OriginationSourceKey = ? and do.DeedsOfficeKey = ?";

            SimpleQuery<Attorney_DAO> attQ = new SimpleQuery<Attorney_DAO>(QueryLanguage.Sql, sql, OriginationSourceKey, deedsOfficeKey);
            attQ.AddSqlReturnDefinition(typeof(Attorney_DAO), "att");
            Attorney_DAO[] res = attQ.Execute();

            foreach (Attorney_DAO att in res)
            {
                retval.Add(BMTM.GetMappedType<IAttorney, Attorney_DAO>(att));
            }

            return retval;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetLitigationAttorneys()
        {
            string query = UIStatementRepository.GetStatement("Repositories.RegistrationRepository", "GetLegalFeeAttorney");

            // execute
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(Account_DAO), null);

            // Get the Return Values

            Dictionary<int, string> dt = new Dictionary<int, string>();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    dt.Add(Convert.ToInt32(dr[0].ToString()), dr[1].ToString());
                }
            }

            return dt;
        }

        public IAttorney GetAttorneyByLegalEntityKey(int legalEntityKey)
        {
            string query = "select a from Attorney_DAO a where a.LegalEntity.Key = ?";
            SimpleQuery<Attorney_DAO> SQ = new SimpleQuery<Attorney_DAO>(query, legalEntityKey);

            object o = Attorney_DAO.ExecuteQuery(SQ);
            Attorney_DAO[] Atts = o as Attorney_DAO[];
            if (Atts != null && Atts.Length == 1)
            {
                IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IAttorney, Attorney_DAO>(Atts[0]);
            }
            return null;
        }

        public IAttorney GetAttorneyByKey(int AttorneyKey)
        {
            /*
            string query = "select a from Attorney_DAO a where a.Key = ?";
            SimpleQuery<Attorney_DAO> SQ = new SimpleQuery<Attorney_DAO>(query, AttorneyKey);
            object o = Attorney_DAO.ExecuteQuery(SQ);
            Attorney_DAO[] Atts = o as Attorney_DAO[];
            */
            Attorney_DAO[] Atts = Attorney_DAO.FindAllByProperty("Key", AttorneyKey);
            if (Atts != null && Atts.Length == 1)
            {
                IBusinessModelTypeMapper BMTM = SAHL.Common.Factories.TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IAttorney, Attorney_DAO>(Atts[0]);
            }
            return null;
        }
    }
}