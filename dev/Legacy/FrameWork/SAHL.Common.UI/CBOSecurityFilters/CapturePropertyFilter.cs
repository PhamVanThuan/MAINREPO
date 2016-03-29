using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.CacheData;

namespace SAHL.Common.UI.CBOSecurityFilters
{
    public class CapturePropertyFilter : GenericRegExFilter
    {

        protected bool _shouldwefilter;

        public CapturePropertyFilter()
            : base()
        {
            if (!CheckForFilter())
            {
                return;
            }
            _filters.Add(new Regex("^capture property\\b", RegexOptions.IgnoreCase));            
        }

        protected bool CheckForFilter()
        {
            _shouldwefilter = true;
            // check if we have a valid instance node, if not we cannot filter our Context menu.
            if (_instanceNode == null)
            {
                _shouldwefilter = false;
                return false;
            }

            IX2Repository X2Rep = RepositoryFactory.GetRepository<IX2Repository>();
            IInstance Instance = X2Rep.GetInstanceByKey(_instanceNode.InstanceID);

            //Instance.

            IEventList<IWorkList> Worklists = Instance.WorkLists;

            // check for the QA state.
            if (Instance.State.Name.ToLower() == "qa")
            {
                foreach (IWorkList WL in Worklists)
                {
                    if (WL.ADUserName.ToLower() == _currentPrincipal.Identity.Name.ToLower())
                    {
                        _shouldwefilter = false;
                        return false;
                    }
                }
            }

            if (Instance.State.Name.ToLower() == "manage application")
            {
                if (_currentPrincipal.IsInRole("New Business Supervisor"))
                {
                    _shouldwefilter = false;
                    return false;
                }
                if (_currentPrincipal.IsInRole("New Business Processor"))
                {
                    _shouldwefilter = false;
                    return false;
                }

                if (_currentPrincipal.IsInRole("New Business Manager"))
                {
                    _shouldwefilter = false;
                    return false;
                }

            }

            if (Instance.State.Name.ToLower() == "resubmission")
            {
                if (_currentPrincipal.IsInRole("Resubmission Admin"))
                {
                    _shouldwefilter = false;
                    return false;
                }
            }
            return _shouldwefilter;
        }

        /// <summary>
        /// this function always returns true, it needs to be overridden to change this.
        /// </summary>
        /// <returns></returns>
        protected override bool ShouldWeFilter()
        {
            return _shouldwefilter;
        }
    }
}
