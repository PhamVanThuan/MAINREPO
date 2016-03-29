using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Base;
using Castle.ActiveRecord.Queries;
using Castle.ActiveRecord;
namespace SAHL.Common.BusinessModel
{
    public partial class Province : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Province_DAO>, IProvince
	{
		protected void OnCities_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		protected void OnCities_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}

        protected void OnSuburbs_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnSuburbs_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnCities_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnCities_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnSuburbs_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }

        protected void OnSuburbs_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

        public IReadOnlyEventList<ISuburb> SuburbsByPrefix(string prefix, int maxRowCount)
        {
            if (this.Key < 1)
                return new ReadOnlyEventList<ISuburb>();

            SimpleQuery q = new SimpleQuery(typeof(Suburb_DAO), @"
                from Suburb_DAO s 
                where s.City.Province.Key = ?
                and s.Description LIKE ?
                ",
                this.Key,
                prefix + "%"
            );
            q.SetQueryRange(maxRowCount);
            List<Suburb_DAO> lstDao = new List<Suburb_DAO>((Suburb_DAO[])ActiveRecordBase.ExecuteQuery(q));
            DAOEventList<Suburb_DAO, ISuburb, Suburb> suburbs = new DAOEventList<Suburb_DAO, ISuburb, Suburb>(lstDao);
            return new ReadOnlyEventList<ISuburb>(suburbs);
        }
	}
}


