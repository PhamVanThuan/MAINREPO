using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.BusinessModel.Interfaces.Repositories;


namespace SAHL.Common.BusinessModel.Test
{
    class LegalEntityOrganisationNodeTest :TestBase
    {

        [Test]
        public void MoveMe()
        {
            using (new TransactionScope(OnDispose.Rollback))
            {
                IControl ctrl = LKRepo.Controls.ObjectDictionary["EstateAgentChannelRoot"];
                ILegalEntityOrganisationNode leonRoot = OSRepo.GetLegalEntityOrganisationNodeForKey((int)ctrl.ControlNumeric.Value);

                //loop through children, first EAON not designation, get a siblings child and then move them
                ILegalEntityOrganisationNode newParent = null;
                ILegalEntityOrganisationNode childToMove = null;

                foreach (ILegalEntityOrganisationNode leon in leonRoot.ChildOrganisationStructures)
                {
                    if (leonRoot.OrganisationType.Key != (int)OrganisationTypes.Designation)
                    {
                        if (newParent == null)
                            newParent = leonRoot;
                        else
                        {
                            foreach (ILegalEntityOrganisationNode leonChild in leonRoot.ChildOrganisationStructures)
                            {
                                if (leonChild.OrganisationType.Key != (int)OrganisationTypes.Designation)
                                {
                                    childToMove = leonChild;
                                    break;
                                }
                            }

                            if (childToMove != null)
                                break;
                        }
                    }

                }

                if (newParent != null && childToMove != null)
                {
                    childToMove.MoveMe(newParent);

                    Assert.That(childToMove.Parent.Key == newParent.Key);
                    Assert.That(newParent.ChildOrganisationStructures.Contains(childToMove));
                }

            }
        }

        private ILookupRepository _lkRepo;
        public ILookupRepository LKRepo
        {
            get
            {
                if (_lkRepo == null)
                    _lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return _lkRepo;
            }
        }

        private IEstateAgentRepository _eaRepo;
        public IEstateAgentRepository EARepo
        {
            get
            {
                if (_eaRepo == null)
                    _eaRepo = RepositoryFactory.GetRepository<IEstateAgentRepository>();
                return _eaRepo;
            }
        }

        private IOrganisationStructureRepository _osRepo;
        public IOrganisationStructureRepository OSRepo
        {
            get
            {
                if (_osRepo == null)
                    _osRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                return _osRepo;
            }
        }
    }
}
