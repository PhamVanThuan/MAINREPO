using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Data;
using SAHL.Common.BusinessModel.DAO;
using System.Text.RegularExpressions;
using SAHL.Common.CacheData;
using SAHL.Common.Security;
using SAHL.Common.Globals;

namespace SAHL.Common.UI.CBOSecurityFilters
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkFlowReadOnlyFilter : GenericRegExFilter
    {

        protected bool _shouldwefilter;

        public WorkFlowReadOnlyFilter()
            : base()
        {
            // here we need to decide if the current user is allowed an unfiltered context menu.
            if (!CheckForFilter())
            {
                return;
            }

            _filters.Add(new Regex("^add\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^update\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^delete\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^remove\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^capture property\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^associate asset/liability\\b", RegexOptions.IgnoreCase));

            _NodesNotToFilter.Add("Add Application Memo");
        }

        protected override bool ShouldWeFilter()
        {
            return _shouldwefilter;
        }

        /// <summary>
        /// We check whether this user can be assigned this instance, or is already. If he/she can, we don't filter the context menu, otherwise we do.
        /// </summary>
        /// <returns>true if we are to filter, false if not.</returns>
        protected bool CheckForFilter()
        {
            // check if we have a valid instance node, if not we cannot filter our Context menu.
            if (_instanceNode == null)
            {
                _shouldwefilter = false;
                return false;
            }
            
            IX2Repository X2Rep = RepositoryFactory.GetRepository<IX2Repository>();
            IInstance Instance = X2Rep.GetInstanceByKey(_instanceNode.InstanceID);
            IEventList<IWorkList> Worklists = Instance.WorkLists;
            // first check for assignment, the only check required if a case is assigned to a user.
            foreach (IWorkList WL in Worklists)
            {
                if (WL.ADUserName.ToLower() == _currentPrincipal.Identity.Name.ToLower())
                {
                    _shouldwefilter = false;
                    return false;
                }
            }

            // now lets check for static User Groups, if the current user is in any, we don't filter and need check no further.
            IEventList<ISecurityGroup> SecurityGroups = Instance.State.SecurityGroups;
            foreach (ISecurityGroup SG in SecurityGroups)
            { 
                if ((SG.IsDynamic == false)&&(_currentPrincipal.IsInRole(SG.Name)))
                {
                    _shouldwefilter = false;
                    return false;
                }
            }

            ISecurityRepository secRepo = RepositoryFactory.GetRepository<ISecurityRepository>();
            IADUser adUser = secRepo.GetADUserByPrincipal(_currentPrincipal.Identity.Name);

            if (!X2Rep.CanInstanceBeAssignedToUser(Instance, adUser))
                _shouldwefilter = true;
            else
                _shouldwefilter = false;

            return _shouldwefilter;
        }
    }
}
