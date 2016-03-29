using SAHL.Services.Interfaces.Query.Connector;
using SAHL.Services.Interfaces.Query.Connector.Enums;

namespace SAHL.Services.Query.Connector.Core
{
    public class WhereClauseOperatorPart : IWhereClauseOperatorPart
    {
        public WhereClauseOperatorPart(IQuery query)
        {
            this.query = query;
        }

        public ClauseOperator ClauseOperator { get; private set; }
        public IWhereOperatorPart OperatorPart { get; private set; }

        public IQuery query { get; private set; }

        public IWhereOperatorPart And()
        {
            this.ClauseOperator = ClauseOperator.And;
            return CreateWhere();
        }

        public IWhereOperatorPart Or()
        {
            this.ClauseOperator = ClauseOperator.Or;
            return CreateWhere();
        }

        public string AsString()
        {
            return this.ClauseOperator.ToString().ToLower() + ": {" + this.OperatorPart.AsString() + "}";
        }

        private IWhereOperatorPart CreateWhere()
        {
            this.OperatorPart = new WhereOperatorPart(query);
            return this.OperatorPart;
        }
    }
}
