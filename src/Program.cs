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
            if (o.PercentageToKeep == 0)
                o.PercentageToKeep = 50; // default

            if (o.PercentageToKeep < 1)
                o.PercentageToKeep = 1;
            
            if (o.PercentageToKeep > 99)
                o.PercentageToKeep = 99;

            if(!o.TraceFiles.Any() && !string.IsNullOrWhiteSpace(o.OutputFolder))
            {
                HaveError = true;
                return;
            }

            if(o.TraceFiles.Any() && string.IsNullOrWhiteSpace(o.OutputFolder))
            {
                HaveError = true;
                return;
            }

            var fileReader = new FileReader(new FileReaderStrategy());
            var fileWriter = new FileWriter();

            if (o.GoodFiles.Any())
            {
                Console.WriteLine();
                Console.WriteLine("Reading good trace files");
                var data = fileReader.ParseFolders(o.GoodFiles);
                var result = AverageColumns(data, o.PercentageToKeep);
                fileWriter.WriteGoodFile(result);
            }

            if (o.BadFiles.Any())
            {
                Console.WriteLine();
                Console.WriteLine("Reading bad trace files");
                var data = fileReader.ParseFolders(o.BadFiles);
                var result = AverageColumns(data, o.PercentageToKeep);
                fileWriter.WriteBadFile(result);

                Console.WriteLine();
                Console.WriteLine("Read in good trace file to calculate delta");
                var good = fileReader.ReadGoodFile();
                if (good != null)
                {
                    var calculateDelta = new CalculateDelta();
                    var delta = calculateDelta.Calculate(good, result);
                    if(delta != null)
                        fileWriter.WriteDeltaFile(delta);
                }
            }
        }

        private static List<int> AverageColumns(List<List<int>> data, int percentageToKeep)
        {
            Console.WriteLine($"Number of lines read in: {data.Count}");
            var transposedData = TraceHelper.Transpose(data);
            var result = new List<int>();
            // Because the data has been rotated, we iterate the rows, which are in fact columns
            foreach (var column in transposedData)
            {
                result.Add(TraceHelper.AverageCentre(column, percentageToKeep));
            }

            return result;
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
