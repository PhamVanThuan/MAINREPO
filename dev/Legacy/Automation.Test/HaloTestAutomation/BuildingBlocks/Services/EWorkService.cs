using Automation.DataAccess.DataHelper;
using BuildingBlocks.Services.Contracts;
using Common.Constants;
using Common.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WatiN.Core.Exceptions;
using WatiN.Core.Logging;
using WatiN.Core.UtilityClasses;

namespace BuildingBlocks.Services
{
    public class EWorkService : EWorkDataHelper, IEWorkService
    {
        /// <summary>
        /// Waits for 90 seconds for an eWork case to be found linked to the account in the map and stage provided.
        /// </summary>
        /// <param name="accountKey">Account Number</param>
        /// <param name="eMap">eWork Map</param>
        /// <param name="eStage">eWork Stage</param>
        public void WaitForeWorkCaseToCreate(int accountKey, string eStage, string eMap)
        {
            var timer = new SimpleTimer(TimeSpan.FromSeconds(90));
            bool b = false;
            while (!timer.Elapsed)
            {
                var r = base.GeteFolderCase(accountKey, eStage, eMap);
                if (r != null)
                {
                    b = true;
                    break;
                }
            }
            if (!b)
            {
                throw new WatiNException("No Pipeline case created after waiting 30 seconds.");
            }
        }

        /// Gets the expected Ework Stage
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public string GetEworkStage(int accountKey)
        {
            var results = base.GetLossControlByeFolder(accountKey.ToString());
            return results.Rows(0).Column("eStageName").GetValueAs<string>();
        }

        /// <summary>
        /// Gets the expected Ework User
        /// </summary>
        /// <param name="accountKey"></param>
        /// <returns></returns>
        public string GetEworkUser(int accountKey)
        {
            var results = base.GetLossControlByeFolder(accountKey.ToString());
            return results.Rows(0).Column("UserToDo").GetValueAs<string>();
        }

        /// <summary>
        /// Fetches the eFolderId for an ework case in the loss control map.
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        /// <returns>eFolderID</returns>
        public string GeteFolderIdForCaseInLossControl(int accountKey)
        {
            try
            {
                var r = base.GetLossControlByeFolder(accountKey.ToString());
                return r.Rows(0).Column("eFolderID").GetValueAs<string>();
            }
            catch (Exception ex)
            {
                throw new WatiNException(ex.ToString());
            }
        }

        /// <summary>
        /// This will wait the specified number of seconds for a given action to exist against an ework case in the eEvent table.
        /// </summary>
        /// <param name="eFolderId">eFolderId</param>
        /// <param name="eActionName">eActionName</param>
        /// <param name="eventTime">eventTime</param>
        /// <param name="seconds">seconds</param>
        public void WaitForEworkEvent(string eFolderId, string eActionName, string eventTime, int seconds)
        {
            var timer = new SimpleTimer(TimeSpan.FromSeconds(seconds));
            bool b = false;
            while (!timer.Elapsed)
            {
                Logger.LogAction(string.Format(@"eEvent search: eFolderID={0}, eActionName={1}, TimeStamp={2}", eFolderId, eActionName,
                    DateTime.Now.ToString(Formats.DateTimeFormatSQL)));
                var r = base.GetEworkEvent(eFolderId, eActionName, eventTime);
                if (r.HasResults)
                {
                    b = true;
                    break;
                }
            }
            if (!b)
                throw new WatiNException(string.Format(@"Ework event {0} did not get run for {1} after waiting 30 seconds", eActionName, eFolderId));
        }

