using Automation.DataAccess;

using Common.Enums;
using System.Collections.Generic;

namespace BuildingBlocks.Services.Contracts
{
    public interface IEWorkService
    {
        void UpdateEworkStage(string newStage, string currentStage, string currentMap, string folderName);

        int GetPipelineCaseWhereActionNotApplied(string eWorkAction, string x2State, int isFL);

        int GetPipelineCaseInEworkStage(string x2State, string eworkStage, int isFL);

        QueryResults GetPipelineCaseWhereActionIsApplied(string eworkAction, string x2State, int isFL);

        void WaitForeWorkCaseToCreate(int accountKey, string eStage, string eMap);

        string GetEworkStage(int accountKey);

        string GetEworkUser(int accountKey);

        void UpdateEworkAssignedUser(int accountKey, string lclcUser, string eStageName);

        string GeteFolderIdForCaseInLossControl(int accountKey);

        void WaitForEworkEvent(string eFolderId, string eActionName, string eventTime, int seconds);

        Automation.DataModels.TokenAssignment GetNextUserForRoundRobinAssignmentInLossControl(string tokenAssignmenteMapName);

        Automation.DataModels.eWorkCase GetEWorkCase(string eMapName, List<string> eWorkStages, int accountkey = 0, OriginationSourceEnum origSource = OriginationSourceEnum.None,
                DetailTypeEnum detailTypeExclusion = DetailTypeEnum.None, OriginationSourceEnum OriginationSourceKey = OriginationSourceEnum.None,
                string idnumber = "", bool IsSubsidised = false);

        void WaitForEWorkStage(string efoldername, string expected_eStageName, string eMapName, int noTries);

        Automation.DataModels.eWorkCase GetEWorkCase(string eMapName, string eWorkStage, int accountkey = 0, OriginationSourceEnum origSource = OriginationSourceEnum.None, DetailTypeEnum detailTypeExclusion = DetailTypeEnum.None,
            OriginationSourceEnum OriginationSourceKey = OriginationSourceEnum.None, string idnumber = "", bool IsSubsidised = false, ProductEnum product = ProductEnum.None);

        QueryResults GetEFolderID(string eFolderName, string eStageName, string eMapName);

        QueryResults GetEFolderID(string eFolderName, string eStageName);

        QueryResults GetEWorkEFolderAssignmentData(string eFolderName, string eMapName, string eStageName);
    }
}