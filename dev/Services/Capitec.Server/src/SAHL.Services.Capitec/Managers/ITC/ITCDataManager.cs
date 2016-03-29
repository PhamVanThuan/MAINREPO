using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Services.Capitec.Managers.ITC.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.Capitec.Managers.ITC
{
    public class ITCDataManager : IITCDataManager
    {
        private IDbFactory dbFactory;

        public ITCDataManager(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public ITCDataModel GetItcById(Guid itcID)
        {
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.GetByKey<ITCDataModel, Guid>(itcID);
            }
        }

        public List<ITCDataModel> GetItcModelsForPerson(string identityNumber)
        {
            var getItcsStatement = new GetItcsForIdentityNumberStatement(identityNumber);
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.Select(getItcsStatement).ToList();
            }
        }

        public PersonITCDataModel GetPersonITC(Guid personID)
        {
            using (var db = dbFactory.NewDb().InReadOnlyAppContext())
            {
                return db.GetByKey<PersonITCDataModel, Guid>(personID);
            }
        }

        public void SavePersonItc(Guid personID, Guid itcID, DateTime itcDate)
        {
            var personITC = new PersonITCDataModel(personID, itcID, itcDate);
            using (var db = dbFactory.NewDb().InAppContext())
            {
                db.Insert(personITC);
                db.Complete();
            }
        }

        public void UpdatePersonItc(Guid personID, Guid itcID, DateTime itcDate)
        {
            var personITC = new PersonITCDataModel(personID, itcID, itcDate);
            using (var db = dbFactory.NewDb().InAppContext())
            {
                db.Update(personITC);
                db.Complete();
            }
        }

        public void SaveITC(Guid itcID, DateTime itcDate, string itcData)
        {
            var itc = new SAHL.Core.Data.Models.Capitec.ITCDataModel(itcID, itcDate, itcData);
            using (var dbContext = dbFactory.NewDb().InAppContext())
            {
                dbContext.Insert(itc);
                dbContext.Complete();
            }
        }

    }
}