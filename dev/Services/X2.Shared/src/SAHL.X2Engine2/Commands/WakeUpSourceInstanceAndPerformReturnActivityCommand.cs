using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;
using SAHL.Core.X2;
using SAHL.Core.X2.Providers;
using SAHL.X2Engine2.ViewModels;

namespace SAHL.X2Engine2.Commands
{
    public class WakeUpSourceInstanceAndPerformReturnActivityCommand : ServiceCommand, IContinueWithCommands
    {
        public InstanceDataModel Instance { get; protected set; }

        public Activity Activity { get; protected set; }

        public IX2Map Map { get; protected set; }

        public IX2ContextualDataProvider ContextualDataProvider { get; protected set; }

        public string UserName { get; set; }

        public bool IgnoreWarnings { get; set; }

        public object Data { get; set; }

        public WakeUpSourceInstanceAndPerformReturnActivityCommand(InstanceDataModel instance, Activity activity, IX2Map map, IX2ContextualDataProvider contextualDataProvider)
        {
            this.Instance = instance;
            this.Activity = activity;
            this.Map = map;
            this.ContextualDataProvider = contextualDataProvider;
            this.UserName = "X2";
            this.IgnoreWarnings = true;
            this.Data = null;
        }
    }
}