using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using SAHL.Common.DataAccess;
using SAHL.Shared.Messages;

namespace SAHL.Common.BusinessModel.Specs
{
    public static class Extensions
    {
        public static void ShouldIndicateAFailedRule(this int actual)
        {
            actual.ShouldEqual(1);
        }

        public static void ShouldIndicateASuccessfulRule(this int actual)
        {
            actual.ShouldEqual(1);
        }

        public static bool SqlParametersCompare(this  SAHL.Common.DataAccess.ParameterCollection parameters, ParameterCollection otherParameters)
        {
            var collectionExpression = from p in parameters
                                       from o in otherParameters
                                       where p.Value == o.Value && p.ParameterName == o.ParameterName
                                       select p;

            return collectionExpression.Any();
        }

        public static bool LogMessageTypeCompare(this LogMessage myMessage, object enumToCompareTo)
        {
            if (string.Equals(myMessage.LogMessageType.ToString(), enumToCompareTo.ToString()))
            {
                return true;
            }
            return false;
        }
    }
}