using SAHL.Services.Interfaces.Query.Connector;

namespace SAHL.Services.Query.Connector.Core
{
    public class WhereFieldPart : IWhereFieldPart
    {
        private readonly IQuery query;

        public WhereFieldPart(IQuery query)
        {
            this.query = query;
        }

        private string fieldName;
        private IWhereValuePart whereValuePart;

        public IWhereValuePart Field(string field)
        {
            this.fieldName = field;
            return CreateValuePart();
        }

        public string AsString()
        {
            return "'" + this.fieldName + "': " + this.whereValuePart.AsString();
        }

        private IWhereValuePart CreateValuePart()
        {
            this.whereValuePart = new WhereValuePart(this.query);
            return this.whereValuePart;
        }
    }
}
