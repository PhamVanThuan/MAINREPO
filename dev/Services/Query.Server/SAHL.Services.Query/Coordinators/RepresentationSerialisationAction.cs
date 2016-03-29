using Newtonsoft.Json.Linq;
using SAHL.Services.Interfaces.Query.Models;

namespace SAHL.Services.Query.Coordinators
{
    public class RepresentationSerialisationAction
    {
        public IQueryDataModel DataModel { get; private set; }
        public JToken SerialisationResult { get; set; }

        public RepresentationSerialisationAction(IQueryDataModel dataModel)
        {
            this.DataModel = dataModel;
        }
    }
}