using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using System;
using System.Text;

namespace SAHL.Services.WorkflowTask.Server.Statements
{
    public class UpdateUserTagWithGivenValuesStatement : ISqlStatement<UserTagsDataModel>
    {
        public Guid Id { get; private set; }

        public string Caption { get; private set; }

        public string BackColour { get; private set; }

        public string ForeColour { get; private set; }

        public UpdateUserTagWithGivenValuesStatement(Guid id, string tagText, string backColour, string foreColour)
        {
            Id = id;
            Caption = tagText;
            BackColour = backColour;
            ForeColour = foreColour;
        }

        public string GetStatement()
        {
            return
                "UPDATE [2am].[tag].[UserTags] SET Caption = @Caption ,  BackColour = @BackColour ,  ForeColour = @ForeColour  WHERE Id = @Id";
        }
    }
}