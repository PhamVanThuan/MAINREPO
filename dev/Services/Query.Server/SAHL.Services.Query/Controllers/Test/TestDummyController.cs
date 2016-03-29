using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SAHL.Services.Query.Controllers.Test;
using WebApi.Hal;

namespace SAHL.Services.Query.Controllers.TestDummy
{
    [ServiceGenerationToolExclude]
    public class TestDummyRepresentation : Representation
    {
        public int OrganisationStructureKey { get; set; }
        public int ParentKey { get; set; }
        public string OrganisationType { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public List<TestDummyRepresentation> Children { get; set; }

        public override string Href
        {
            get
            {
                var href = base.Href;
                return href;
            }
        }

        public override string Rel
        {
            get
            {
                var rel = base.Rel;
                return rel;
            }
        }
    }

    [ServiceGenerationToolExclude]
    public class TestDummyController : ApiController
    {
        [RepresentationRoute("/api/Fake", "TestDummyListRepresentation_root", typeof(TestDummyRepresentation))]
        public TestDummyRepresentation Get()
        {
            var testDummyFactory = new TestDummyFactory();
            var dummyData = testDummyFactory.FetchDummyDataForParent(1);

            return dummyData;
        }

        [RepresentationRoute("/api/Fake/{id}", typeof(TestDummyRepresentation))]
        public TestDummyRepresentation Get(int id)
        {
            var testDummyFactory = new TestDummyFactory();
            var dummyData = testDummyFactory.FetchDummyDataForParent(id);

            return dummyData;
        }
    }

    public class TestDummyFactory
    {
        public TestDummyRepresentation FetchDummyDataForParent(int i)
        {
            var items = new List<TestDummyRepresentation>
            {
                new TestDummyRepresentation
                {
                    OrganisationStructureKey = 6,
                    ParentKey = 1,
                    OrganisationType = "Division",
                    Description = "Divisional Sales",
                    Level = 1
                },
                new TestDummyRepresentation
                {
                    OrganisationStructureKey = 7,
                    ParentKey = 1,
                    OrganisationType = "Division",
                    Description = "EXCO",
                    Level = 1
                },
                new TestDummyRepresentation
                {
                    OrganisationStructureKey = 192,
                    ParentKey = 1,
                    OrganisationType = "Division",
                    Description = "Finance",
                    Level = 1
                },
                new TestDummyRepresentation
                {
                    OrganisationStructureKey = 560,
                    ParentKey = 1,
                    OrganisationType = "Division",
                    Description = "Human Resources",
                    Level = 1
                },
                new TestDummyRepresentation
                {
                    OrganisationStructureKey = 561,
                    ParentKey = 1,
                    OrganisationType = "Division",
                    Description = "Information Technology",
                    Level = 1
                },
                new TestDummyRepresentation
                {
                    OrganisationStructureKey = 132,
                    ParentKey = 1,
                    OrganisationType = "Division",
                    Description = "Loss Control",
                    Level = 1
                },
                new TestDummyRepresentation
                {
                    OrganisationStructureKey = 556,
                    ParentKey = 1,
                    OrganisationType = "Division",
                    Description = "Operations",
                    Level = 1
                }
            };

            var root = new TestDummyRepresentation();
            root.Children = items.Where(a => a.ParentKey == i).ToList();
            return root;
        }
    }
}
