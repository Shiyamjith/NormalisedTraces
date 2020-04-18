using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using NormaliseTrace.Application;
using NormaliseTrace.Infrastructure;

namespace NormaliseTrace
{
    internal class Program
    {
        public static bool HaveError;

        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions)
                .WithNotParsed(HandleParseError);

            if (HaveError)
            {
                var helpText = HelpText.AutoBuild(result, h => HelpText.DefaultParsingErrorsHandler(result, h), e => e);
                Console.WriteLine(helpText);
                ShowHelp();
            }
        }

        static void RunOptions(Options o)
        {
            ValidateOptions(o);
            if(HaveError)
                return;

            var fileReaderStrategy = new FileReaderStrategy();
            var alphaFileReader    = new AlphaFileReader(fileReaderStrategy);
            var alphaFileWriter    = new AlphaFileWriter();

            if (o.GoodFiles.Any())
            {
                Console.WriteLine();
                Console.WriteLine("Reading good trace files");
                var data = alphaFileReader.ParseFolders(o.GoodFiles);
                var result = TraceHelper.AverageColumns(data, o.PercentageToKeep);
                alphaFileWriter.WriteGoodFile(result);
            }

            if (o.BadFiles.Any())
            {
                Console.WriteLine();
                Console.WriteLine("Reading bad trace files");
                var data = alphaFileReader.ParseFolders(o.BadFiles);
                var result = TraceHelper.AverageColumns(data, o.PercentageToKeep);
                alphaFileWriter.WriteBadFile(result);

                Console.WriteLine();
                Console.WriteLine("Read in good trace file to calculate delta");
                var good = alphaFileReader.ReadGoodFile();
                if (good != null)
                {
                    var calculateDelta = new CalculateDelta();
                    var delta = calculateDelta.Calculate(o.Columns, good.ToList(), result);
                    if(delta != null)
                        alphaFileWriter.WriteDeltaFile(delta);
                }
            }

            if(o.TraceFiles.Any() && !string.IsNullOrEmpty(o.OutputFolder))
            {
                Console.WriteLine();
                Console.WriteLine("Reading trace files");
                var delta            = alphaFileReader.ReadDeltaFile();
                var processTraceFile = new ProcessTraceFile(fileReaderStrategy, o.Columns, delta);
                processTraceFile.Process(o.OutputFolder, o.TraceFiles);
            }
        }

        private static void ValidateOptions(Options o)
        {
            if (o.Columns < 1)
                o.Columns = 504; // default

            if (o.PercentageToKeep == 0)
                o.PercentageToKeep = 50; // default

            if (o.PercentageToKeep < 1)
                o.PercentageToKeep = 1;

            if (o.PercentageToKeep > 99)
                o.PercentageToKeep = 99;

            if (!o.TraceFiles.Any() && !string.IsNullOrWhiteSpace(o.OutputFolder))
                HaveError = true;

            if (o.TraceFiles.Any() && string.IsNullOrWhiteSpace(o.OutputFolder))
                HaveError = true;
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            HaveError = true;
        }

        private static void ShowHelp()
        {
            Console.WriteLine("This application normalises traces.");
            Console.WriteLine("Source code and documentation over on GitHub");
            Console.WriteLine("https://github.com/Shiyamjith/NormalisedTraces");
            Console.WriteLine();
            Console.WriteLine("Instructions for use:");
            Console.WriteLine("1. When a new detector has been installed, run an alpha trace on it to obtain a 'good' trace.");
            Console.WriteLine("   Run this application using the -g|-good parameter and passing the filename of the good trace.");
            Console.WriteLine("   This will read in and store the good trace with this application.");
            Console.WriteLine("2. At the start of a day, run an alpha trace to obtain a 'bad' (usually only slightly damaged) trace.");
            Console.WriteLine("   Run this application using the -b|-bad parameter and passing the filename of the daily bad trace.");
            Console.WriteLine("   This application will then create a delta file of the differences between the good and bad trace.");
            Console.WriteLine("3. The application is now ready for use.");
            Console.WriteLine("   Run this application using the -n|-normalise parameter and passing the filename of the trace to be normalised.");
            Console.WriteLine("   Also specify the -o|-output parameter passing in the name of the folder to write output to.");
            Console.WriteLine();
        }
    }
}
