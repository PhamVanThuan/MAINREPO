using Machine.Fakes;
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Tools.DomainServiceDocumenter.Lib.Specs
{
    [Ignore("Issues with the output directories not existing")]
    public class when_generating_objects : WithFakes
    {
        static Documenter documenter;
        Establish context = () =>
        {
            documenter = new Documenter();
        };

        Because of = () =>
        {
            //string dir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestData");
            //string outdir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestOutput");

            string dir = @"D:\git\dev-b\Tools\General\src\SAHL.Tools.DomainServiceDocumenter.Lib.Specs\TestData";
            string outdir = @"D:\git\dev-b\Tools\General\src\SAHL.Tools.DomainServiceDocumenter.Lib.Specs\TestOutput";
            documenter.GenerateObjects("Application", dir, outdir);
        };

        It should_ = () =>
        {

        };
    }
}
