using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.CacheData;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using Castle.ActiveRecord;
using SAHL.Common.Globals;
using SAHL.Web.Views.Administration.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Web.UI.Events;
using System.Data;
using SAHL.Common.UI;
using SAHL.Common;
using SAHL.Common.DomainMessages;

namespace SAHL.Web.Views.Administration.Presenters
{
    /// <summary>
    /// Attorney Contact Base
    /// </summary>
    public class AttorneyContactBase : SAHLCommonBasePresenter<SAHL.Web.Views.Administration.Interfaces.IAttorneyContact>
    {
        /// <summary>
        /// Should extra roles be required to be added to the presenter this should be the next point
        /// after changing the uistatement.
        /// </summary>
        private readonly List<int> _externalRoleTypeKeys = new List<int>
        {
            (int)SAHL.Common.Globals.ExternalRoleTypes.DebtCounselling,
            (int)SAHL.Common.Globals.ExternalRoleTypes.DeceasedEstates,
            (int)SAHL.Common.Globals.ExternalRoleTypes.Foreclosure,
            (int)SAHL.Common.Globals.ExternalRoleTypes.Sequestrations,
            (int)SAHL.Common.Globals.ExternalRoleTypes.WebAccess
        };

        private const string _legalEntityKey = "LegalEntityKey";
        private const string _externalLERoles = "ExternalLERoles";
        private int _attorneyKey;
        private DataTable attorneyExternalLERoles;

        private List<ICacheObjectLifeTime> _lifeTimes;
        protected List<ICacheObjectLifeTime> LifeTimes
        {
            get
            {
                if (_lifeTimes == null)
                {
                    List<string> views = new List<string>();
                    views.Add("AttorneyDetails");
                    views.Add("AttorneyDetailsAdd");
                    views.Add("AttorneyDetailsUpdate");
                    views.Add("AttorneyContact");
                    _lifeTimes = new List<ICacheObjectLifeTime>();
                    _lifeTimes.Add(new SimplePageCacheObjectLifeTime(views));
                }
                return _lifeTimes;
            }
        }

        private ILookupRepository lookupRepository;
        protected ILookupRepository LookupRepository
        {
            get
            {
                if (lookupRepository == null)
                {
                    lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                }

                return lookupRepository;
            }
        }

        private ICommonRepository commonRepository;
        protected ICommonRepository CommonRepository
        {
            get
            {
                if (commonRepository == null)
                    commonRepository = RepositoryFactory.GetRepository<ICommonRepository>();

                return commonRepository;
            }
        }

        private ILegalEntityRepository leRepo;
        protected ILegalEntityRepository LERepo
        {
            get
            {
                if (leRepo == null)
                    leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();

                return leRepo;
            }
        }

        private ILookupRepository lkRepo;
        protected ILookupRepository LKRepo
        {
            get
            {
                if (lkRepo == null)
                    lkRepo = RepositoryFactory.GetRepository<ILookupRepository>();

                return lkRepo;
            }
        }

        protected Dictionary<int, string> ExternalRoleTypes
        {
            get
            {   Dictionary<int, string> dict = new Dictionary<int,string>();
                foreach (var key in _externalRoleTypeKeys)
                {
                    dict.Add(key, LKRepo.ExternalRoleTypes.ObjectDictionary[key.ToString()].Description);            
                }
                return dict;
            }
        }

