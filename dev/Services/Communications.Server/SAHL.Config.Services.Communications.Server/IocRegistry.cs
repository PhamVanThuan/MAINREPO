using ActionMailerNext.Standalone;
using SAHL.Core.IoC;
using SAHL.Services.Communications;
using SAHL.Services.Communications.ExternalServices.Notification;
using SAHL.Services.Communications.Managers.Email;
using SAHL.Services.Communications.Managers.LiveReplies;
using SAHL.Services.Interfaces.Communications.ExternalServices.Notification;
using StructureMap.Configuration.DSL;
using System.Collections.Specialized;
using System.Configuration;

namespace SAHL.Config.Services.Communications.Server
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            For<ILiveRepliesManager>().Singleton().Use<LiveRepliesManager>();
            For<IStartable>().Use(x => x.GetInstance<ILiveRepliesManager>());
            For<IStoppable>().Use(x => x.GetInstance<ILiveRepliesManager>());
            For<NameValueCollection>().Use(ConfigurationManager.AppSettings);
            For<ICommunicationSettings>().Use<CommunicationSettings>();
            For<IEmailManager>().Use<EmailManager>();
            For<ISMSNotificationServiceConfiguration>().Singleton().Use<SMSNotificationServiceConfiguration>();
            For<INotificationService>().Singleton().Use<SMSNotificationService>();
        }
    }
}