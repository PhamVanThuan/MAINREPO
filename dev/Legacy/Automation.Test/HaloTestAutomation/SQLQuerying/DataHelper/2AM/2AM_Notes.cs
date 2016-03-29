using Common.Constants;
using Common.Enums;
using System;

namespace Automation.DataAccess.DataHelper
{
    public partial class _2AMDataHelper
    {
        /// <summary>
        /// Get all the notes saved against the debtcounselling case.
        /// </summary>
        /// <param name="debtcounsellingkey"></param>
        /// <returns></returns>
        public QueryResults GetNotes(GenericKeyTypeEnum genericKeyType, params int[] genericKey)
        {
            string genericKeys
                = Helpers.GetDelimitedString<int>(genericKey, ",");
            string query = string.Format(@"select * from dbo.Note as n with (nolock)
	                                        inner join dbo.NoteDetail as nd with (nolock)
		                                        on n.NoteKey = nd.NoteKey
	                                        inner join dbo.aduser as ad
		                                        on nd.legalentitykey = ad.legalentitykey
											where n.GenericKeyTypeKey = {0} and  n.Generickey in ({1})
                                            order by n.notekey desc", (int)genericKeyType, genericKeys);
            var statement = new SQLStatement { StatementString = query };
            return dataContext.ExecuteSQLQuery(statement);
        }

        public void InsertNote(int genericKey, GenericKeyTypeEnum genericKeyType, string workflowState, string noteText, int legalEntityKey, DateTime diaryDate, string tag)
        {
            var query
                = String.Format(@"insert into dbo.note (generickeytypekey,generickey,diarydate)
                                      values({0},{1},'{2}')

                                      declare @noteKey int
                                      set @noteKey = (select notekey from dbo.note where generickey = {1})

                                      insert into dbo.notedetail (notekey,tag,workflowstate,inserteddate,notetext,legalentitykey)
                                      values(@noteKey,'{3}','{4}','{5}','{6}','{7}')", (int)genericKeyType, genericKey, diaryDate.ToString(Formats.DateTimeFormatSQL),
                                                                               tag, workflowState, DateTime.Now.ToString(Formats.DateTimeFormatSQL),
                                                                                                                                    noteText, legalEntityKey);
            dataContext.Execute(query);
        }

        public void DeleteNotes(int genericKey)
        {
            var query = String.Format(@" delete from dbo.notedetail
                                     where notekey in (select notekey from dbo.note where generickey = {0})

								     delete from dbo.note
								     where generickey = {0}", genericKey);
            dataContext.Execute(query);
        }
    }
}