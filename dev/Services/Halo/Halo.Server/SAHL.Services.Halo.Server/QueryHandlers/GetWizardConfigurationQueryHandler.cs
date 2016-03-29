using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using AutoMapper;

using SAHL.Core.Logging;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.UI.Halo.Shared.Repository;
using SAHL.UI.Halo.Shared.Configuration;
using SAHL.Services.Interfaces.Halo.Models;
using SAHL.Services.Interfaces.Halo.Queries;
using SAHL.Services.Interfaces.Halo.Models.Configuration;

namespace SAHL.Services.Halo.Server.QueryHandlers
{
    public class GetWizardConfigurationQueryHandler : QueryHandlerBase, IServiceQueryHandler<GetWizardConfigurationQuery>
    {
        private readonly ITileWizardConfigurationRepository tileWizardRepository;

        public GetWizardConfigurationQueryHandler(ITileWizardConfigurationRepository tileWizardRepository, 
                                                  IRawLogger rawLogger, ILoggerSource loggerSource, ILoggerAppSource loggerAppSource) 
            : base(rawLogger, loggerSource, loggerAppSource)
        {
            if (tileWizardRepository == null) { throw new ArgumentNullException("tileWizardRepository"); }
            this.tileWizardRepository = tileWizardRepository;
        }

        public ISystemMessageCollection HandleQuery(GetWizardConfigurationQuery query)
        {
            var messages = SystemMessageCollection.Empty();

            try
            {
                IHaloWizardTileConfiguration wizardConfiguration = null;

                if (!string.IsNullOrWhiteSpace(query.WizardName))
                {
                    wizardConfiguration = this.tileWizardRepository.FindWizardConfiguration(query.WizardName);
                    if (wizardConfiguration == null)
                    {
                        throw new Exception(string.Format("Unable to find the Wizard Configuration named {0}", query.WizardName));
                    }
                }
                else
                {
                    wizardConfiguration = this.tileWizardRepository.FindWizardWorkflowConfiguration(query.ProcessName, query.WorkflowName, query.ActivityName);
                    if (wizardConfiguration == null)
                    {
                        throw new Exception(string.Format("Unable to find the Wizard Workflow Configuration [{0}:{1}:{2}]", 
                                                          query.ProcessName, query.WorkflowName, query.ActivityName));
                    }
                }

                var haloWizardModel = this.CreateWizardConfigurationModel(wizardConfiguration);

                var queryResult = new WizardConfigurationQueryResult
                    {
                        WizardConfiguration = haloWizardModel,
                    };
                query.Result = new ServiceQueryResult<WizardConfigurationQueryResult>(new List<WizardConfigurationQueryResult>() { queryResult });
            }
            catch (Exception runtimeException)
            {
                messages.AddMessage(new SystemMessage(string.Format("Failed to load the Wizard Configurations\n{0}", runtimeException),
                                                      SystemMessageSeverityEnum.Exception));
            }

            return messages;
        }

        private HaloWizardModel CreateWizardConfigurationModel(IHaloWizardTileConfiguration wizardConfiguration)
        {
            var wizardModel     = new HaloWizardModel();
            var haloWizardModel = Mapper.Map<IHaloWizardTileConfiguration, HaloWizardModel>(wizardConfiguration, wizardModel);
            return haloWizardModel;
        }
    }
}
