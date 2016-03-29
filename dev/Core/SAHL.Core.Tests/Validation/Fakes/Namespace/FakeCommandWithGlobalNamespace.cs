using System.ComponentModel.DataAnnotations;

internal class FakeCommandWithGlobalNamespace
{
    [Required]
    public string Key { get; set; }
}

internal class FakeCommandWithGlobalNamespaceComposite
{
    public FakeCommandWithGlobalNamespace Item { get; set; }
}

namespace Capitec.Validation.Fakes
{
    internal class CapitecFakeCommandSingleProperty
    {
        [Required]
        public string Key { get; set; }
    }

    internal class CapitecFakeCommandComposite
    {
        public CapitecFakeCommandSingleProperty Item { get; set; }
    }
}