        protected DataTable GetExternalRoles
        {
            get { return (DataTable)this.GlobalCacheData[_externalLERoles]; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view"></param>
        /// <param name="controller"></param>
        public AttorneyContactBase(SAHL.Web.Views.Administration.Interfaces.IAttorneyContact view, SAHLCommonBaseController controller)
            : base(view, controller)
		{    
            _view.OnSelectionChanged +=new KeyChangedEventHandler(_view_OnSelectionChanged);
            _view.OnAddToCBO += new KeyChangedEventHandler(_view_OnAddToCBO);
            _view.LegalEntityAdd += new EventHandler<LegalEntityEventArgs>(OnLegalEntityAdd);
            _view.OnCheckedChanged += new SAHL.Common.Web.UI.Events.KeyChangedEventHandler(_view_OnCheckedChanged);
            _view.OnDone += new EventHandler(_view_OnDone);
		}

        protected void _view_OnDone(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("AttorneyDetails");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (GlobalCacheData.ContainsKey("AttorneyKey"))
            {
                _attorneyKey = (int)GlobalCacheData["AttorneyKey"];
            }

            _view.BindExternalRoleTypes(LookupRepository.LitigationAttorneyRoleTypes);

            SAHL.Common.BusinessModel.Interfaces.IAttorney attorney = LERepo.GetAttorneyByKey(_attorneyKey);
            _view.AttorneyName = attorney.LegalEntity.DisplayName;
            attorneyExternalLERoles = LERepo.GetExternalRolesByAttorneyKey(_attorneyKey);
            RefreshExternalRoles();
            _view.ExternalRoleTypeKeys = ExternalRoleTypes;
            _view.SetUplitigationAttorneyGrid(GetExternalRoles);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewPreRender(object sender, EventArgs e)
        {
            base.OnViewPreRender(sender, e);
        }

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnAddToCBO(object sender, KeyChangedEventArgs e)
        {
                //Rebind with edit mode off
                _view.BindSetUplitigationAttorneyGridPostRowUpdate(GetExternalRoles);

                // add the selected legal entity to the cbo and navigate
                int _selectedLegalEntityKey = Convert.ToInt32(e.Key);

                this.GlobalCacheData.Remove(_legalEntityKey);

                if (_selectedLegalEntityKey <= 0)
                    return;

                // get the top level legal entity static node
                CBOMenuNode topParentNode = CBOManager.GetCBOMenuNodeByUrl(_view.CurrentPrincipal, "ClientSuperSearch", CBONodeSetType.CBO);
                bool alreadyAdded = false;

                // do a check to ensure that the legal entity hasn't already been added
                foreach (CBOMenuNode childNode in topParentNode.ChildNodes)
                {
                    if (childNode.GenericKey == _selectedLegalEntityKey)
                    {
                        CBOManager.SetCurrentCBONode(_view.CurrentPrincipal, childNode, CBONodeSetType.CBO);
                        alreadyAdded = true;
                        break;
                    }
                }

                if (!alreadyAdded)
                {
                    ICBOMenu ClientNameTemplate = GetLegalEntityTemplate(topParentNode);
                    CBOManager.AddCBOMenuToNode(_view.CurrentPrincipal, topParentNode, ClientNameTemplate, _selectedLegalEntityKey, GenericKeyTypes.LegalEntity, CBONodeSetType.CBO);
                }

                // navigate to selected node
                //base.Navigator.Navigate(CBOManager.GetCurrentCBONode(_view.CurrentPrincipal).URL);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void _view_OnSelectionChanged(object sender, KeyChangedEventArgs e)
        {
            int leKey = Convert.ToInt32(e.Key);
            if (GlobalCacheData.ContainsKey(_legalEntityKey))
                this.GlobalCacheData[_legalEntityKey] = leKey;
            else
                this.GlobalCacheData.Add(_legalEntityKey, leKey, LifeTimes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _view_OnCheckedChanged(object sender, KeyChangedEventArgs e)
        {
            Dictionary<string, string> dict = sender as Dictionary<string, string>;

            int leKey = Convert.ToInt32(e.Key);
            int attKey = _attorneyKey;
            ExternalRoleTypes erType = (ExternalRoleTypes)Enum.Parse(typeof(ExternalRoleTypes), dict["ExternalRoleTypeKey"]);
            GeneralStatuses gs = GeneralStatuses.Inactive;
            if (Convert.ToBoolean(dict["Checked"]))
            {
                gs = GeneralStatuses.Active;
            }

            this.ExclusionSets.Add(RuleExclusionSets.LegalEntityAttorneyContactDetails);
            
            ILegalEntity legalEntity = LERepo.GetLegalEntityByKey(leKey);
            //Only Update the Legal Entity Login if there is one
            if (legalEntity.LegalEntityLogin != null)
            {
                legalEntity.LegalEntityLogin.GeneralStatus = LookupRepository.GeneralStatuses[gs];
                LERepo.SaveLegalEntityLogin(legalEntity.LegalEntityLogin);
            }
            
            SaveExternalRole(attKey, leKey, erType, gs);
            this.ExclusionSets.Remove(RuleExclusionSets.LegalEntityAttorneyContactDetails);

            RefreshExternalRoles();
            _view.BindSetUplitigationAttorneyGridPostRowUpdate(GetExternalRoles);
            this.GlobalCacheData.Remove(_legalEntityKey);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnLegalEntityAdd(object sender, LegalEntityEventArgs e)
        {
            this.ExclusionSets.Add(RuleExclusionSets.LegalEntityAttorneyContactDetails);
            
            IRuleService svcRule = ServiceFactory.GetService<IRuleService>();
            
            ILegalEntityLogin legalEntityLogin = LERepo.CreateEmptyLegalEntityLogin();
            legalEntityLogin.Username = e.EmailAddress;

            svcRule.ExecuteRule(_view.Messages, "LegalEntityLoginDuplicateUsername", legalEntityLogin);

            if (_view.IsValid)
            {
                using (TransactionScope txn = new TransactionScope())
                {
                    try
                    {
                        //Ensure that the rule exclusion set is in place
                        ILegalEntityNaturalPerson legalEntity = (ILegalEntityNaturalPerson)LERepo.GetEmptyLegalEntity(SAHL.Common.Globals.LegalEntityTypes.NaturalPerson);
                        legalEntity.FirstNames = e.FirstName;
                        legalEntity.Surname = e.Surname;
                        legalEntity.HomePhoneNumber = e.TelephoneNumber;
                        legalEntity.HomePhoneCode = e.TelephoneCode;
                        legalEntity.EmailAddress = e.EmailAddress;
                        legalEntity.FaxNumber = e.FaxNumber;
                        legalEntity.FaxCode = e.FaxCode;
                        legalEntity.DocumentLanguage = CommonRepository.GetLanguageByKey((int)Languages.English);
                        legalEntity.IntroductionDate = DateTime.Now;

                        //Save the Legal Entity
                        LERepo.SaveLegalEntity(legalEntity, false);

                        //Insert External Role
                        LERepo.InsertExternalRole((ExternalRoleTypes)e.RoleTypeKey, _attorneyKey, SAHL.Common.Globals.GenericKeyTypes.Attorney, legalEntity.Key, false);

                        txn.VoteCommit();

                        RefreshExternalRoles();
                        _view.BindSetUplitigationAttorneyGridPostRowUpdate(GetExternalRoles);
                        _view.ClearLegalEntityFields();
                    }
                    catch (Exception)
                    {
                        txn.VoteRollBack();
                        if (_view.IsValid)
                            throw;
                    }
                }
            }
        }

        #endregion

        #region Helpers

        private void RefreshExternalRoles()
        {
            attorneyExternalLERoles = LERepo.GetExternalRolesByAttorneyKey(_attorneyKey);
            if (GlobalCacheData.ContainsKey(_externalLERoles))
                this.GlobalCacheData[_externalLERoles] = attorneyExternalLERoles;
            else
                this.GlobalCacheData.Add(_externalLERoles, attorneyExternalLERoles, LifeTimes);
        }

        private void SaveExternalRole(int attKey, int leKey, ExternalRoleTypes erType, GeneralStatuses gs)
        {
            if ((int)gs == (int)GeneralStatuses.Inactive)
            {
                //get the role to deactivate
                IExternalRole er = LERepo.GetExternalRoles(attKey, GenericKeyTypes.Attorney, erType, GeneralStatuses.Active, leKey)[0];
                er.GeneralStatus = LKRepo.GeneralStatuses[gs];
                SaveExternalRole(er);
            }
            else
            {
                //insert a new role
                InsertExternalRole(erType, attKey, leKey);
            }
        }

        private void InsertExternalRole(ExternalRoleTypes ert, int attKey, int leKey)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                LERepo.InsertExternalRole(ert, attKey, GenericKeyTypes.Attorney, leKey, false);

                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (View.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }
        }

        private void SaveExternalRole(IExternalRole er)
        {
            TransactionScope txn = new TransactionScope();
            try
            {
                LERepo.SaveExternalRole(er);

                txn.VoteCommit();
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (View.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }
        }

        private static ICBOMenu GetLegalEntityTemplate(CBOMenuNode LegalEntitiesNode)
        {
            for (int i = 0; i < LegalEntitiesNode.CBOMenu.ChildMenus.Count; i++)
            {
                if (LegalEntitiesNode.CBOMenu.ChildMenus[i].Description == "ClientName")
                    return LegalEntitiesNode.CBOMenu.ChildMenus[i];
            }
            return null;
        }

        #endregion
    }
}