        /// <summary>
        /// Get the next user for e-work round robin assignment according to TokenAssignment
        /// </summary>
        /// <returns></returns>
        public Automation.DataModels.TokenAssignment GetNextUserForRoundRobinAssignmentInLossControl(string tokenAssignmenteMapName)
        {
            var tokenAssignments = (from assignToken in base.GetTokenAssignments()
                                    where assignToken.eMapName == tokenAssignmenteMapName
                                    select assignToken).ToList();

            var currentTokenAssignment = (from assignToken in tokenAssignments
                                          where assignToken.eMapName == tokenAssignmenteMapName && assignToken.Token == 1
                                          select assignToken).FirstOrDefault();

            var tokenAssignmentByMaxSeq = (from assignToken in tokenAssignments
                                           orderby assignToken.SeqNbr descending
                                           select assignToken).FirstOrDefault();

            var tokenAssignmentByMinSeq = (from assignToken in tokenAssignments
                                           orderby assignToken.SeqNbr ascending
                                           select assignToken).FirstOrDefault();

            if (currentTokenAssignment.SeqNbr == tokenAssignmentByMaxSeq.SeqNbr)
            {
                return tokenAssignmentByMinSeq;
            }
            else
            {
                return (from assignToken in tokenAssignments
                        where assignToken.SeqNbr == (currentTokenAssignment.SeqNbr + 1)
                        orderby assignToken.SeqNbr ascending
                        select assignToken).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get a single ework record from the database based on the parameters
        /// </summary>
        /// <param name="eMapName"></param>
        /// <param name="accountkey"></param>
        /// <param name="origSource"></param>
        /// <param name="detailTypeExclusion"></param>
        /// <param name="OriginationSourceKey"></param>
        /// <param name="idnumber"></param>
        /// <param name="IsSubsidised"></param>
        /// <returns></returns>
        public Automation.DataModels.eWorkCase GetEWorkCase(string eMapName, List<string> eWorkStages, int accountkey = 0, OriginationSourceEnum origSource = OriginationSourceEnum.None,
                DetailTypeEnum detailTypeExclusion = DetailTypeEnum.None, OriginationSourceEnum OriginationSourceKey = OriginationSourceEnum.None,
                string idnumber = "", bool IsSubsidised = false)
        {
            IEnumerable<Automation.DataModels.eWorkCase> losscontrolcases = null;
            foreach (string stage in eWorkStages)
            {
                losscontrolcases = base.GetEWorkCases(eMapName, stage, accountkey);
                if (losscontrolcases.Any())
                {
                    break;
                }
            }
            if (!losscontrolcases.Any())
                return null;
            if (accountkey > 0)
                return losscontrolcases.FirstOrDefault();

            if (origSource != OriginationSourceEnum.None)
            {
                losscontrolcases = from lossControl in losscontrolcases
                                   where lossControl.OriginationSourceKey == origSource
                                   select lossControl;
            }
            if (IsSubsidised)
            {
                losscontrolcases = from lossControl in losscontrolcases
                                   where lossControl.IsSubsidised
                                   select lossControl;
            }
            else
            {
                losscontrolcases = from lossControl in losscontrolcases
                                   where lossControl.IsSubsidised == false
                                   select lossControl;
            }
            if (!String.IsNullOrEmpty(idnumber))
            {
                losscontrolcases = from lossControl in losscontrolcases
                                   where lossControl.IDNumber == idnumber
                                   select lossControl;
            }
            else
            {
                losscontrolcases = from lossControl in losscontrolcases
                                   where !string.IsNullOrEmpty(lossControl.IDNumber)
                                   select lossControl;
            }
            return losscontrolcases.FirstOrDefault();
        }

        /// <summary>
        /// This method will wait 60 seconds for the efolder to be updated
        /// </summary>
        /// <param name="efoldername"></param>
        /// <param name="expected_eStageName"></param>
        public void WaitForEWorkStage(string efoldername, string expected_eStageName, string eMapName, int noTries)
        {
            var tryCount = 0;
            var interval = TimeSpan.FromSeconds(10);
            while (tryCount < noTries)
            {
                SpinWait.SpinUntil(() => { return false; }, interval);
                var eWorkCase = GetEWorkCase(eMapName, expected_eStageName, accountkey: Int32.Parse(efoldername));
                if (eWorkCase != null && eWorkCase.StageName == expected_eStageName)
                    return;
                tryCount++;
            }
            //if we get here then it means that the folder was not updated or the case is not at the expected stage in eworks
            Assert.Fail(String.Format("Waited for Ework case to move (efoldername:{0}) in the {1} workflow to the {2} state, but exceeded the number of tries.",
                efoldername, eMapName, expected_eStageName));
        }

        ///<summary>
        /// Get a single ework record from the database based on the parameters
        /// </summary>
        /// <param name="eMapName"></param>
        /// <param name="eWorkStage"></param>
        /// <param name="accountkey"></param>
        /// <param name="origSource"></param>
        /// <param name="detailTypeExclusion"></param>
        /// <param name="OriginationSourceKey"></param>
        /// <param name="idnumber"></param>
        /// <param name="IsSubsidised"></param>
        /// <returns></returns>
        public Automation.DataModels.eWorkCase GetEWorkCase(string eMapName, string eWorkStage, int accountkey = 0, OriginationSourceEnum origSource = OriginationSourceEnum.None,
                DetailTypeEnum detailTypeExclusion = DetailTypeEnum.None, OriginationSourceEnum OriginationSourceKey = OriginationSourceEnum.None,
                string idnumber = "", bool IsSubsidised = false, ProductEnum product = ProductEnum.None)
        {
            var lossControlCases = base.GetEWorkCases(eMapName, eWorkStage, accountkey);
            if (!lossControlCases.Any())
                return null;
            if (accountkey > 0)
                return lossControlCases.FirstOrDefault();

            if (product != ProductEnum.None)
                lossControlCases = from lossControl in lossControlCases
                                   where lossControl.ProductKey == (int)product
                                   select lossControl;

            if (origSource != OriginationSourceEnum.None)
                lossControlCases = from lossControl in lossControlCases
                                   where lossControl.OriginationSourceKey == origSource
                                   select lossControl;
            if (IsSubsidised)
                lossControlCases = from lossControl in lossControlCases
                                   where lossControl.IsSubsidised
                                   select lossControl;
            else
                lossControlCases = from lossControl in lossControlCases
                                   where lossControl.IsSubsidised == false
                                   select lossControl;

            if (!String.IsNullOrEmpty(idnumber))
                lossControlCases = from lossControl in lossControlCases
                                   where lossControl.IDNumber == idnumber
                                   select lossControl;
            else
            {
                lossControlCases = from lossControl in lossControlCases
                                   where !string.IsNullOrEmpty(lossControl.IDNumber)
                                   select lossControl;
            }
            return lossControlCases.FirstOrDefault();
        }
    }
}