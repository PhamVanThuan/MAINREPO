using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.BDDfy;
using FluentAssertions;

namespace SAHL.Services.CATS.Server.Specs.Managers.CATsFileSpecs
{
    public class TestHelper
    {
        public static void TheDataAtPositionShouldBe(string text, int startPosition, int length, string value)
        {
            text.Substring(startPosition - 1, length).Should().Be(value);
        }
    }
}
