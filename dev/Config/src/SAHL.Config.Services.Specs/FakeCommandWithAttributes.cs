using System;
using System.ComponentModel.DataAnnotations;
using SAHL.Core.Services;

namespace SAHL.Config.Core.Specs
{
    public class FakeCommandWithAttributes : ServiceCommand
    {
        [Required (ErrorMessage = "Key is required")]
        public string Key { get; set; }
    }
}