using System.Collections.Generic;
using SAHL.Core.Data.Models.X2;
using SAHL.Core.Services;

namespace SAHL.X2Engine2.Commands
{
    public class CreateWorkListCommand : ServiceCommand
    {
        public List<WorkListDataModel> WorkListDataModels { get; protected set; }

        public CreateWorkListCommand(WorkListDataModel workListDataModel)
        {
            this.WorkListDataModels = new List<WorkListDataModel> { workListDataModel };
        }

        public CreateWorkListCommand(List<WorkListDataModel> worklistDataModels)
        {
            this.WorkListDataModels = worklistDataModels;
        }
    }
}