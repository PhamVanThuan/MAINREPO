using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using System.Collections;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;
namespace SAHL.Common.BusinessModel
{
    public partial class TextStatement : BusinessModelBase<SAHL.Common.BusinessModel.DAO.TextStatement_DAO>, ITextStatement
	{
        public static IReadOnlyEventList<ITextStatement> GetTextStatementsForTypes(int[] TextStatementTypes)
        {
            //string TS = "";
            //for (int i = 0; i < TextStatementTypes.Length; i++)
            //{
            //    TS += TextStatementTypes[i];
            //    if (i < TextStatementTypes.Length - 1)
            //        TS += ", ";
            //}

            string HQL = "from TextStatement_DAO TS where TS.TextStatementType in (:types)";
            SimpleQuery<TextStatement_DAO> q = new SimpleQuery<TextStatement_DAO>(HQL);
            q.SetParameterList("types", TextStatementTypes);
            TextStatement_DAO[] result = q.Execute();
            return new ReadOnlyEventList<ITextStatement>(new DAOEventList<TextStatement_DAO, ITextStatement, TextStatement>(result));
        }
    }
}


