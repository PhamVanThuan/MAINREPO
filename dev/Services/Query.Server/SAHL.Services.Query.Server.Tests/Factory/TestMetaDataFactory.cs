using NSubstitute;
using SAHL.Services.Query.Server.Tests.Representations;

namespace SAHL.Services.Query.Server.Tests.Factory
{
    public static class TestMetaDataFactory
    {
        
        private static ILinkResolver linkResolver;

        public static TestRepresentation CreateTestRepresentation()
        {
            linkResolver = Substitute.For<ILinkResolver>();

            return new TestRepresentation(linkResolver)
            {
                Id = 1,
                Count = 2,
                Description = "Test Item 1",
                Name = "Test Name 1"
            };
        }    
    }
}