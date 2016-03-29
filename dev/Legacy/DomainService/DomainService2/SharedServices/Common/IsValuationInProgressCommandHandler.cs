using System.Data;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DomainMessages;

namespace DomainService2.SharedServices.Common
{
    public class IsValuationInProgressCommandHandler : IHandlesDomainServiceCommand<IsValuationInProgressCommand>
    {
        ICastleTransactionsService castleTransactionsService;

        public IsValuationInProgressCommandHandler(ICastleTransactionsService castleTransactionsService)
        {
            this.castleTransactionsService = castleTransactionsService;
        }

        public void Handle(IDomainMessageCollection messages, IsValuationInProgressCommand command)
        {
            command.Result = false;
            string SQL = string.Format("select data.applicationkey, s.name, o.reservedaccountkey from x2data.Valuations Data (nolock) join x2.Instance i (nolock) on data.instanceid=i.id join x2.state s (nolock) on i.stateid=s.id join [2am]..offer o (nolock) on data.applicationkey=o.offerkey where data.Applicationkey={0} and s.type not in (5, 6)", command.GenericKey);
            DataSet ds = castleTransactionsService.ExecuteQueryOnCastleTran(SQL, SAHL.Common.Globals.Databases.X2, null);
            if (ds != null)
            {
                if (ds.Tables.Count != 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        command.Result = true;
                        messages.Add(new Error(string.Format("Application:{0} ReservedAccountKey:{2} in Valuations is not complete. Is at State:{1}", dr[0], dr[1], dr[2]), ""));
                    }
                }
            }
        }
    }
}