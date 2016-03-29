using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Queries;
using NHibernate;
using NHibernate.Criterion;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.Service;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.DomainMessages;
using SAHL.Common.Exceptions;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Interfaces;
using SAHL.Common.Security;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IEstateAgentRepository))]
    class EstateAgentRepository : AbstractRepositoryBase, IEstateAgentRepository
    {
        public EstateAgentRepository(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        public EstateAgentRepository()
        {
            this.castleTransactionService = new CastleTransactionsService();
        }

        private ICastleTransactionsService castleTransactionService;

        public void SaveEstateAgentOrganisationStructure(IEstateAgentOrganisationNode eaos)
        {
            base.Save<IEstateAgentOrganisationNode, OrganisationStructure_DAO>(eaos);
        }

        public void GetEstateAgentInfoWithHistory(int legalEntityKey, DateTime maxDate, out ILegalEntity Company, out ILegalEntity Branch, out ILegalEntity Principal)
        {
            Company = null;
            Branch = null;
            Principal = null;

            string query = UIStatementRepository.GetStatement("Repositories.OrganisationStructureRepository", "EstateAgentInfo");
            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@LegalEntityKey", legalEntityKey));
            prms.Add(new SqlParameter("@MaxDate", maxDate));

            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), prms);

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    switch (Convert.ToInt32(dr["OrganisationStructureTypeKey"]))
                    {
                        case (int)OrganisationTypes.Branch_Originator:
                            Branch = LERepo.GetLegalEntityByKey(Convert.ToInt32(dr["LegalEntityKey"]));
                            break;
                        case (int)OrganisationTypes.Company:
                            Company = LERepo.GetLegalEntityByKey(Convert.ToInt32(dr["LegalEntityKey"]));
                            break;
                        case (int)OrganisationTypes.Designation: //this will always be the principal based on the uiStatement
                            Principal = LERepo.GetLegalEntityByKey(Convert.ToInt32(dr["LegalEntityKey"]));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //private IOrganisationStructureRepository _osRepo;

        //private IOrganisationStructureRepository OSRepo
        //{
        //    get
        //    {
        //        if (_osRepo == null)
        //            _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

        //        return _osRepo;
        //    }
        //}

        private ILegalEntityRepository _leRepo;

        private ILegalEntityRepository LERepo
        {
            get
            {
                if (_leRepo == null)
                    _leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return _leRepo;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Key">The Organisation Structure Key</param>
        /// <returns></returns>
        public IEstateAgentOrganisationNode GetEstateAgentOrganisationNodeForKey(int Key)
        {
            OrganisationStructure_DAO dao = OrganisationStructure_DAO.Find(Key);

            if (dao != null)
                return (IEstateAgentOrganisationNode)organisationStructureFactory.GetLEOSNode(dao);
            else
                return null;
        }

        /// <summary>
        /// Get the IEstateAgentOrganisationNode for the LegalEntityKey provided
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEstateAgentOrganisationNode GetEstateAgentOrganisationNodeForLegalEntity(int key)
        {
            LegalEntityOrganisationStructure_DAO[] daoList = LegalEntityOrganisationStructure_DAO.FindAllByProperty("LegalEntity.Key", key);

            if (daoList != null && daoList.Length > 0)
                return (IEstateAgentOrganisationNode)organisationStructureFactory.GetLEOSNode(daoList[0].OrganisationStructure);
            else
                return null;
        }

        private OrganisationStructureRepository.OrganisationStructureFactory _organisationStructureFactory;

        private OrganisationStructureRepository.OrganisationStructureFactory organisationStructureFactory
        {
            get
            {
                if (_organisationStructureFactory == null)
                {
                    _organisationStructureFactory = new OrganisationStructureRepository.OrganisationStructureFactory();
                    _organisationStructureFactory.OrganisationStructureNodeType = OrganisationStructureNodeTypes.EstateAgent;
                }
                return _organisationStructureFactory;
            }
        }
    }
}