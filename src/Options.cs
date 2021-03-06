﻿using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace NormaliseTrace
{
    public class Options
    {
        //[Option('s', "smooth", Group = "Options", Required = false, HelpText = "If ringing/noise is seen in the graph, this value is used to smooth out a window of n values. Defaults to 0.")]
        //public int Smooth { get; set; }

        [Option('r', "rise", Group = "Options", Required = false, HelpText = "Identity sharp rise in graph by looking for a difference of this much between two consecutive values in the data. Defaults to 20.")]
        public int Rise { get; set; }

        [Option('p', "percent", Group = "Options", Required = false, HelpText = "Percentage (1 to 99) of the data to keep. Defaults to keep centre 50% if not specified.")]
        public int PercentageToKeep { get; set; }

        [Option('g', "good", Group = "Options", Required = false, HelpText = "Reads in a good alpha trace files and stores it as the new reference trace. Use this only when the detector is quite new.")]
        public IEnumerable<string> GoodFiles { get; set; }

        [Option('b', "bad", Group = "Options", Required = false, HelpText = "Reads in a bad alpha trace files and calculates a delta file to use to normalise traces. Ideally do this every day, or at least once a week.")]
        public IEnumerable<string> BadFiles { get; set; }

        [Option('n', "normalise", Group = "Options", Required = false, HelpText = "The trace files to be normalised. Also specify the -o parameter.")]
        public IEnumerable<string> TraceFiles { get; set; }

        [Option('o', "output", Group = "Options", Required = false, HelpText = "The output folder to write the normalised traces. If an output file already exists, it will be overwritten.")]
        public string OutputFolder { get; set; }

        [Usage(ApplicationAlias = "NormaliseTrace")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example> 
                {
                    new Example("Read in good traces", new Options { GoodFiles    = new[] {"c:\\traces\\good\\R3_Strip*.csv", "c:\\traces\\good\\R20*.csv" }}),
                    new Example("Read in bad traces",  new Options { BadFiles     = new[] { "c:\\traces\\bad\\R55_Strip*.csv", "c:\\traces\\bad\\R56*.csv" }}),

                    new Example("Normalise traces", new Options 
                    {
                        TraceFiles   = new[] { "c:\\traces\\March07\\Sham*.csv", "c:\\traces\\March08\\Sham*.csv" }, 
                        OutputFolder = "c:\\traces\\March\\Output"
                    })
                };
            }
        }
    }
}