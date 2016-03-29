using System;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Origination.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using System.Collections.Generic;
using SAHL.Common.CacheData;
using Castle.ActiveRecord;
using SAHL.Common.DomainMessages;
using SAHL.Common.BusinessModel.Helpers;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using SAHL.X2.Framework.Interfaces;

namespace SAHL.Web.Views.Origination.Presenters
{
    public class ApplicationWizardApplicant : SAHLCommonBasePresenter<IApplicationWizardApplicant>
    {
        private ILegalEntityNaturalPerson _lenp;
        private ILookupRepository _lookupRepo;
        private ILegalEntityRepository _leRepo;
        private IApplicationRepository _appRepo;
        protected IApplication _app;
        private bool _skipCalculator;
        int existingLegalEntityKey = -1;
      
      
       
        public ApplicationWizardApplicant(IApplicationWizardApplicant view, SAHLCommonBaseController controller)
            : base(view, controller)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnViewInitialised(object sender, EventArgs e)
        {
            base.OnViewInitialised(sender, e);

            if (_view.IsMenuPostBack)
            {
                GlobalCacheData.Clear();
            }

            if (!_view.ShouldRunPage)
                return;

            _view.OnCancelButtonClicked += new EventHandler(_view_OnCancelButtonClicked);
            _view.OnNextButtonClicked += new EventHandler(_view_OnNextButtonClicked);
            _view.OnBackButtonClicked += new EventHandler(_view_OnBackButtonClicked);

 
            _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            _leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            _appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();

            IEventList<IApplicationSource> marketSource = new EventList<IApplicationSource>();

            for (int i = 0; i < _lookupRepo.ApplicationSources.Count; i++)
            {
                if (_lookupRepo.ApplicationSources[i].GeneralStatus.Key == (int)GeneralStatuses.Active)
                    marketSource.Add(_view.Messages, _lookupRepo.ApplicationSources[i]);
            }

            _view.PopulateMarketingSource(marketSource);

            IApplicationRepository appRepo = RepositoryFactory.GetRepository<IApplicationRepository>();
            if (_view.IsMenuPostBack)
            {
                if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
                {
                    GlobalCacheData.Remove(ViewConstants.ApplicationKey);
                }
                if (GlobalCacheData.ContainsKey("INSTANCEID"))
                {
                    GlobalCacheData.Remove("INSTANCEID");
                }

                if (GlobalCacheData.ContainsKey("SKIPCALCULATOR"))
                {
                    GlobalCacheData.Remove("SKIPCALCULATOR");
                }

                if (GlobalCacheData.ContainsKey("MUSTNAVIGATE"))
                {
                    GlobalCacheData.Remove("MUSTNAVIGATE");
                }

                if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                {
                    GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
                }               
                _view.ShowCancelButton = false;
            }         

            if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
            {
               
                    _app = appRepo.GetApplicationByKey(int.Parse(GlobalCacheData[ViewConstants.ApplicationKey].ToString()));
                    _view.NumberOfApplicants = _app.EstimateNumberApplicants.Value;
                    if (!_view.IsPostBack && _app.OriginationSource != null)
                    {
                        _view.MarketingSource = _app.ApplicationSource.Key.ToString();
                    }
                    if (GlobalCacheData.ContainsKey(ViewConstants.EstateAgentApplication))
                        _view.IsEstateAgentApplication = Convert.ToBoolean(GlobalCacheData[ViewConstants.EstateAgentApplication]);
                    else
                        _view.IsEstateAgentApplication = false;

                    if (GlobalCacheData.ContainsKey(ViewConstants.OldMutualDeveloperLoan))
                        _view.IsOldMutualDeveloperLoan = Convert.ToBoolean(GlobalCacheData[ViewConstants.OldMutualDeveloperLoan]);
                    else
                        _view.IsEstateAgentApplication = false;
                
                
            }
            else
            {
                _view.NumberOfApplicants = 1;
            }

            if (_app != null)
            {
                _view.ShowCancelButton = true;
                _view.ShowBackButton = true;
            }
            else
            {
                _view.ShowCancelButton = false;
            }

            if (GlobalCacheData.ContainsKey("SKIPCALCULATOR"))
            {
                _skipCalculator = true;
            }

            ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
            {
                existingLegalEntityKey = int.Parse(GlobalCacheData[ViewConstants.SelectedLegalEntityKey].ToString());
                ILegalEntity le = LER.GetLegalEntityByKey(existingLegalEntityKey);
                
                _view.BindExistingLegalEntityAndApplication(le, _app);
            }                  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnBackButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Back");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnNextButtonClicked(object sender, EventArgs e)
        {
            long instanceID = 0;

            TransactionScope txn = new TransactionScope();
            try
            {

                if (_view.MarketingSource.Length == 0)
                {
                    ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                    _view.Messages.Add(new Error("Please specify a marketing source.", "Please specify a marketing source."));
                    if (_view.ExistingLegalEntityKey != 0)
                    {
                        ILegalEntity le = LER.GetLegalEntityByKey(_view.ExistingLegalEntityKey);
                        _view.BindExistingLegalEntityAndApplication(le, _app);
                    }

                    return;
                }
                if (_view.NumberOfApplicants < 1)
                {
                    ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                    _view.Messages.Add(new Error("Number of applicants must be specified. Minimum of 1 applicant.", "Number of applicants must be specified. Minimum of 1 applicant."));
                    if (_view.ExistingLegalEntityKey != 0)
                    {
                        ILegalEntity le = LER.GetLegalEntityByKey(_view.ExistingLegalEntityKey);
                        _view.BindExistingLegalEntityAndApplication(le, _app);
                    }
                    return;
                }


               
                if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                {
                    existingLegalEntityKey = int.Parse(GlobalCacheData[ViewConstants.SelectedLegalEntityKey].ToString());
                    _lenp = (ILegalEntityNaturalPerson)_leRepo.GetLegalEntityByKey(existingLegalEntityKey);
                }

                if (_view.LEIDNumber.Length > 0 && _view.ExistingLegalEntityKey == 0 && existingLegalEntityKey == -1)
                {
                    ILegalEntityNaturalPerson foundLe = _leRepo.GetNaturalPersonByIDNumber(_view.LEIDNumber);
                    if (foundLe != null)
                    {
                       _view.Messages.Add(new Error("Please select the legalEntity from the Identity Number dropdown","Please select the legalEntity from the Identity Number dropdown"));
                        return;
                    }
                }

                if (existingLegalEntityKey == -1  && _view.ExistingLegalEntityKey == 0)
                {
                    CreateNewLegalEntity();
                }
                else
                    if (_view.ExistingLegalEntityKey != 0)
                    {
                        _lenp = (ILegalEntityNaturalPerson)_leRepo.GetLegalEntityByKey(_view.ExistingLegalEntityKey);
                    }
                


                if (_lenp == null && existingLegalEntityKey != -1)
                {
                    ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                    _lenp = LER.GetLegalEntityByKey(existingLegalEntityKey) as ILegalEntityNaturalPerson;
                }

                if (_lenp != null)
                {
                    bool mustUpdate = false;
                    
                    if (_lenp.FirstNames == null || _lenp.FirstNames.Trim().Length == 0)
                    {
                        if (_view.LEFirstNames.Trim().Length > 0)
                        {
                            _lenp.FirstNames = _view.LEFirstNames;
                            mustUpdate = true;

                        }
                    }
                    if (_lenp.Surname == null || _lenp.Surname.Trim().Length == 0)
                    {
                        if (_view.LESurname.Trim().Length > 0)
                        {
                            _lenp.Surname = _view.LESurname;
                            mustUpdate = true;
                        }
                    }
                    

                    if (mustUpdate)
                    {
                        _lenp.HomePhoneCode = _view.PhoneCode.ToString();
                        _lenp.HomePhoneNumber = _view.PhoneNumber.ToString();

                        ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                        LER.SaveLegalEntity(_lenp, false);
                    }
                }

                if (existingLegalEntityKey != -1 || _view.ExistingLegalEntityKey != 0)
                {
                    if (_view.ExistingLegalEntityKey != 0)
                    {
                        UpdateLegalEntity(_view.ExistingLegalEntityKey);
                    }
                    else
                    {
                        if (existingLegalEntityKey != -1)
                        {
                            UpdateLegalEntity(existingLegalEntityKey);
                        }
                    }
                }               

                IList<ICacheObjectLifeTime> lifeTimes = new List<ICacheObjectLifeTime>();
                
                IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();                     

                if (_app == null || _app.Key == -1)
                {
                    IApplicationUnknown applicationUnknown = AR.GetEmptyUnknownApplicationType(OriginationSourceHelper.PrimaryOriginationSourceKey(_view.CurrentPrincipal));
                    // add the application role to the application
                    AddApplicationRoleLeadMainApplicant(applicationUnknown);

                    //Set the marketing source
                    if (_view.MarketingSource.Length > 0)
                    {
                        //applicationUnknown.ApplicationSource = _lookupRepo.ApplicationSources.ObjectDictionary[_view.MarketingSource];
                        applicationUnknown.ApplicationSource = _appRepo.GetApplicationSourceByKey(Convert.ToInt32(_view.MarketingSource));

                    }                  

                    //Set the number of applicants
                    applicationUnknown.EstimateNumberApplicants = _view.NumberOfApplicants;

                    ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
                    applicationUnknown.ApplicationStatus = LR.ApplicationStatuses.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.OfferStatuses.Open)];
                    
                    // save the application
                    //applicationUnknown.CalculateHouseHoldIncome();
                    AR.SaveApplication(applicationUnknown);

                    // once we have an application create a workflow case
                    Dictionary<string, string> Inputs = new Dictionary<string, string>();
                    Inputs.Add("ApplicationKey", applicationUnknown.Key.ToString());
                    GlobalCacheData.Add(ViewConstants.EstateAgentApplication, _view.IsEstateAgentApplication.ToString(), lifeTimes);
                    GlobalCacheData.Add(ViewConstants.OldMutualDeveloperLoan, _view.IsOldMutualDeveloperLoan.ToString(), lifeTimes);
                    Inputs.Add("isEstateAgentApplication", _view.IsEstateAgentApplication.ToString());
                    
                    IX2Info XI = X2Service.GetX2Info(_view.CurrentPrincipal);
                    if (XI == null || String.IsNullOrEmpty(XI.SessionID))
                        X2Service.LogIn(_view.CurrentPrincipal);

                    X2Service.CreateWorkFlowInstance(_view.CurrentPrincipal, SAHL.Common.Constants.WorkFlowProcessName.Origination, (-1).ToString(), SAHL.Common.Constants.WorkFlowName.ApplicationCapture, "Create Lead", Inputs, false);
                    if (!_view.IsValid)
                    {
                        X2Service.CancelActivity(_view.CurrentPrincipal);
                        throw new Exception();
                    }


                    X2Service.CreateCompleteActivity(_view.CurrentPrincipal, Inputs, false, null);

                    //get the instanceID
                    instanceID = XI.InstanceID;

                    if (GlobalCacheData.ContainsKey(ViewConstants.ApplicationKey))
                    {
                        GlobalCacheData.Remove(ViewConstants.ApplicationKey);
                    }
                    GlobalCacheData.Add(ViewConstants.ApplicationKey, applicationUnknown.Key, lifeTimes);

                    if (instanceID > 0)
                    {
                        // add the instanceID to the global cache for our redirect view to use
                        GlobalCacheData.Remove(ViewConstants.InstanceID);
                        GlobalCacheData.Add(ViewConstants.InstanceID, instanceID, new List<ICacheObjectLifeTime>());

                    }
                    if (_view.IsOldMutualDeveloperLoan)
                    {
                        IApplicationAttribute applicationAttributeNew = _appRepo.GetEmptyApplicationAttribute();
                        applicationAttributeNew.ApplicationAttributeType = _lookupRepo.ApplicationAttributesTypes.ObjectDictionary[((int)OfferAttributeTypes.OldMutualDeveloperLoan).ToString()];
                        applicationAttributeNew.Application = applicationUnknown;
                        applicationUnknown.ApplicationAttributes.Add(_view.Messages, applicationAttributeNew);
                    }
                }
                else
                {
                    //Set the number of applicants
                    _app.EstimateNumberApplicants = _view.NumberOfApplicants;

                    //Set the marketing source
                    //_app.ApplicationSource = _lookupRepo.ApplicationSources.ObjectDictionary[_view.MarketingSource];
                    _app.ApplicationSource = _appRepo.GetApplicationSourceByKey(Convert.ToInt32(_view.MarketingSource));

                    if (_view.IsOldMutualDeveloperLoan)
                    {
                        IApplicationAttribute applicationAttributeNew = _appRepo.GetEmptyApplicationAttribute();
                        applicationAttributeNew.ApplicationAttributeType = _lookupRepo.ApplicationAttributesTypes.ObjectDictionary[((int)OfferAttributeTypes.OldMutualDeveloperLoan).ToString()];
                        applicationAttributeNew.Application = _app;
                        _app.ApplicationAttributes.Add(_view.Messages, applicationAttributeNew);
                    }
                    // save the application
                    //applicationUnknown.CalculateHouseHoldIncome();
                    AR.SaveApplication(_app);

                }

                if (GlobalCacheData.ContainsKey(ViewConstants.SelectedLegalEntityKey))
                {
                    GlobalCacheData.Remove(ViewConstants.SelectedLegalEntityKey);
                }

                GlobalCacheData.Add(ViewConstants.SelectedLegalEntityKey, _lenp.Key, lifeTimes);

                txn.VoteCommit();

                if(GlobalCacheData.ContainsKey("SKIPCALCULATOR"))
                {
                    _view.Navigator.Navigate("Declarations");
                }
                else
                {
                    _view.Navigator.Navigate("Next");
                }
            }
            catch (Exception)
            {
                txn.VoteRollBack();
                if (_view.IsValid)
                    throw;
            }
            finally
            {
                txn.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        private void AddApplicationRoleLeadMainApplicant(IApplication application)
        {
            IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
            IApplicationRole applicationRole = application.AddRole((int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant, _lenp);            
            // add the 'income contributor' application role attribute

            IApplicationRoleAttribute applicationRoleAttribute = AR.GetEmptyApplicationRoleAttribute();
            applicationRoleAttribute.OfferRole = applicationRole;
            applicationRoleAttribute.OfferRoleAttributeType = AR.GetApplicationRoleAttributeTypeByKey((int)SAHL.Common.Globals.OfferRoleAttributeTypes.IncomeContributor);
            applicationRole.ApplicationRoleAttributes.Add(_view.Messages, applicationRoleAttribute);
            if (_skipCalculator)
            {
                AR.SaveApplication(application);
            }
            if (!_view.IsValid)
                throw new Exception();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="existingLegalEntityKey"></param>
        private void UpdateLegalEntity(int existingLegalEntityKey)
        {
            IApplicationRepository AR = RepositoryFactory.GetRepository<IApplicationRepository>();
            ILegalEntityRepository LER = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            ILegalEntityNaturalPerson le = null;
            if (_view.ExistingLegalEntityKey == 0 || existingLegalEntityKey != _view.ExistingLegalEntityKey)
            {
                le = LER.GetLegalEntityByKey(existingLegalEntityKey) as ILegalEntityNaturalPerson;
                if (_view.LEFirstNames.Length > 0 || existingLegalEntityKey == -1)
                {
                    le.FirstNames = _view.LEFirstNames;
                }
                if (_view.LESurname.Length > 0 || existingLegalEntityKey == -1)
                {
                    le.Surname = _view.LESurname;
                }

                if (_view.LEIDNumber.Length > 0)
                {
                    //Nazir J => 2008-07-14
                    //le.SetIDNumber(_view.LEIDNumber);
                    le.IDNumber = _view.LEIDNumber;
                }

                le.HomePhoneCode = _view.PhoneCode.ToString();
                le.HomePhoneNumber = _view.PhoneNumber.ToString();
                
                _view.BindExistingLegalEntityAndApplication(le, _app);
                
                LER.SaveLegalEntity(le, false);
            }
            else
            {
                if (_view.ExistingLegalEntityKey != 0 && existingLegalEntityKey == _view.ExistingLegalEntityKey)
                {
                    le = LER.GetLegalEntityByKey(_view.ExistingLegalEntityKey) as ILegalEntityNaturalPerson;
                    le.HomePhoneCode = _view.PhoneCode.ToString();
                    le.HomePhoneNumber = _view.PhoneNumber.ToString();

                    _view.BindExistingLegalEntityAndApplication(le, _app);

                    LER.SaveLegalEntity(le, false);
                }
              
                int[] roles = new int[1] { (int)SAHL.Common.Globals.OfferRoleTypes.LeadMainApplicant };
                if (_app != null)
                {
                    _app.RemoveRolesForLegalEntity(_view.Messages, le, roles);
                    AR.SaveApplication(_app);
                    AddApplicationRoleLeadMainApplicant(_app);
                }
                
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _view_OnCancelButtonClicked(object sender, EventArgs e)
        {
            _view.Navigator.Navigate("Cancel");
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CreateNewLegalEntity()
        {
            // save the legal entity details and create an application
            // Create a blank LE populate it and save it
            _leRepo = RepositoryFactory.GetRepository<ILegalEntityRepository>();
            ICommonRepository commRepo = RepositoryFactory.GetRepository<ICommonRepository>();

            _lenp = (ILegalEntityNaturalPerson)_leRepo.GetEmptyLegalEntity(LegalEntityTypes.NaturalPerson);
            _lenp.IntroductionDate = DateTime.Now;

            _lenp.FirstNames = _view.LEFirstNames;
            _lenp.Surname = _view.LESurname;
            ILookupRepository LR = RepositoryFactory.GetRepository<ILookupRepository>();
            _lenp.LegalEntityStatus = LR.LegalEntityStatuses.ObjectDictionary[Convert.ToString((int)SAHL.Common.Globals.LegalEntityStatuses.Alive)];
            if (_view.LEIDNumber.Length > 0)
            {
                //Nazir J => 2008-07-14
                //_lenp.SetIDNumber(_view.LEIDNumber);
                _lenp.IDNumber = _view.LEIDNumber;
            }

            if (_view.PhoneCode.Length > 0 && _view.PhoneNumber.Length > 0)
            {
                _lenp.HomePhoneCode = _view.PhoneCode.ToString();
                _lenp.HomePhoneNumber = _view.PhoneNumber.ToString();
            }
            _lenp.DocumentLanguage = commRepo.GetLanguageByKey((int)SAHL.Common.Globals.Languages.English);
            _leRepo.SaveLegalEntity(_lenp, false);
            if (_skipCalculator || (_app != null && _app.ApplicationType.Key == (int)SAHL.Common.Globals.OfferTypes.Unknown))
            {
                AddApplicationRoleLeadMainApplicant(_app);              
            }
        }       
    }
}
