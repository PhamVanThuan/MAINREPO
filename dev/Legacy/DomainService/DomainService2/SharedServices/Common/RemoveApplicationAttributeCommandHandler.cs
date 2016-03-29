using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.SharedServices.Common
{
    public class RemoveApplicationAttributeCommandHandler : IHandlesDomainServiceCommand<RemoveApplicationAttributeCommand>
    {
        private IApplicationRepository applicationRepository;

        public RemoveApplicationAttributeCommandHandler(IApplicationRepository applicationRepository)
        {
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, RemoveApplicationAttributeCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            if (application != null)
            {

                var applicationAttribute = application.ApplicationAttributes
                    .Where(x => x.ApplicationAttributeType.Key == command.ApplicationAttributeTypeKey)
                    .FirstOrDefault();

                if (applicationAttribute != null)
                {
                    application.ApplicationAttributes.Remove(messages, applicationAttribute);
                    applicationRepository.SaveApplication(application);
                }
            }
        }

    }
}
