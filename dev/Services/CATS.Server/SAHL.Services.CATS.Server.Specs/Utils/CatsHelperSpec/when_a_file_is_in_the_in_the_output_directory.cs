using Machine.Fakes;
using Machine.Specifications;
using SAHL.Services.CATS.ConfigExtension;
using SAHL.Services.CATS.Managers;
using SAHL.Services.CATS.Managers.CATS;
using System.IO.Abstractions;

namespace SAHL.Services.CATS.Server.Specs.Utils.CatsHelperSpec
{
    public class when_a_file_is_in_the_in_the_output_directory : WithFakes
    {
        private static ICATSManager catsHelper;
        private static string filename;
        private static ICATSDataManager catsDataManager;
        private static IFileSystem fileSystem;
        private static ICatsAppConfigSettings catsConfigSsettings;
        private static bool results;

        private Establish context = () =>
         {
             catsDataManager = An<ICATSDataManager>();
             fileSystem = An<IFileSystem>();
             catsConfigSsettings = An<ICatsAppConfigSettings>();
             catsHelper = new CATSManager(catsDataManager, fileSystem, catsConfigSsettings);
             filename = "fileName";

             catsConfigSsettings.WhenToldTo(x => x.CATSInputFileLocation).Return("c:\\temp\\IN\\");
             catsConfigSsettings.WhenToldTo(x => x.CATSOutputFileLocation).Return("c:\\temp\\OUT\\");
             catsConfigSsettings.WhenToldTo(x => x.CATSFailureFileLocation).Return("c:\\temp\\sendfailure\\");
             fileSystem.File.WhenToldTo(x => x.Exists(catsConfigSsettings.CATSInputFileLocation + filename)).Return(false);
             fileSystem.File.WhenToldTo(x => x.Exists(catsConfigSsettings.CATSOutputFileLocation + filename)).Return(true);
             fileSystem.File.WhenToldTo(x => x.Exists(catsConfigSsettings.CATSFailureFileLocation + filename)).Return(false);
         };

        private Because of = () =>
         {
             results = catsHelper.HasFileFailedProcessing(filename);
         };

        private It should_check_if_the_file_is_in_the_output_directory = () =>
         {
             fileSystem.File.WasToldTo(x => x.Exists(catsConfigSsettings.CATSOutputFileLocation + filename));
         };

        private It should_return_true = () =>
         {
             results.ShouldBeTrue();
         };
    }
}