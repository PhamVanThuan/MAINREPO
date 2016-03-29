using System.ComponentModel.DataAnnotations;

namespace SAHL.Core.Tests.Validation.Fakes.DeeplyNested
{
    public class FakeValidationCompositeDeeplyNested
    {
        [Required]
        public string Id { get; set; }

        public FakeValidationCompositeDeeplyNestedNode1 Node1 { get; set; }

        public static FakeValidationCompositeDeeplyNested Create()
        {
            return new FakeValidationCompositeDeeplyNested
            {
                Node1 = new FakeValidationCompositeDeeplyNestedNode1
                {
                    Node2 = new FakeValidationCompositeDeeplyNestedNode2
                    {
                        Node3 = new FakeValidationCompositeDeeplyNestedNode3
                        {
                            Node4 = new FakeValidationCompositeDeeplyNestedNode4(),
                        }
                    }
                }
            };
        }
    }

    public class FakeValidationCompositeDeeplyNestedNode1
    {
        [Required]
        public string Id1 { get; set; }

        public FakeValidationCompositeDeeplyNestedNode2 Node2 { get; set; }
    }

    public class FakeValidationCompositeDeeplyNestedNode2
    {
        [Required]
        public string Id2 { get; set; }

        public FakeValidationCompositeDeeplyNestedNode3 Node3 { get; set; }
    }

    public class FakeValidationCompositeDeeplyNestedNode3
    {
        [Required]
        public string Id3 { get; set; }

        public FakeValidationCompositeDeeplyNestedNode4 Node4 { get; set; }
    }

    public class FakeValidationCompositeDeeplyNestedNode4
    {
        [Required]
        public string Id4 { get; set; }
    }
}