using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Configuration;
using SAHL.Common.DomainMessages;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Rules.Service;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Service.Mandates;

namespace SAHL.Common.Service
{
    [FactoryType(typeof(IMandateService))]
    public class MandateService : IMandateService
    {
        private Assembly _assembly;

        public MandateService()
        {
            _assembly = Assembly.Load("SAHL.Common.BusinessModel");
        }

        public IList<IADUser> ExecuteMandateSet(string MandateSet, params object[] parameters)
        {
            AllocationMandateSetGroup_DAO[] groups = AllocationMandateSetGroup_DAO.FindAllByProperty("AllocationGroupName", MandateSet);
            if (groups.Length == 0) throw new Exception(string.Format("MandateSet: {0} does not exist in the database", MandateSet));

            List<IADUser> Users = new List<IADUser>();
            // build a list of mandates that need to be run (NB at this stage Im assuming that there is a 1:1 from allocationMandateSet to
            // allocationMandateOperator in the DB. So for evey AllocationmandateSet there will be
            if (null == groups[0].AllocationMandateSets || groups[0].AllocationMandateSets.Count == 0)
                return new List<IADUser>();

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            IAllocationMandateSet set = BMTM.GetMappedType<IAllocationMandateSet>(groups[0].AllocationMandateSets[0]);

            List<bool> Sets = new List<bool>();
            List<string> Debug = new List<string>();
            // Start at false, If any of the results are true will pass
            bool b = false;
            bool b1 = false;
            int order = 1;
            foreach (IAllocationMandateOperator ams in set.AllocationMandateOperators)
            {
                b1 = ExecuteMandate(ams.AllocationMandate.Name, parameters);
                Debug.Add(string.Format("{0}  :{1}", b1, ams.AllocationMandate.Name));
                if (order != ams.Order)
                {
                    // add the previous set result
                    Sets.Add(b);
                    if (!b)
                    {
                        //string WeShouldReturnHere = "yes";
                        break;
                    }
                    // Start at false for the next set. If any of the results are true will pass
                    b = false;
                    order = ams.Order;
                    b = b1 | b;
                }
                else
                {
                    b = b1 | b;
                }
                //if (!b)
                //{
                //    break;
                //}
            }
            // add the last set result
            Sets.Add(b);
            b1 = true;
            foreach (bool b2 in Sets)
            {
                b1 = b2 & b1;
            }
            if (b1)
            {
                foreach (IUserOrganisationStructure uos in set.UserOrganisationStructures)
                {
                    if (!Users.Contains(uos.ADUser))
                        Users.Add(uos.ADUser);
                }
            }
            if (b1)
                return Users;
            return new List<IADUser>();
        }

        public bool ExecuteMandate(string Mandate, params object[] parameters)
        {
            AllocationMandate_DAO[] mandates = AllocationMandate_DAO.FindAllByProperty("Name", Mandate);
            if (mandates.Length == 0)
                throw new Exception(string.Format("Mandate: {0} does not exist in the database", Mandate));

            AllocationMandate_DAO mandate = mandates[0];

            string createErrorMessage = string.Format("Unable to create mandate {0}", mandate.TypeName);
            Type type = _assembly.GetType(mandate.TypeName);
            if (type == null)
                throw new Exception(createErrorMessage);

            IMandate m = null;
            try
            {
                m = Activator.CreateInstance(type) as IMandate;
                if (m == null)
                    throw new Exception(createErrorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(createErrorMessage, ex);
            }

            IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
            bool b = m.StartMandate(BMTM.GetMappedType<IAllocationMandate>(mandate));
            b = m.ExecuteMandate(parameters);
            m.CompleteMandate();
            return b;
        }
    }
}