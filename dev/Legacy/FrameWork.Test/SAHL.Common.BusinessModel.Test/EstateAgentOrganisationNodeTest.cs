using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SAHL.Test;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using System.Data;
using SAHL.Common.Globals;
using SAHL.Common.CacheData;
using SAHL.Common.Security;


namespace SAHL.Common.BusinessModel.Test
{
	[TestFixture]
    public class EstateAgentOrganisationNodeTest : TestBase
    {
        [Test]
        public void GetPrincipal()
        {
            SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            int principalOSKey = 0;
            int consultantOSKey = 0;
            int companyOSKey = 0;

            string query = @"select top 1  os.OrganisationStructureKey as ConsultantOSKey, 
                            Prin.OrganisationStructureKey as PrincipalOSKey , 
                            Prin.parentkey as CompanyOSKey     
                            from 
                            (select parentkey , OrganisationStructureKey 
                            from OrganisationStructure nolock
                            where description like 'Principal' and GeneralStatuskey = 1) as Prin
                            inner join OrganisationStructure os on os.parentkey = Prin.parentkey
                            where description like 'Consultant' and GeneralStatuskey = 1
";

            DataTable DT = base.GetQueryResults(query);
            if (DT.Rows.Count > 0)
            {

                consultantOSKey = Convert.ToInt32(DT.Rows[0][0]);
                principalOSKey = Convert.ToInt32(DT.Rows[0][1]);
                companyOSKey = Convert.ToInt32(DT.Rows[0][2]);

                //Test for Principal
                Assert.IsTrue(GetPrincipalTestHelper(principalOSKey));

                //Test for ConsultantsPrincipal
                Assert.IsTrue(GetPrincipalTestHelper(consultantOSKey));


                //Test for Company should fail, and throws an exception
                //the domain message collection needs to be cleared after this call
                Assert.IsTrue(GetPrincipalTestHelper(companyOSKey));


                spc.DomainMessages.Clear();
            }

        }

        private bool GetPrincipalTestHelper(int oskey)
        {
            using (new SessionScope())
            {

                //select @OrganisationStructureKey = ControlNumeric from control where ControlDescription = 'EstateAgentChannelRoot'
                IEstateAgentOrganisationNode eaon;
                //int oskey = 2340;

                eaon = EARepo.GetEstateAgentOrganisationNodeForKey(oskey);

                ILegalEntityNaturalPerson lenp = eaon.GetEstateAgentPrincipal();

                if (lenp == null)
                    return false;

                return true;
            }
        }


        [Test]
        public void MoveMeCompany()
        {
            #region CompanyMove
            using (new TransactionScope(OnDispose.Rollback))
            {
                SAHLPrincipalCache spc = SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
                spc.DomainMessages.Clear();

                IControl ctrl = LKRepo.Controls.ObjectDictionary["EstateAgentChannelRoot"];
                IEstateAgentOrganisationNode eaonRoot = EARepo.GetEstateAgentOrganisationNodeForKey((int)ctrl.ControlNumeric.Value);
                IEstateAgentOrganisationNode newParent = null;

                //loop through children, first EAON not designation, get a siblings child and then move them

                IEstateAgentOrganisationNode childToMove = null;

                foreach (IEstateAgentOrganisationNode eaon in eaonRoot.ChildOrganisationStructures)
                {
                    if (eaon.OrganisationType.Key != (int)OrganisationTypes.Designation)
                    {
                        if (newParent == null)
                            newParent = eaon;
                        else
                        {
                            foreach (IEstateAgentOrganisationNode eaonChild in eaon.ChildOrganisationStructures)
                            {
                                if (eaonChild.OrganisationType.Key != (int)OrganisationTypes.Designation)
                                {
                                    childToMove = eaonChild;
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
                    childToMove.MoveMe(newParent, null);

                    Assert.That(childToMove.Parent.Key == newParent.Key);
                    Assert.That(newParent.ChildOrganisationStructures.Contains(childToMove));
                }

            }
            #endregion
        }

        [Test]
        public void MoveMeDesignation()
        {
            try
            {

                #region DesignationMove

                using (new TransactionScope(OnDispose.Rollback))
                {
                    IControl ctrl = LKRepo.Controls.ObjectDictionary["EstateAgentChannelRoot"];
                    IEstateAgentOrganisationNode eaonRoot = EARepo.GetEstateAgentOrganisationNodeForKey((int)ctrl.ControlNumeric.Value);
                    IEstateAgentOrganisationNode newParent = null;


                    //get a designation, move it to a different parent
                    IEstateAgentOrganisationNode eaonD = null;

                    foreach (IEstateAgentOrganisationNode eaon in eaonRoot.ChildOrganisationStructures)
                    {
                        if (eaon.OrganisationType.Key != (int)OrganisationTypes.Designation && eaon.ChildOrganisationStructures.Count > 0)
                        {
                            foreach (IEstateAgentOrganisationNode child in eaon.ChildOrganisationStructures)
                            {
                                if (child.Description == Constants.EstateAgent.Principal)
                                {
                                    newParent = eaon;
                                    break;
                                }
                                else
                                    continue;
                            }
                        }
                    }


                    foreach (IEstateAgentOrganisationNode eaon in eaonRoot.ChildOrganisationStructures)
                    {
                        //walk down a seperate tree node to get a child
                        if (eaon.Key != newParent.Key)
                        {
                            eaonD = GetChild(eaon, OrganisationTypes.Designation, Constants.EstateAgent.Consultant);
                        }
                        else
                        {
                            foreach (IEstateAgentOrganisationNode eaonC in eaon.ChildOrganisationStructures)
                            {
                                eaonD = GetChild(eaonC, OrganisationTypes.Designation, Constants.EstateAgent.Consultant);
                                if (eaonD != null)
                                    break;
                            }
                        }
                        if (eaonD != null && eaonD.Description != Constants.EstateAgent.Principal)
                            break;
                    }



                    if (newParent != null && eaonD != null && eaonD.LegalEntities.Count > 0)
                    {
                        //Get the LegalEntity to move
                        ILegalEntity le = eaonD.LegalEntities[0];

                        IOrganisationType ot = eaonD.OrganisationType;
                        string desc = eaonD.Description;

                        eaonD.MoveMe(newParent, le);

                        //check the LE exists in the collection of the destination Node
                        IEstateAgentOrganisationNode eaon = (IEstateAgentOrganisationNode)newParent.FindChild(ot, desc, OrganisationStructureNodeTypes.EstateAgent);
                        Assert.That(eaon.LegalEntities.Contains(le));
                    }

                }

                #endregion
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        private IEstateAgentOrganisationNode GetChild(IEstateAgentOrganisationNode eaon, OrganisationTypes ot, string description)
        {
            if (eaon.OrganisationType.Key == (int)ot)
            {
                return eaon;
            }
            //If I am not who I am looking for recurse through children
            foreach (IEstateAgentOrganisationNode eaonC in eaon.ChildOrganisationStructures)
            {
                IEstateAgentOrganisationNode eaonF = GetChild(eaonC, ot, description);
                if (eaonF != null)
                {
                    return eaonF;
                }
            }

            return null;
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

    }
}
