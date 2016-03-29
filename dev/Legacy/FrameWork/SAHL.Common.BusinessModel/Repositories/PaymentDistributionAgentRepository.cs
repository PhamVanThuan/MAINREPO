using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Attributes;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Globals;
using Castle.ActiveRecord.Queries;
using Castle.ActiveRecord;
using NHibernate;
using SAHL.Common.Exceptions;
using SAHL.Common.CacheData;
using System.Collections.ObjectModel;
using NHibernate.Criterion;
using System.Data;
using SAHL.Common.DataAccess;
using System.Data.SqlClient;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.Interfaces;
using SAHL.Common.DomainMessages;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IPaymentDistributionAgentRepository))]
    class PaymentDistributionAgentRepository : AbstractRepositoryBase, IPaymentDistributionAgentRepository
    {

        public void SavePaymentDistributionAgentOrganisationStructure(IPaymentDistributionAgentOrganisationNode eaos)
        {
            base.Save<IPaymentDistributionAgentOrganisationNode, OrganisationStructure_DAO>(eaos);
        }

        private IOrganisationStructureRepository _osRepo;

        private IOrganisationStructureRepository OSRepo
        {
            get
            {
                if (_osRepo == null)
                    _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();

                return _osRepo;
            }
        }

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
        public IPaymentDistributionAgentOrganisationNode GetPaymentDistributionAgentOrganisationNodeForKey(int Key)
        {
            OrganisationStructure_DAO dao = OrganisationStructure_DAO.Find(Key);

			if (dao != null)
			{
				return (IPaymentDistributionAgentOrganisationNode)organisationStructureFactory.GetLEOSNode(dao);
			}
			else
				return null;
        }

        /// <summary>
        /// Get the IPaymentDistributionAgentOrganisationNode for the LegalEntityKey provided
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IPaymentDistributionAgentOrganisationNode GetPaymentDistributionAgentOrganisationNodeForLegalEntity(int key)
        {
            LegalEntityOrganisationStructure_DAO[] daoList = LegalEntityOrganisationStructure_DAO.FindAllByProperty("LegalEntity.Key", key);

			if (daoList != null && daoList.Length > 0)
			{
				return (IPaymentDistributionAgentOrganisationNode)organisationStructureFactory.GetLEOSNode(daoList[0].OrganisationStructure);
			}
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
					_organisationStructureFactory.OrganisationStructureNodeType = OrganisationStructureNodeTypes.PaymentDistributionAgency;
				}
				return _organisationStructureFactory;
			}
		}
    }
}
