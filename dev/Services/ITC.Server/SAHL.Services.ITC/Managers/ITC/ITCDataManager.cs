using System;
using System.Collections.Generic;
using SAHL.Core.Data;
using SAHL.Services.ITC.Managers.Itc.Statements;

namespace SAHL.Services.ITC.Managers.Itc
{
    public class ItcDataManager : IItcDataManager
    {
        private IDbFactory dbFactory;

        public ItcDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public SAHL.Core.Data.Models._2AM.ITCRequestDataModel GetITCByID(Guid itcID)
        {
            using (var db = this.dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.GetByKey<SAHL.Core.Data.Models._2AM.ITCRequestDataModel, Guid>(itcID);
            }
        }

        public IEnumerable<Core.Data.Models._2AM.ITCDataModel> GetItcsForLegalEntity(string identityNumber)
        {
            var sql = new GetItcsForLegalEntityStatement(identityNumber);
            using (var dbContext = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return dbContext.Select(sql);
            }
        }

        public void SaveITC(Guid itcID, DateTime itcDate, string itcData)
        {
            var itc = new SAHL.Core.Data.Models._2AM.ITCRequestDataModel(itcID, itcDate, itcData);
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(itc);
                dbContext.Complete();
            }
        }

        public void SaveITC(int clientKey, int? accountKey, DateTime itcDate, string itcRequestXML, string itcResponseXML, string responseStatus, string userId)
        {
            var itc = new SAHL.Core.Data.Models._2AM.ITCDataModel(clientKey, accountKey, itcDate, itcResponseXML, responseStatus, userId, itcRequestXML);
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(itc);
                dbContext.Complete();
            }
        }
    }
}