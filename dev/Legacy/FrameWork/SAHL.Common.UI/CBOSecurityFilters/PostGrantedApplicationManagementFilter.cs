using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Security;
using System.Text.RegularExpressions;
using SAHL.Common.Factories;
//using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;

namespace SAHL.Common.UI.CBOSecurityFilters
{
    public class PostGrantedApplicationManagementFilter : GenericRegExFilter
    {

        bool _shouldwefilter;

        public PostGrantedApplicationManagementFilter()
            : base()
        {
            if (_applicationKey == 0) // we can't filter if we don't have an application.
            {
                _shouldwefilter = false;
                return;
            }
            ICommonRepository commonRepo = RepositoryFactory.GetRepository<ICommonRepository>();
            IApplication App = commonRepo.GetByKey<IApplication>(_applicationKey);

            if (App == null)
            {
                _shouldwefilter = false;
                return;
            }

            IApplicationInformation AppInfo = App.GetLatestApplicationInformation();
            if (AppInfo.ApplicationInformationType.Description == "Accepted Offer")
            {
                _shouldwefilter = true;
            }
            else
            {
                _shouldwefilter = false;
                return;
            }

            // _userGroupNodes.Add(new UserGroupNodes());  
            UserGroupNodes _tobeadded = new UserGroupNodes();
            _tobeadded.UserGroup = "Registrations Administrator";
            _tobeadded.Nodes.Add("Legal Entity Details");
            _tobeadded.Nodes.Add("Valuations");
            _tobeadded.Nodes.Add("Home Owners Cover");
            _userGroupNodes.Add(_tobeadded);

            _tobeadded = new UserGroupNodes();
            _tobeadded.UserGroup = "Registrations Supervisor";
            _tobeadded.Nodes.Add("Legal Entity Details");
            _tobeadded.Nodes.Add("Valuations");
            _tobeadded.Nodes.Add("Home Owners Cover");
            _tobeadded.Nodes.Add("Property Summary");
            _tobeadded.Nodes.Add("Property Details");
            _userGroupNodes.Add(_tobeadded);

            _tobeadded = new UserGroupNodes();
            _tobeadded.UserGroup = "Registrations Manager";
            _tobeadded.Nodes.Add("Legal Entity Details");
            _tobeadded.Nodes.Add("Valuations");
            _tobeadded.Nodes.Add("Home Owners Cover");
            _tobeadded.Nodes.Add("Property Summary");
            _tobeadded.Nodes.Add("Property Details");
            _userGroupNodes.Add(_tobeadded);


            _filters.Add(new Regex("^Add Legal Entity\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^Remove Legal Entity\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^Update Legal Entity Details\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^Capture Property\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^Update Property Details\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^Update Inspection Contact Details\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^Update Deeds Office Details\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^Update Property Address\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^Update Valuation\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^Update HOC Details\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^Add Loan Agreement\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^Manage Loan Conditions\\b", RegexOptions.IgnoreCase));
            _filters.Add(new Regex("^Update Application Loan Attributes\\b", RegexOptions.IgnoreCase));


            //IStageDefinitionRepository SDRep = RepositoryFactory.GetRepository<IStageDefinitionRepository>();
            //if (SDRep.CountCompositeStageOccurance(_applicationKey, GenericKeyTypes.Offer, 1232) > 0)
            //    _shouldwefilter = true;  
        }

        protected override bool ShouldWeFilter()
        {
            return _shouldwefilter;
        }
    }
}
