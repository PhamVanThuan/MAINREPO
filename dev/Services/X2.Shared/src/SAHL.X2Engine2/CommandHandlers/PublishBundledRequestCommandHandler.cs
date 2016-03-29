using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Communication;
using SAHL.X2Engine2.Factories;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.X2Engine2.CommandHandlers
{
    public class PublishBundledRequestCommandHandler : IServiceCommandHandler<PublishBundledRequestCommand>
    {
        private IX2ServiceCommandRouter commandHandler;
        private IRequestFactory requestFactory;
        private IX2RequestInterrogator interrogator;
        private IX2RequestPublisher requestPublisher;
        private IX2QueueNameBuilder x2QueueNameBuilder;

        public PublishBundledRequestCommandHandler(IX2ServiceCommandRouter commandHandler, IRequestFactory requestFactory, IX2RequestPublisher requestPublisher, IX2RequestInterrogator interrogator, IX2QueueNameBuilder x2QueueNameBuilder)
        {
            this.commandHandler = commandHandler;
            this.requestFactory = requestFactory;
            this.requestPublisher = requestPublisher;
            this.interrogator = interrogator;
            this.x2QueueNameBuilder = x2QueueNameBuilder;
        }

        public ISystemMessageCollection HandleCommand(PublishBundledRequestCommand command, IServiceRequestMetadata metadata)
        {
            var messages = new SystemMessageCollection();
            
            if (command.Commands.Any())
            {
                var requests = new List<IX2Request>();
                foreach (dynamic cmd in command.Commands)
                {
                    var request = this.requestFactory.CreateRequest(cmd);
                    requests.Add(request);
                }

                var workflow = interrogator.GetRequestWorkflow(requests.First());
                var systemQueue = this.x2QueueNameBuilder.GetSystemQueue(workflow);

                var bundledRequest = new X2BundledRequest(requests);
                requestPublisher.Publish<X2BundledRequest>(systemQueue, bundledRequest);
            }
            return messages;
        }
    }
}