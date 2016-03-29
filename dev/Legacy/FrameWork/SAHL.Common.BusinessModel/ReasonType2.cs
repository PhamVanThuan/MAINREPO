using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;
namespace SAHL.Common.BusinessModel
{
    public partial class ReasonType : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ReasonType_DAO>, IReasonType
	{
        //public static IReadOnlyEventList<IReasonType> GetByReasonTypeGroup(IDomainMessageCollection Messages, int[] ReasonTypeGroupKeys)
        //{
        //    if (Messages == null)
        //        throw new Exception();

        //    if (ReasonTypeGroupKeys.Length == 0)
        //        throw new Exception();

        //    //string keys = "";

        //    //for (int i = 0; i < ReasonTypeGroupKeys.Length; i++)
        //    //{
        //    //    keys += "," + ReasonTypeGroupKeys[i].ToString();
        //    //}

        //    //keys = keys.Remove(0,1);

        //    string HQL = "Select rt from ReasonType_DAO rt join rt.ReasonTypeGroup as rtg where rtg.Key in (:keys)";
        //    SimpleQuery<ReasonType_DAO> q = new SimpleQuery<ReasonType_DAO>(HQL);
        //    q.SetParameterList("keys", ReasonTypeGroupKeys);
        //    ReasonType_DAO[] res = q.Execute();

        //    IEventList<IReasonType> list = new DAOEventList<ReasonType_DAO, IReasonType, ReasonType>(res);
        //    return new ReadOnlyEventList<IReasonType>(list);
        //}

		protected void OnReasonDefinitions_BeforeAdd(ICancelDomainArgs args, object Item)
		{

		}

		protected void OnReasonDefinitions_BeforeRemove(ICancelDomainArgs args, object Item)
		{

		}
	}
}


