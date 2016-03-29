using Common.Constants;
using Common.Enums;
using System;
using System.Collections.Generic;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Returns the court details captured
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <param name="courtName">courtName</param>
        /// <param name="caseNumber">caseNumber</param>
        /// <param name="hearingDate">hearingDate</param>
        /// <param name="hearingType">hearingType</param>
        /// <param name="appearanceType">appearanceType</param>
        /// <returns></returns>
        public QueryResults GetCourtDetails(int debtCounsellingKey, Automation.DataModels.CourtDetails courtDetails)
        {
            string query =
                string.Format(@"select * from [2am].debtcounselling.HearingDetail hd with (nolock)
								join [2am].debtcounselling.HearingType ht with (nolock)
								on hd.HearingTypeKey=ht.HearingTypeKey
								join [2am].debtcounselling.HearingAppearanceType hat with (nolock)
								on hd.HearingAppearanceTypeKey=hat.HearingAppearanceTypeKey
								join [2am].debtcounselling.court c with (nolock)
								on hd.courtKey=c.courtKey
								where hd.debtcounsellingkey={0}
								and c.Name='{1}'
								and hd.caseNumber = '{2}'
								and convert(varchar(10),hearingdate,103) = '{3}'
								and ht.description = '{4}'
								and hat.description = '{5}'
								and hd.Comment='{6}'", debtCounsellingKey, courtDetails.court, courtDetails.caseNumber,
                                                            courtDetails.hearingDate, courtDetails.hearingType, courtDetails.appearanceType,
                                                            courtDetails.comments);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Returns the tribunal details captured
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        /// <param name="caseNumber">caseNumber</param>
        /// <param name="hearingDate">hearingDate</param>
        /// <param name="hearingType">hearingType</param>
        /// <param name="appearanceType">appearanceType</param>
        /// <returns></returns>
        public QueryResults GetTribunalDetails(int debtCounsellingKey, Automation.DataModels.CourtDetails courtDetails)
        {
            string query =
                string.Format(@"select * from [2am].debtcounselling.HearingDetail hd with (nolock)
								join [2am].debtcounselling.HearingType ht with (nolock)
								on hd.HearingTypeKey=ht.HearingTypeKey
								join [2am].debtcounselling.HearingAppearanceType hat with (nolock)
								on hd.HearingAppearanceTypeKey=hat.HearingAppearanceTypeKey
								where hd.debtcounsellingkey={0}
								and hd.caseNumber = '{1}'
								and convert(varchar(10),hearingdate,103) = '{2}'
								and ht.description = '{3}'
								and hat.description = '{4}'
								and hd.comment='{5}'", debtCounsellingKey, courtDetails.caseNumber, courtDetails.hearingDate, courtDetails.hearingType,
                                                            courtDetails.appearanceType, courtDetails.comments);
            SQLStatement statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        /// <summary>
        /// Deletes any records in the debtCounselling.HearingDetail table for the debt counselling case.
        /// </summary>
        /// <param name="debtCounsellingKey">debtCounsellingKey</param>
        public void DeleteCourtDetails(int debtCounsellingKey)
        {
            string q =
                string.Format(@"Delete from [2am].debtCounselling.HearingDetail where debtCounsellingKey={0}", debtCounsellingKey);
            SQLStatement statement = new SQLStatement { StatementString = q };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        /// <summary>
        /// Inserts court details against a debt counselling case
        /// </summary>
        /// <param name="debtCounsellingKey">DebtCounsellingKey</param>
        /// <param name="hearingType"></param>
        /// <param name="hearingAppearanceType"></param>
        public void InsertCourtDetails(int debtCounsellingKey, HearingTypeEnum hearingType, HearingAppearanceTypeEnum hearingAppearanceType,
            DateTime hearingDate, string comment)
        {
            string query = string.Empty;
            switch (hearingType)
            {
                case HearingTypeEnum.Court:
                    query =
                        string.Format(@"insert into debtcounselling.hearingDetail
										(debtCounsellingKey, hearingTypeKey, hearingAppearanceTypeKey, CourtKey,
										CaseNumber, HearingDate, GeneralStatusKey, Comment)
										values
										({0}, {1}, {2}, 1, 'test/test/test', '{3}', 1, '{4}')", debtCounsellingKey,
                                                                                     (int)hearingType,
                                                                                     (int)hearingAppearanceType, hearingDate.ToString(Formats.DateTimeFormatSQL),
                                                                                     comment);
                    break;

                case HearingTypeEnum.Tribunal:
                    query =
                        string.Format(@"insert into debtcounselling.hearingDetail
										(debtCounsellingKey, hearingTypeKey, hearingAppearanceTypeKey,
										CaseNumber, HearingDate, GeneralStatusKey, Comment)
										values
										({0}, {1}, {2},'test/123/456', '{3}', 1, '{4}')", debtCounsellingKey,
                                                                               (int)hearingType,
                                                                               (int)hearingAppearanceType, hearingDate.ToString(Formats.DateTimeFormatSQL),
                                                                               comment);
                    break;

                default:
                    break;
            }
            var statement = new SQLStatement { StatementString = query };
            dataContext.ExecuteNonSQLQuery(statement);
        }

        public IEnumerable<Automation.DataModels.HearingDetails> GetHearingDetails(int dcKey)
        {
            var hearingDetail =
                dataContext.Query<Automation.DataModels.HearingDetails>(string.Format(@"select * from [2am].debtcounselling.HearingDetail where debtCounsellingKey={0}", dcKey));
            return hearingDetail;
        }
    }
}