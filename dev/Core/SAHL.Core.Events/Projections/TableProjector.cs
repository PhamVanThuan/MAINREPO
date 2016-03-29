using SAHL.Core.Data;
using SAHL.Core.Services;
using System;

namespace SAHL.Core.Events.Projections
{
    public class TableProjector<TEvent, TDataModel> : ITableProjector<TEvent, TDataModel>
        where TEvent : IEvent
        where TDataModel : IDataModel
    {
        private ITableProjector<TEvent, TDataModel> innerEventProjector;
        private IDbFactory dbFactory;

        public string TableName { get; protected set; }

        public string Schema { get; protected set; }

        public TableProjector(ITableProjector<TEvent, TDataModel> innerEventProjector, IDbFactory dbFactory)
        {
            if (String.IsNullOrEmpty(TableName))
            {
                TableName = innerEventProjector.GetType().Name;
            }
            if (String.IsNullOrEmpty(Schema))
            {
                Schema = "projection";
            }
            this.innerEventProjector = innerEventProjector;
            this.dbFactory = dbFactory;
        }

        public void Handle(TEvent @event, IServiceRequestMetadata metadata)
        {
            using (var db = this.dbFactory.NewDb().DDLInAppContext())
            {
                db.Create<TDataModel>(TableName, Schema);
            }
            this.innerEventProjector.Handle(@event, metadata);
        }
    }
}