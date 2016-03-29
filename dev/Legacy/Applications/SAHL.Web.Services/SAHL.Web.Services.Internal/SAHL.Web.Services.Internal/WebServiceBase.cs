using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.CacheData;
using System.Security.Principal;
using SAHL.Web.Services.Internal.DataModel;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Globals;
using System.Threading;
using SAHL.Common.Security;
using SAHL.Common.Logging;
using System.Diagnostics;

namespace SAHL.Web.Services.Internal
{
    public abstract class WebServiceBase
    {
        protected void SetupPrincipal(string leKey)
        {
            SAHLPrincipal principal = new SAHLPrincipal(new GenericIdentity(leKey));
            Thread.CurrentPrincipal = principal;
        }

        protected SAHLPrincipalCache SPC
        {
            get
            {
                return SAHLPrincipalCache.GetPrincipalCache(SAHLPrincipal.GetCurrent());
            }
        }

        protected bool DomainValidationFailed
        {
            get
            {
                if (SPC.DomainMessages.HasErrorMessages || SPC.DomainMessages.HasWarningMessages)
                    return true;
                else
                    return false;
            }
        }

        protected void HandleDomainMessages(ServiceMessage validateRequest)
        {
            foreach (IDomainMessage dm in SPC.DomainMessages)
            {
                validateRequest.ServiceMessages.Add(new Message(ServiceMessageType.Warning, dm.Message));
            }
        }

        protected void LogException(string methodName, string message, Dictionary<string, object> parameters, System.Exception exception)
        {
            AddCachedMessagesToParameters(parameters);
            Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(parameters));
            LogPlugin.Logger.LogErrorMessageWithException(
                                      methodName
                                    , message
                                    , exception
                                    , parameters
                                );
        }

        private void AddCachedMessagesToParameters(Dictionary<string, object> parameters)
        {
            if (SPC.DomainMessages.HasErrorMessages)
            {
                foreach (var message in SPC.DomainMessages.ErrorMessages)
                {
                    parameters.Add(string.Format("{0}_{1}",message.MessageType.ToString(),Guid.NewGuid()), message.Message);
                }
            }

            if (SPC.DomainMessages.HasWarningMessages)
            {
                foreach (var message in SPC.DomainMessages.WarningMessages)
                {
                    parameters.Add(string.Format("{0}_{1}", message.MessageType.ToString(), Guid.NewGuid()), message.Message);
                }
            }
        }

        protected const string _validationLoginMessage = "Error! Contact SA Home Loans";
        protected const string _validationMethodMessage = "Error! Service Request Can not be completed.";

        private static IMessageService _messageService;
        private IAccountRepository _accountRepository;
        private IDebtCounsellingRepository _debtCounsellingRepository;
        private ILegalEntityRepository _legalEntityRepository;
        private ILookupRepository _lookupRepository;
        private IPropertyRepository _propertyRepository;
        private IX2Repository _x2Repository;
        private IOrganisationStructureRepository _organisationStructureRepository;
        private ICommonRepository _commonRepository;
        private IApplicationRepository _applicationRepository;

        protected static IMessageService messageService
        {
            get
            {
                if (_messageService == null)
                {
                    _messageService = ServiceFactory.GetService<IMessageService>();
                }
                return _messageService;
            }
        }

        protected IApplicationRepository applicationRepository
        {
            get
            {
                if (_applicationRepository == null)
                {
                    _applicationRepository = RepositoryFactory.GetRepository<IApplicationRepository>();
                }
                return _applicationRepository;
            }
        }


        protected IAccountRepository accountRepository
        {
            get
            {
                if (_accountRepository == null)
                {
                    _accountRepository = RepositoryFactory.GetRepository<IAccountRepository>();
                }
                return _accountRepository;
            }
        }

        protected IDebtCounsellingRepository debtCounsellingRepository
        {
            get
            {
                if (_debtCounsellingRepository == null)
                {
                    _debtCounsellingRepository = RepositoryFactory.GetRepository<IDebtCounsellingRepository>();
                }
                return _debtCounsellingRepository;
            }
        }

        protected ILegalEntityRepository legalEntityRepository
        {
            get
            {
                if (_legalEntityRepository == null)
                {
                    _legalEntityRepository = RepositoryFactory.GetRepository<ILegalEntityRepository>();
                }
                return _legalEntityRepository;
            }
        }

        protected ILookupRepository lookupRepository
        {
            get
            {
                if (_lookupRepository == null)
                {
                    _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();
                }
                return _lookupRepository;
            }
        }

        protected IPropertyRepository propertyRepository
        {
            get
            {
                if (_propertyRepository == null)
                {
                    _propertyRepository = RepositoryFactory.GetRepository<IPropertyRepository>();
                }
                return _propertyRepository;
            }
        }

        protected IX2Repository x2Repository
        {
            get
            {
                if (_x2Repository == null)
                {
                    _x2Repository = RepositoryFactory.GetRepository<IX2Repository>();
                }
                return _x2Repository;
            }
        }

        protected IOrganisationStructureRepository organisationStructureRepository
        {
            get
            {
                if (_organisationStructureRepository == null)
                {
                    _organisationStructureRepository = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
                }
                return _organisationStructureRepository;
            }
        }

        protected ICommonRepository commonRepository
        {
            get
            {
                if (_commonRepository == null)
                {
                    _commonRepository = RepositoryFactory.GetRepository<ICommonRepository>();
                }
                return _commonRepository;
            }
        }
    }
}