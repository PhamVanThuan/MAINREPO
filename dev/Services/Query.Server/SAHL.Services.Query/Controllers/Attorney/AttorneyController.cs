using System.Net.Http;
using System.Web.Http;
using SAHL.Core.Data;
using SAHL.Services.Interfaces.Query.Coordinators;
using SAHL.Services.Query.Coordinators;
using SAHL.Services.Query.DataManagers;
using SAHL.Services.Query.DataManagers.Statements;
using SAHL.Services.Query.Factories;
using SAHL.Services.Query.Metadata;
using SAHL.Services.Query.Models.Attorney;
using SAHL.Services.Query.Resources.Attorney;
using WebApi.Hal;

namespace SAHL.Services.Query.Controllers.Attorney
{
    public class AttorneyController : QueryServiceBaseController
    {
        public AttorneyController(IQueryCoordinator queryCoordinator, IQueryFactory queryFactory, IDbFactory dbFactory, IDataModelCoordinator dataModelCoordinator)
            : base(queryCoordinator, queryFactory, new QueryServiceDataManager<AttorneyDataModel, GetAttorneyStatement>(dbFactory, dataModelCoordinator))
        {
        }

        [RepresentationRoute("/api/attorneys/count", typeof(CountRepresentation), "Count")]
        [HttpGet]
        public override HttpResponseMessage Count()
        {
            return base.Count();
        }

        [RepresentationRoute("/api/attorneys", typeof(AttorneyListRepresentation))]
        public override HttpResponseMessage Get()
        {
            return base.Get();
        }

        [RepresentationRoute("/api/attorneys/{id}", typeof(AttorneyRepresentation))]
        public override HttpResponseMessage Get(string id)
        {
            return base.Get(id);
        }
    }

    //TODO: replace this with a real representation
    public class CountRepresentation : Representation
    {
        public int? Count { get; set; }

        public override string Rel { get; set; }
        public override string Href { get; set; }
    }
} 