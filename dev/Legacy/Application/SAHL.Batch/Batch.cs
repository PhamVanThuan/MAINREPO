using System;
using CommandLine;
using CommandLine.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using SAHL.Common.Globals;
using SAHL.Common.Logging;
using SAHL.Communication;

namespace SAHL.Batch
{
    internal sealed class Options
    {
        [Option('e', "execute", Required = true, HelpText = "CapMailingHouseExtract / CapExtractBatch / BatchCapImport / BatchDataReport / DCAcceptedProposalShortfallsExtract")]
		public string Process { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var helpText = new HelpText();
            helpText.AddOptions(this, "CapMailingHouseExtract / CapExtractBatch / BatchCapImport / BatchDataReport / DCAcceptedProposalShortfallsExtract");
            return helpText.RenderParsingErrorsText(this, 1);
        }
    }

    public class Batch
    {
        public const string CapMailingHouseExtract = "CapMailingHouseExtract";
        public const string CapExtractBatch = "CapExtractBatch";
        public const string BatchCapImport = "BatchCapImport";
        public const string BatchDataReport = "BatchDataReport";
        public const string DCAcceptedProposalShortfallsExtract = "DCAcceptedProposalShortfallsExtract";

        private static int Main(string[] args)
        {
            try
            {
                var options = new Options();
                var parser = new Parser();
                if (!parser.ParseArguments(args, options))
                    Environment.Exit(1);

                int batchReturn = 0;

                string sCulture = System.Configuration.ConfigurationManager.AppSettings["CultureToUse"];
                var cultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(sCulture);

                //Custom settings
                cultureInfo.DateTimeFormat.DateSeparator = System.Configuration.ConfigurationManager.AppSettings["DateSeparator"];
                cultureInfo.DateTimeFormat.ShortDatePattern = System.Configuration.ConfigurationManager.AppSettings["ShortDatePattern"];
                cultureInfo.DateTimeFormat.ShortTimePattern = System.Configuration.ConfigurationManager.AppSettings["ShortTimePattern"];
                cultureInfo.DateTimeFormat.LongDatePattern = System.Configuration.ConfigurationManager.AppSettings["LongDatePattern"];
                cultureInfo.DateTimeFormat.LongTimePattern = System.Configuration.ConfigurationManager.AppSettings["LongTimePattern"];
                cultureInfo.NumberFormat.CurrencyDecimalSeparator = System.Configuration.ConfigurationManager.AppSettings["DecimalSeparator"];
                cultureInfo.NumberFormat.NumberDecimalSeparator = System.Configuration.ConfigurationManager.AppSettings["DecimalSeparator"];
                cultureInfo.TextInfo.ListSeparator = System.Configuration.ConfigurationManager.AppSettings["ListSeparator"];

                System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
                System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;

                //set up the message bus for logging and metrics
                var messageBus = new EasyNetQMessageBus(new EasyNetQMessageBusConfigurationProvider());
                LogPlugin.Logger = new MessageBusLogger(messageBus, new MessageBusLoggerConfiguration());
                MetricsPlugin.Metrics = new MessageBusMetrics(messageBus, new MessageBusMetricsConfiguration());

                LogPlugin.Logger.LogInfoMessage("Main", "Starting" + options.Process);

                switch (options.Process)
                {
                    case CapMailingHouseExtract:
                        {
                            CapMailingHouseExtract batch = new CapMailingHouseExtract();
                            batchReturn = batch.Run(BulkBatchTypes.CapMailingHouseExtract);
                            break;
                        }
                    case CapExtractBatch:
                        {
                            CapExtract_Batch batch = new CapExtract_Batch();
                            batchReturn = batch.Run(BulkBatchTypes.CapExtractClientList);
                            break;
                        }
                    case BatchCapImport:
                        {
                            CapImport_Batch batch = new CapImport_Batch();
                            batchReturn = batch.Run(BulkBatchTypes.CapImportClientList);
                            break;
                        }
                    case BatchDataReport:
                        {
                            DataReport_Batch batch = new DataReport_Batch();
                            batchReturn = batch.Run(BulkBatchTypes.DataReportBatch);
                            break;
                        }
                    case DCAcceptedProposalShortfallsExtract:
                        {
                            DCAcceptedProposalShortfallsExtract_Batch batch = new DCAcceptedProposalShortfallsExtract_Batch();
                            batch.Run(BulkBatchTypes.DCAcceptedProposalShortfallsExtract);
                            break;
                        }
                    default:
                        break;
                }

                LogPlugin.Logger.LogInfoMessage("Main", "Processing Complete" + options.Process);

                return batchReturn;
            }
            catch (Exception ex)
            {
                ExceptionPolicy.HandleException(ex, "UI Exception");
                return 0;
            }
            finally
            {
                Environment.Exit(0);
            }
        }
    }
}