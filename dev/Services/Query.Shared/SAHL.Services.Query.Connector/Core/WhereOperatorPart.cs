using SAHL.Services.Interfaces.Query.Connector;
using SAHL.Services.Interfaces.Query.Connector.Enums;

namespace SAHL.Services.Query.Connector.Core
{
    public class WhereOperatorPart : IWhereOperatorPart
    {
        private readonly IQuery query;
        
        public QueryOperator QueryOperator { get; private set; }
        public IWhereFieldPart WhereFieldPart { get; private set; }

        public WhereOperatorPart(IWhereFieldPart whereFieldPart, IQuery query)
        {
            this.WhereFieldPart = whereFieldPart;
            this.query = query;
        }

        public WhereOperatorPart(IQuery query)
        {
            this.query = query;
        }

        public IWhereFieldPart Equals()
        {
            this.QueryOperator = QueryOperator.Eq;
            return CreateFieldQuery();
        }

        public IWhereFieldPart IsGreaterThan()
        {
            this.QueryOperator = QueryOperator.Gt;
            return CreateFieldQuery();
        }

        public IWhereFieldPart IsLessThan()
        {
            this.QueryOperator = QueryOperator.Lt;
            return CreateFieldQuery();
        }

        public IWhereFieldPart IsGreaterThanEqualTo()
        {
            this.QueryOperator = QueryOperator.Gte;
            return CreateFieldQuery();
        }

        public IWhereFieldPart IsLessThanEqualTo()
        {
            this.QueryOperator = QueryOperator.Lte;
            return CreateFieldQuery();
        }

        public IWhereFieldPart Like()
        {
            this.QueryOperator = QueryOperator.Like;
            return CreateFieldQuery();
        }

        public IWhereFieldPart StartsWith()
        {
            this.QueryOperator = QueryOperator.StartsWith;
            return CreateFieldQuery();
        }

        public IWhereFieldPart EndsWith()
        {
            this.QueryOperator = QueryOperator.EndsWith;
            return CreateFieldQuery();
        }

        public IWhereFieldPart Contains()
        {
            this.QueryOperator = QueryOperator.Contains;
            return CreateFieldQuery();
        }

        public string AsString()
        {
            return this.QueryOperator.ToString().ToLower() + ": {" + this.WhereFieldPart.AsString() + "}";
        }

        private IWhereFieldPart CreateFieldQuery()
        {
            this.WhereFieldPart = new WhereFieldPart(query);
            return this.WhereFieldPart;
        }
    }
}
