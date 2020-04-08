using System;

namespace NormaliseTrace
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 2)
            {
                ShowHelp();
                return;
            }
        }

        private static void ShowHelp()
        {
            Console.WriteLine("This application normalises traces.");
            Console.WriteLine("NormaliseTrace <command> [filename]");
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
            Console.WriteLine("Available commands:");
            Console.WriteLine();
            Console.WriteLine("-g   Reads in a good alpha trace file and stores it as the new reference trace.");
            Console.WriteLine("     Use this only when the detector is quite new.");
            Console.WriteLine();
            Console.WriteLine("-b   Reads in a bad alpha trace file and calculates a delta file to use to normalise traces.");
            Console.WriteLine("     Ideally do this every day, or at least once a week.");
            Console.WriteLine();
            Console.WriteLine("-n   The trace file to be normalised. Also specify the -o parameter.");
            Console.WriteLine();
            Console.WriteLine("-o   The output folder to write the normalised traces. If an output file already exists, it will be overwritten.");
            Console.WriteLine();
            Console.WriteLine("Example:");
            Console.WriteLine("NormaliseTrace -g c:\\traces\\good\\R3_Strip*.csv");
            Console.WriteLine("NormaliseTrace -b c:\\traces\\bad\\R3_Strip*.csv");
            Console.WriteLine("NormaliseTrace -n c:\\traces\\March07\\Sham*.csv -o c:\\traces\\March07\\Output");
        }
    }
}
