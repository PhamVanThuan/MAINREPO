using Castle.ActiveRecord;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Attributes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord.Queries;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.Factories;
using SAHL.Common.Exceptions;
using System;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Repositories
{
    [FactoryType(typeof(IBondRepository))]
    public class BondRepository : AbstractRepositoryBase, IBondRepository
    {
		public IBond GetBondByKey(int Key)
		{
			return base.GetByKey<IBond, Bond_DAO>(Key);

			//Bond_DAO dao = ActiveRecordBase<Bond_DAO>.Find(Key);

			//if (dao != null)
			//{
			//    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
			//    return BMTM.GetMappedType<IBond, Bond_DAO>(dao);

			//}
			//return null;
		}

		
		/// <summary>
        /// Implements <see cref="IBondRepository.SaveBond"></see>.
        /// </summary>
        public void SaveBond( IBond Bond)
        {
			base.Save<IBond, Bond_DAO>(Bond);
        }

        public IReadOnlyEventList<IBond> GetBondByRegistrationNumber(string bondRegistrationNumber)
        {
            if (String.IsNullOrEmpty(bondRegistrationNumber))
                return null;

            //Bond_DAO[] res = Bond_DAO.FindAllByProperty("BondRegistrationNumber", bondRegistrationNumber);
            const string HQL = "select bond from Bond_DAO bond where bond.BondRegistrationNumber = ?";
            SimpleQuery<Bond_DAO> q = new SimpleQuery<Bond_DAO>(HQL, bondRegistrationNumber);
            Bond_DAO[] bonds = q.Execute();

            IEventList<IBond> list = new DAOEventList<Bond_DAO, IBond, Bond>(bonds);
            return new ReadOnlyEventList<IBond>(list);               
        }

        // Commented as its not referenced anywhere and doesnt work anyway
        //public IBond GetBondByRegistrationNumberAndDeedsOfficeKey(string bondRegistrationNumber,int deedsOfficeKey)
        //{
        //    if (String.IsNullOrEmpty(bondRegistrationNumber)|| deedsOfficeKey < 1)
        //        return null;
            
        //    //Bond_DAO[] res = Bond_DAO.FindAllByProperty("BondRegistrationNumber", bondRegistrationNumber);
        //    const string HQL = "select bond from Bond_DAO bond where bond.BondRegistrationNumber = ? and bond.DeedsOffice.Key = ?";
        //    SimpleQuery<Bond_DAO> q = new SimpleQuery<Bond_DAO>(HQL, bondRegistrationNumber,deedsOfficeKey);
        //    Bond_DAO[] bonds = q.Execute();
            
        //    if (bonds.Length > 1)
        //        throw new Exception("There are multiple records with the same BondRegistrationNumber and DeedsOfficeKey");

        //    if (bonds != null && bonds.Length > 0)
        //    {
        //        IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
        //        return BMTM.GetMappedType<IBond, Bond_DAO>(bonds[0]);
        //    }
        //    return null;
        //}

        public IBond GetBondByApplicationKey(int ApplicationKey)
        {          
            //Bond_DAO[] res = Bond_DAO.FindAllByProperty("BondRegistrationNumber", bondRegistrationNumber);
            const string HQL = "select bond from Bond_DAO bond where bond.Application.Key = ?";
            SimpleQuery<Bond_DAO> q = new SimpleQuery<Bond_DAO>(HQL, ApplicationKey);
            Bond_DAO[] bonds = q.Execute();

            if (bonds != null && bonds.Length > 0)
            {
                IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                return BMTM.GetMappedType<IBond, Bond_DAO>(bonds[0]);
            }
            return null;
        }
    }
}
